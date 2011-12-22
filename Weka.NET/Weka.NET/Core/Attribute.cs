namespace Weka.NET.Core
{
    using System;
    
    public abstract class Attribute
    {
        public const string DecodedMissingAttribute = "?";

        public const double EncodedMissingValue = double.NaN;

        public string Name { get; private set; }

        public abstract double Encode(string value);

        public abstract string Decode(double value);

        public Attribute(string name)
        {
            Name = name;
        }
    }
}
