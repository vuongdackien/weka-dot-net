using System;

namespace Weka.NET.Core
{
    public class Attribute
    {
        public string Name { get; private set; }

        public AttributeType Type { get; private set; }

        public int Index { get; private set; }

        public Attribute(string name, AttributeType type, int index)
        {
            Name = name;
            Type = type;
            Index = index;
        }

    }

    public enum AttributeType
    {
        NUMERIC,
        NOMINAL,
        STRING
    }

    public static class AttributeTypeExt
    {
        public static bool IsNumeric(this AttributeType type)
        {
            return type == AttributeType.NUMERIC;
        }

        public static bool IsNominal(this AttributeType type)
        {
            return type == AttributeType.NOMINAL;
        }

        public static bool IsString(this AttributeType type)
        {
            return type == AttributeType.STRING;
        }
    }

}
