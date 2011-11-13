using System.Collections.Generic;
using System.Linq;
using System;
using System.Text;

namespace Weka.NET.Core
{
    public class DataSet
    {
        public string Name { get; private set; }

        public IList<Attribute> Attributes { get; private set; }

        public IList<Instance> Instances { get; private set; }

        public DataSet(string name, IEnumerable<Attribute> attributes, IEnumerable<Instance> instances)
        {
            Name = name;

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
            buff.Append("DataSet[Name=").Append(Name);
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


    }
}
