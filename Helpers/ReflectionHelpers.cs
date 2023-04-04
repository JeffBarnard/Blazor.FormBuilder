using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Blazor.FormBuilderComponent.Helpers
{
    public class ReflectionHelpers
    {
        public static string GetDescription<T>(T enumValue)
        {
            var type = enumValue.GetType();
            var name = Enum.GetName(type, enumValue);
            if (name != null)
            {
                var field = type.GetField(name);
                if (field != null)
                {
                    var attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }

        public static void AssignDefaultValues(object obj)
        {
            var properties = obj.GetType().GetProperties();
            foreach (var property in properties)
            {
                // TODO only assign if value == default<T>
                //if (property.GetValue(obj) == null || property.GetValue(obj) == 0)
                //{
                    var defaultValue = GetDefaultValue(property);
                    if (defaultValue != null)
                    {
                        property.SetValue(obj, defaultValue);
                    }
                //}
            }
        }

        public static bool HasLabelAttribute(PropertyInfo propertyInfo)
        {   
            var displayAttribute = (DisplayAttribute)propertyInfo.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();
            return displayAttribute?.Name?.Length > 0;
        }

        public static string? GetLabelAttribute(PropertyInfo propertyInfo)
        {
            var attribute = (DisplayAttribute)propertyInfo.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();
            return attribute?.Name;
        }

        public static DataType GetDataTypeAttribute(PropertyInfo propertyInfo)
        {
            var attribute = (DataTypeAttribute)propertyInfo.GetCustomAttributes(typeof(DataTypeAttribute), false).FirstOrDefault();
            if (attribute != null)
                return attribute.DataType;
            
            return DataType.Text;
        }

        public static Type GetTypeFromName(string typeName)
        {
            var type = Type.GetType(typeName);
            if (type != null)
            {
                return type;
            }

            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = a.GetType(typeName);
                if (type != null)
                {
                    return type;
                }
            }

            return null;
        }

        public static object GetDefaultValue(PropertyInfo propertyInfo)
        {
            var defaultValueAttribute = propertyInfo.GetCustomAttribute<DefaultValueAttribute>();
            return defaultValueAttribute?.Value;
        }

        public static object GetDefaultValue2(PropertyInfo propertyInfo)
        {
            var defaultValueAttribute = propertyInfo.GetCustomAttributes(typeof(DefaultValueAttribute), false).FirstOrDefault() as DefaultValueAttribute;
            return defaultValueAttribute?.Value;
        }

        public static object GetDefaultValue3(PropertyInfo property)
        {
            var attributes = property.GetCustomAttributes(typeof(DefaultValueAttribute), false);
            if (attributes.Length > 0)
            {
                return ((DefaultValueAttribute)attributes[0]).Value;
            }
            return null;
        }

        public static string GetEnumValueDescription<T>(T enumValue) where T : Enum
        {
            var type = enumValue.GetType();
            var name = Enum.GetName(type, enumValue);
            if (name != null)
            {
                var field = type.GetField(name);
                if (field != null)
                {
                    var attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }

    }
}
