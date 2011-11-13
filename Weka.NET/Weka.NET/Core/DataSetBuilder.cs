using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Weka.NET.Core;

namespace Weka.NET.Core
{
    public class DataSetBuilder
    {
        public static DataSetBuilder AnyDataSet()
        {
            return new DataSetBuilder();
        }

        string relationName;

        IList<Weka.NET.Core.Attribute> attributes = new List<Weka.NET.Core.Attribute>();

        IList<Instance> instances = new List<Instance>();

        public DataSetBuilder WithRelationName(string name)
        {
            relationName = name;

            return this;
        }

        public DataSetBuilder WithNumericAttribute(string name)
        {
            attributes.Add(new NumericAttribute(name: name, index:attributes.Count));

            return this;
        }

        public DataSetBuilder WithNominalAttribute(string name, string[] values)
        {
            attributes.Add( new NominalAttribute(name, attributes.Count, values) );

            return this;
        }

        public DataSetBuilder AddData(string[] values)
        {
            var encoded = new double?[values.Length];

            for(int i=0;i<values.Length;i++)
            {
                encoded[i] = attributes[i].Encode(values[i]);               
            }

            instances.Add(new Instance(encoded));

            return this;
        }

        public DataSet Build()
        {
            return new DataSet(name:relationName, attributes: attributes, instances: instances);
        }


    }
}
