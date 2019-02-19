using System;
using System.Collections.Generic;

namespace ListService
{
    public static class Database
    {
        public static List<RunningPomodoro> RunningPomodoros { get; } = new List<RunningPomodoro>();

        public static List<User> Users { get; } = new List<User>();
    }

    public class RunningPomodoro
    {
        public string UserName { get; set; }

        public DateTime? StartTime { get; set; }
    }

    public class User
    {
        public string UserName { get; set; }

        public string FirstName { get; set; }
    }
}