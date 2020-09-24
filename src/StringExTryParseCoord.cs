using System;

namespace __Snippets__
{
    internal static partial class StringExtensions
    {
        internal static (bool success, double lat, double lng) TryParseLatitudeLongitude(this string str)
        {
            (bool, double, double) result = default;

            if (string.IsNullOrWhiteSpace(str) == false)
            {
#if (!NETSTANDARD && !NETCOREAPP)
                var parts = str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
#else
                var parts = str.Split(',', StringSplitOptions.RemoveEmptyEntries);
#endif
                if (parts.Length == 2)
                {
                    if (double.TryParse(parts[0].Trim(), out var lat)
                        && double.TryParse(parts[1].Trim(), out var lng))
                    {
                        result = (true, lat, lng);
                    }
                }
            }
            
            return result;
        }
    }
}
