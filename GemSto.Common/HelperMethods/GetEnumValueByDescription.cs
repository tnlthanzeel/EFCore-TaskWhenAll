using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GemSto.Common.HelperMethods
{
    public static class GetEnumValueByDescription
    {
        public static T GetValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) return default(T); // throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description.StartsWith(description))
                        return (T)field.GetValue(null);
                }
                //else
                //{
                //    if (field.Name.Contains(description))
                //        return (T)field.GetValue(null);
                //}
            }
             return default(T);
        }
    }
}
