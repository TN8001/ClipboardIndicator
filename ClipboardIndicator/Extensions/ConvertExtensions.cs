using System.ComponentModel;

namespace ClipboardIndicator
{
    internal static class ConvertExtensions
    {
        public static T ConvertFromString<T>(this T target, string value)
            => (T)TypeDescriptor.GetConverter(target.GetType()).ConvertFrom(value);

        public static string ConvertToString<T>(this T value)
            => (string)TypeDescriptor.GetConverter(value.GetType()).ConvertTo(value, typeof(string));
    }
}
