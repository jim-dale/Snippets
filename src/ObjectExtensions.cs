using System;
using System.Reflection;

namespace __Snippets__
{
    internal static partial class ObjectExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item">The target object to set the property value for.</param>
        /// <param name="name">The name of the property to set on the target object.</param>
        /// <param name="value">The value of the property to set on the target object.</param>
        internal static void SetValue(this object item, string name, object value)
        {
            Type type = item.GetType();
            PropertyInfo propertyInfo = type.GetProperty(name);
            Type propertyType = propertyInfo.PropertyType;

            //Convert.ChangeType does not handle conversion to nullable types
            //if the property type is nullable, we need to get the underlying type of the property
            var targetType = IsNullableType(propertyType) ? Nullable.GetUnderlyingType(propertyType) : propertyType;

            value = Convert.ChangeType(value, targetType);

            propertyInfo.SetValue(item, value, null);
        }

        internal static bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }
    }
}
