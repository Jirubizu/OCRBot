using System.Text.RegularExpressions;

namespace OCRBot.Utilities
{
    public static class StringUtilities
    {
        public static string TypeFormat(string type)
        {
            var spacedType = Regex.Replace(type, "([A-Z])", " $1").Trim();
            return spacedType.Split(" ").Length > 1 ? type : Regex.Replace(type, @"[\d-]", string.Empty).ToLowerInvariant();
        }
    }
}