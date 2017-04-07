using System;

namespace DevilHome.Common.Interfaces.Extensions
{
    public class StringValue : Attribute
    {
        public StringValue(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }
}