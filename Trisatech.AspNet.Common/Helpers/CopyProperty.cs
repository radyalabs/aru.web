using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;

namespace Trisatech.AspNet.Common.Helpers
{
    public static class CopyProperty
    {
        public static void CopyPropertiesTo<T1, T2>(T1 source, T2 dest)
        {
            List<PropertyInfo> sourceProps = typeof(T1).GetProperties().Where(x => x.CanRead).ToList();
            List<PropertyInfo> destProps = typeof(T2).GetProperties()
                    .Where(x => x.CanWrite)
                    .ToList();

            foreach (var sourceProp in sourceProps)
            {
                if (!IsHasIgnore(sourceProp) && destProps.Any(x => x.Name == sourceProp.Name))
                {
                    var p = destProps.First(x => x.Name == sourceProp.Name);
                    if (p.CanWrite)
                    { // check if the property can be set or no.
                        p.SetValue(dest, sourceProp.GetValue(source, null), null);
                    }
                }

            }
        }

        [AttributeUsage(AttributeTargets.Property)]
        public class CopyPropertyIgnore : System.Attribute
        {

        }

        public static bool IsHasIgnore(PropertyInfo prop)
        {
            object[] attrs = prop.GetCustomAttributes(true);
            foreach (object attr in attrs)
            {
                CopyPropertyIgnore authAttr = attr as CopyPropertyIgnore;
                if (authAttr != null)
                {
                    return true;
                }
            }
            return false;
        }
    }

}
