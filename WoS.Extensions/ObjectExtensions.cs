using System.ComponentModel;
using System.Globalization;

namespace Wos.Extensions
{
    public static class ObjectExtensions
    {

        /// <summary>
        /// Converts an Object to the specified target type or returns the default value.
        /// </summary>
        /// <typeparam name="T">The type to convert to</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>The target type</returns>
        public static T ConvertTo<T>(this object value)
        {
            return ConvertTo<T>(value, (T)default);
        }

        /// <summary>
        /// Converts an Object to the specified target type or returns the default value.
        /// </summary>
        /// <typeparam name="T">The type to convert to</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="culture">The culture To convert from</param>
        /// <returns></returns>
        public static T ConvertTo<T>(this object value, CultureInfo culture)
        {
            return ConvertTo<T>(value, (T)default, culture);
        }

        /// <summary>
        /// Converts an Object to the specified target type or returns the default value.
        /// </summary>
        /// <typeparam name="T">The type to convert to</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static T ConvertTo<T>(this object value, T defaultValue)
        {
            return ConvertTo<T>(value, defaultValue, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Converts an Object to the specified target type or returns the default value.
        /// </summary>
        /// <typeparam name="T">The type to convert to</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="culture">The culture To convert from</param>
        /// <returns></returns>
        public static T ConvertTo<T>(this object value, T defaultValue, CultureInfo culture)
        {
            if (value is object && value != DBNull.Value && !(value is string && string.IsNullOrEmpty((string)value)))
            {
                var targetType = typeof(T);
                if (value is T)
                    return (T)value;                

                var converter = TypeDescriptor.GetConverter(value);
                if (converter is object)
                {
                    if (converter.CanConvertTo(targetType))
                        return (T)converter.ConvertTo(default, culture, value, targetType);                    
                }

                converter = TypeDescriptor.GetConverter(targetType);
                if (converter is object)
                {
                    if (converter.CanConvertFrom(value.GetType()))
                        return (T)converter.ConvertFrom(default, culture, value);
                }

                if (targetType.IsEnum)
                    return value.ToString().GetEnum(defaultValue);
            }

            return defaultValue;
        }

    }

}
