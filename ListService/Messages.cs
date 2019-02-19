namespace ListService
{
    public class AccountRegistered
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
    }

    public class AccountDeleted
    {
        public string UserName { get; set; }
    }

    public class PomodoroStarted
    {
        public string UserName { get; set; }
    }

    public class PomodoroEnded
    {
        public string UserName { get; set; }
    }
}