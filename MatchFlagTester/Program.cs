using System;
using MatchFlagLibrary;
using StackExchange.Redis;

namespace MatchFlagTester
{
    class Program
    {
        static void Main(string[] args)
        {
            // MatchFlag match = new MatchFlag(options =>
            // {
            //     options.RedisServer = "127.0.0.1";
            //     options.RedisPort = 6379;
            //     options.FlagCount = 4;
            //     options.AddFlagOption(0, 1000, 1,
            //         (key,index, after, expected) =>
            //         {
            //             Console.WriteLine($"Key {key} Flag {expected} not apear after {after} miliseconds of {index} apearence");
            //         },
            //         (key,index, after, expected) =>
            //         {
            //             Console.WriteLine($"Key {key} Flag {expected} apear after {after} miliseconds of {index} apearence");
            //         });
            // });


            var redis = ConnectionMultiplexer.Connect("127.0.0.1:6379");
            var db = redis.GetDatabase();
            byte[] bbb = db.StringGet("Ali");


            Console.ReadKey(true);
        }
    }
}
