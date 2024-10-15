
namespace Wos.Extensions
{
    public static class StringExtensions
    {

        public static string Highlight(this string value)
        {
            return !string.IsNullOrEmpty(value) ? $"[{value}]" : string.Empty;
        }

        public static string Quote(this string value, string before = "\"", string after = "\"")
        {
            return $"{before}{value}{after}";
        }

        public static TimeSpan ToTimeSpan(this string value)
        {
            if (string.IsNullOrEmpty(value)) return TimeSpan.Zero;

            TimeSpan time;
            if (TimeSpan.TryParse(value, out time))
                return time;
            return TimeSpan.Zero;
        }

    }

}
