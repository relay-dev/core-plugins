namespace Core.Plugins.Extensions
{
    public static class ArrayExtensions
    {
        public static T[] AsSafe<T>(this T[] array) where T : new()
        {
            return array ?? new [] { new T() };
        }
    }
}
