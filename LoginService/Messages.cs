namespace LoginService
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
}