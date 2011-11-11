using System.Collections.Generic;

namespace Weka.NET.Core
{
    public class DataSet
    {
        public IList<Attribute> Attributes { get; private set; }

        public IList<Instance> Instances { get; private set; }


    }
}
