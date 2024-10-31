namespace HrHub.Abstraction.Exceptions
{
    public class CacheNotFoundException : Exception
    {
        public CacheNotFoundException(string cacheKey)
            : base($"{cacheKey} is not found on cache")
        {
        }

        public CacheNotFoundException()
        {
        }

        public CacheNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
