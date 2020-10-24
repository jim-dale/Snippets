using System;

namespace __Snippets__
{
    internal static partial class StringExtensions
    {
        internal static string WithEnvionmentVariables(this string str)
        {
            return Environment.ExpandEnvironmentVariables(str);
        }
    }
}
