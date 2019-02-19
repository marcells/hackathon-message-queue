namespace Infrastructure
{
    public interface IHandler
    {
        void Handle(object message);
    }
}