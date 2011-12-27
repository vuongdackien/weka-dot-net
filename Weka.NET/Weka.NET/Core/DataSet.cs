namespace Weka.NET.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using System;
    using System.Text;
    
    public interface IDataSet
    {
        string RelationName { get; }

        IList<Attribute> Attributes { get; }

        IList<Instance> Instances { get; }

        int Count { get; }

        bool ContainsNominalAttributesOnly();

        bool ContainsMissingValues();
    }

    public class DataSet : IDataSet
    {
        public string RelationName { get; private set; }

        public IList<Attribute> Attributes { get; private set; }

        public IList<Instance> Instances { get; private set; }

        public DataSet(string name, IEnumerable<Attribute> attributes, IEnumerable<Instance> instances)
        {
            RelationName = name;

            if (attributes.Count() == 0)
            {
                throw new ArgumentException("Can't construct a data set with no attributes");
            }
            Attributes = attributes.ToList().AsReadOnly();

            Instances = instances.ToList().AsReadOnly();
        }

        public int Count { get { return Instances.Count; } }

        public override string ToString()
        {
            var buff = new StringBuilder();
            buff.Append("DataSet[Name=").Append(RelationName);
            buff.Append(", attributes=").Append(Attributes.Count);

            return buff.ToString();
        }

        public bool CheckForStringAttributes()
        {
            foreach (var attribute in Attributes)
            {
                if (attribute is StringAttribute)
                {
                    return true;
                }
            }
            return false;
        }

        internal bool CheckForNumericAttributes()
        {
            foreach (var attribute in Attributes)
            {
                if (attribute is NumericAttribute)
                {
                    return true;
                }
            }
            return false;
        }

        public bool ContainsNominalAttributesOnly()
        {
            return Attributes.All(attribute => attribute is NominalAttribute);
        }

        public bool ContainsStringAttribute()
        {
            return Attributes.Any(a => a is StringAttribute);
        }

        public bool ContainsMissingValues()
        {
            foreach (var instance in Instances)
            {
                if (instance.ContainsMissingValue())
                {
                    return true;
                }
            }

            return false;
        }
    }
}
