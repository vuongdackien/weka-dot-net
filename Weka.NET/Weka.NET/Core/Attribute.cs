using System;

namespace Weka.NET.Core
{
    public abstract class Attribute
    {
        public string Name { get; private set; }

        public int Index { get; private set; }

        public abstract double? Encode(string value);

        public abstract string Decode(double? value);

        public Attribute(string name, int index)
        {
            Name = name;
            Index = index;
        }

    }

}
