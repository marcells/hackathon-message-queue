using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Infrastructure;

namespace LoginService
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                var input = Console.ReadLine();

                var tokens = input.Split(' ');

                if (tokens[0] == "r")
                {
                    QueueHelper.Publish(new AccountRegistered
                    {
                        UserName = tokens[1],
                        FirstName = tokens[2]
                    }, "pomodoro.account.register");
                }

                if (tokens[0] == "d")
                {
                    QueueHelper.Publish(new AccountDeleted
                    {
                        UserName = tokens[1]
                    }, "pomodoro.account.delete");
                }
            }
        }
    }
}
