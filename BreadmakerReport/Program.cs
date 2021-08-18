using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using RatingAdjustment.Services;
using BreadmakerReport.Models;
using Microsoft.EntityFrameworkCore.Internal;
using System.Threading;
using System.ComponentModel;

namespace BreadmakerReport
{
    class Program
    {
        static string dbfile = @".\data\breadmakers.db";
        static RatingAdjustmentService rasObject = new RatingAdjustmentService();

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Bread World");            
            var BreadmakerDb = new BreadMakerSqliteContext(dbfile);
            var BMList = BreadmakerDb.Breadmakers                
                .Select(bm => new { review = bm.Reviews.Count
                                    , avg = Math.Round(bm.Reviews.Average(r => r.stars), 2)
                                    , adjust = Math.Round(rasObject.Adjust(bm.Reviews.Average(r => r.stars) , bm.Reviews.Count), 2)
                                    , title = bm.title
                })          
                .AsEnumerable()
                .OrderByDescending(bm => bm.adjust)
                .ToList();

            Console.WriteLine("\n-------------------------------------------\n[#]  Reviews Average  Adjust    Description\n-------------------------------------------");
            for (var j = 0; j < 3; j++)
            {
                var i = BMList[j];
                // TODO: add output                
                Console.WriteLine($"[{j+1}] {i.review,8} {i.avg, -2:F2} {i.adjust, 8:F2}      {i.title}");
            }
        }
    }
}
