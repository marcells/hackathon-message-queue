using System;
using System.Collections.Generic;

namespace PomodoroService
{
    public static class Database
    {
        public static List<User> Users { get; } = new List<User>();
    }

    public class User 
    {
        public string UserName { get; set; }

        public bool IsPomodoroActive { get; set; }
    }
}