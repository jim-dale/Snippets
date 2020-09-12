using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace __Snippets__
{
    internal static partial class ObjectExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        internal static void SetValue(this object item, string name, object value)
        {
            Type type = item.GetType();

            //get the property information based on the type
            PropertyInfo propertyInfo = type.GetProperty(name);

            //find the property type
            Type propertyType = propertyInfo.PropertyType;

            //Convert.ChangeType does not handle conversion to nullable types
            //if the property type is nullable, we need to get the underlying type of the property
            var targetType = IsNullableType(propertyType) ? Nullable.GetUnderlyingType(propertyType) : propertyType;

            //Returns an System.Object with the specified System.Type and whose value is
            //equivalent to the specified object.
            value = Convert.ChangeType(value, targetType);

            //Set the value of the property
            propertyInfo.SetValue(item, value, null);
        }

        internal static bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }
    }
}
