using System;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Infrastructure;
using System.Collections.Generic;

namespace PomodoroService
{
    class Program
    {
        static void Main(string[] args)
        {
            QueueHelper.ListenOn(
                new Handler(),
                "PomodoroService",
                new List<string> { "pomodoro.account.*", "pomodoro.action.*" });

            while (true)
            {
                var input = Console.ReadLine();

                var tokens = input.Split(' ');

                if (tokens[0] == "s")
                {
                    if (!Database.Users.Any(x => x.UserName == tokens[1]))
                        throw new InvalidOperationException();

                    if (Database.Users.Single(x => x.UserName == tokens[1]).IsPomodoroActive)
                        throw new InvalidOperationException();

                    QueueHelper.Publish(new PomodoroStarted { UserName = tokens[1] }, "pomodoro.action.start");
                }

                if (tokens[0] == "e")
                {
                    if (!Database.Users.Any(x => x.UserName == tokens[1]))
                        throw new InvalidOperationException();

                    if (!Database.Users.Single(x => x.UserName == tokens[1]).IsPomodoroActive)
                        throw new InvalidOperationException();

                    QueueHelper.Publish(new PomodoroEnded { UserName = tokens[1] }, "pomodoro.action.end");
                }
            }
        }
    }
}
