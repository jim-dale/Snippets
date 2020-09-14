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
                var parts = str.Split(',', StringSplitOptions.RemoveEmptyEntries);
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
