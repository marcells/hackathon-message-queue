using System;
using System.Linq;
using Infrastructure;

namespace ListService
{
    public class Handler : IHandler
    {
        public void Handle(AccountRegistered message)
        {
            if (Database.RunningPomodoros.Any(x => x.UserName == message.UserName))
                throw new InvalidOperationException();

            Database.Users.Add(new User
            {
                UserName = message.UserName,
                FirstName = message.FirstName
            });
        }

        public void Handle(AccountDeleted message)
        {
            var runningPomodoroToRemove = Database.RunningPomodoros.Single(x => x.UserName == message.UserName);
            Database.RunningPomodoros.Remove(runningPomodoroToRemove);

            var userToRemove = Database.Users.Single(x => x.UserName == message.UserName);
            Database.Users.Remove(userToRemove);
        }

        public void Handle(PomodoroStarted message)
        {
            if (Database.RunningPomodoros.Any(x => x.UserName == message.UserName))
                throw new InvalidOperationException();

            Database.RunningPomodoros.Add(new RunningPomodoro { UserName = message.UserName, StartTime = DateTime.Now });
        }

        public void Handle(PomodoroEnded message)
        {
            if (!Database.RunningPomodoros.Any(x => x.UserName == message.UserName))
                throw new InvalidOperationException();

            var runningPomodoroToRemove = Database.RunningPomodoros.Single(x => x.UserName == message.UserName);
            Database.RunningPomodoros.Remove(runningPomodoroToRemove);
        }

        public void Handle(object message)
        {
        }
    }
}