using System;
using System.Linq;
using Infrastructure;

namespace PomodoroService
{
    public class Handler : IHandler
    {
        public void Handle(AccountRegistered message)
        {
            if (Database.Users.Any(x => x.UserName == message.UserName))
                throw new InvalidOperationException();

            Database.Users.Add(new User { UserName = message.UserName });
        }

        public void Handle(AccountDeleted message)
        {
            var userToRemove = Database.Users.Single(x => x.UserName == message.UserName);

            Database.Users.Remove(userToRemove);
        }

        public void Handle(PomodoroStarted message)
        {
            Database.Users.Single(x => x.UserName == message.UserName).IsPomodoroActive = true;
        }

        public void Handle(PomodoroEnded message)
        {
            Database.Users.Single(x => x.UserName == message.UserName).IsPomodoroActive = false;
        }

        public void Handle(object message)
        {            
        }
    }
}