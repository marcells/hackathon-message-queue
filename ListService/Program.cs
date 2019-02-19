using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure;

namespace ListService
{
    class Program
    {
        static void Main(string[] args)
        {
            QueueHelper.ListenOn(
                new Handler(), 
                "ListService", 
                new List<string> { "pomodoro.account.*", "pomodoro.action.*" });

            while (true)
            {
                var input = Console.ReadLine();

                var tokens = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (tokens[0] == "l")
                {
                    Database.RunningPomodoros.ForEach(x => Console.WriteLine($"{x.UserName:-10} {Database.Users.Single(y => y.UserName == x.UserName).FirstName:-10} {x.StartTime}"));
                }
            }
        }
    }
}
