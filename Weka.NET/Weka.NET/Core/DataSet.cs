using System.Collections.Generic;
using System.Linq;

namespace Weka.NET.Core
{
    public class DataSet
    {
        public IList<Attribute> Attributes { get; private set; }

        public IList<Instance> Instances { get; private set; }

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

        public DataSet(IEnumerable<Instance> instances)
        {
            this.Instances = new List<Instance>(instances).AsReadOnly();
        }
    }
}
