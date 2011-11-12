using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Weka.NET.Core;

namespace Weka.NET.Tests.Core
{
    public class DataSetTestBuilder
    {
        public static DataSetTestBuilder AnyDataSet()
        {
            return new DataSetTestBuilder();
        }

        IList<Instance> instances = new List<Instance>();

        public DataSet Build()
        {
            return new DataSet(instances);
        }
    }
}
