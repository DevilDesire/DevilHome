using System;
using System.Reflection;
using DevilHome.Common.Interfaces.Extensions;

namespace DevilHome.Common.Interfaces.Enums
{
    public static class StringEnum
    {
        public static string GetStringValue(this Enum value)
        {
            string output = "";
            Type type = value.GetType();

            FieldInfo fi = type.GetField(value.ToString());
            StringValue[] attrs = fi.GetCustomAttributes(typeof (StringValue), false) as StringValue[];

            if (attrs != null && attrs.Length > 0)
            {
                output = attrs[0].Value;
            }

            return output;
        }
    }
}