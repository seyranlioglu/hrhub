namespace HrHub.Cache.Models
{
    public class CacheItem<T> : IEquatable<CacheItem<T>>
           where T : class, new()
    {
        public CacheItem()
        {
            this.Key = typeof(T).Name;
        }

        public CacheItem(T item)
        {
            this.Key = typeof(T).Name;
            this.Value = item;
            this.Hash = this.Value.GetHashCode();
        }

        public string Key { get; set; }

        public T Value { get; set; }

        public int Hash { get; set; }

        public bool Equals(CacheItem<T> other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.Key == other.Key && this.Hash == other.Hash;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return this.Equals((CacheItem<T>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((this.Key != null ? this.Key.GetHashCode(StringComparison.CurrentCulture) : 0) * 397) ^ (this.Hash > 0 ? this.Hash.GetHashCode() : 0);
            }
        }
    }
}
