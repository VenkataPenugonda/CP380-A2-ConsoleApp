using System;

namespace RatingAdjustment.Services
{
    /** Service calculating a star rating accounting for the number of reviews
     * 
     */
    public class RatingAdjustmentService
    {
        const double MAX_STARS = 5.0;  // Likert scale
        const double Z = 1.96; // 95% confidence interval

        double _q;
        double _percent_positive;

        /** Percentage of positive reviews
         * 
         * In this case, that means X of 5 ==> percent positive
         * 
         * Returns: [0, 1]
         */
        void SetPercentPositive(double stars)
        {
            this._percent_positive = stars / RatingAdjustmentService.MAX_STARS;
        }

        /**
         * Calculate "Q" given the formula in the problem statement
         */
        void SetQ(double number_of_ratings)
        {
            double left_half = this._percent_positive * (1 - this._percent_positive);
            double power = Math.Pow(RatingAdjustmentService.Z, 2);
            double multiply = 4 * number_of_ratings;
            double ans1 = (left_half + (power / multiply));
            double final_result = ans1 / number_of_ratings;
            this._q = RatingAdjustmentService.Z * Math.Sqrt(final_result);
        }

        /** Adjusted lower bound
         * 
         * Lower bound of the confidence interval around the star rating.
         * 
         * Returns: a double, up to 5
         */
        public double Adjust(double stars, double number_of_ratings) {
            
            this.SetPercentPositive(stars);
            this.SetQ(number_of_ratings);

            double power = Math.Pow(RatingAdjustmentService.Z, 2);
            double left_half = (this._percent_positive + ( power / (2 * number_of_ratings)) - this._q);
            double right_half = (1 + (power / number_of_ratings));
            double final_result = left_half / right_half * RatingAdjustmentService.MAX_STARS;

            return final_result;
        }
    }
}
