
namespace __Snippets__
{
    internal static partial class ReflectionHelper
    {
        public static bool TryGetPropertyValue(object obj, string propertyName, out object result)
        {
            bool success = false;
            result = default;

            var propInfo = obj.GetType().GetProperty(propertyName);
            if (propInfo != null)
            {
                success = true;
                result = propInfo.GetValue(obj, null);
            }

            return success;
        }

        public static object GetPropertyValue(object obj, string propertyName)
        {
            var propInfo = obj.GetType().GetProperty(propertyName);
            return propInfo?.GetValue(obj, null);
        }
    }
}
