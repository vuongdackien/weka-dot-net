using System;

namespace Weka.NET.Core
{
    public enum AttributeType
    {
        STRING
    }

    public class Attribute
    {
        public string Name { get; set; }

        public AttributeType Type { get; set; }

    }
}
