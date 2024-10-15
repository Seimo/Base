
namespace Wos.Extensions
{
    public static class EnumExtensions
    {

        public static T GetEnum<T>(this string entryName, T defaultValue)
        {
            try
            {
                if (string.IsNullOrEmpty(entryName))
                    return defaultValue;
                return (T)Enum.Parse(typeof(T), entryName, true);
            }
            catch
            {
                return defaultValue;
            }
        }

    }

}
