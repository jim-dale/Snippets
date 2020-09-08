using System;
using System.Text.RegularExpressions;

namespace {{TargetNameSpace}}
{
    public static partial class StringExtensions
    {
        public static (bool success, double lat, double lng) TryParseCoordinates(this string str)
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
