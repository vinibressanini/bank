namespace desafioAPI.Bus
{
    public interface IBus<T> where T : class
    {
        Task Publish(T message);
    }
}
