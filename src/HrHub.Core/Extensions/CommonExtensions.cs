namespace HrHub.Core.Extensions
{
    public static class CommonExtensions
    {
        public static T? NullIf<T>(this T value, Predicate<T> condition) where T : struct
        {
            return condition(value) ? null : value;
        }
    }
}
