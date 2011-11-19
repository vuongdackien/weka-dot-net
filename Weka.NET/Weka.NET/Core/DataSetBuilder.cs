using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Weka.NET.Core;

namespace Weka.NET.Core
{
    public interface IDataSetBuilder
    {
        IDataSetBuilder WithRelationName(string name);

        IDataSetBuilder WithNumericAttribute(string name);

        IDataSetBuilder WithNominalAttribute(string name, string[] values);

        IDataSetBuilder AddData(string[] values);

        DataSet Build();
    }

    public class DataSetBuilder : IDataSetBuilder
    {
        public static DataSetBuilder AnyDataSet()
        {
            return new DataSetBuilder();
        }

        string relationName;

        IList<Weka.NET.Core.Attribute> attributes = new List<Weka.NET.Core.Attribute>();

        IList<Instance> instances = new List<Instance>();

        public IDataSetBuilder WithRelationName(string name)
        {
            if (name == null || name.Trim().Length == 0)
            {
                throw new ArgumentException("name is null or empty");
            }

            relationName = name.Trim();

            return this;
        }

        public IDataSetBuilder WithStringAttribute(string name)
        {
            attributes.Add(new StringAttribute(name: name, index: attributes.Count));

            return this;
        }

        public IDataSetBuilder WithNumericAttribute(string name)
        {
            attributes.Add(new NumericAttribute(name: name, index:attributes.Count));

            return this;
        }

        public IDataSetBuilder WithNominalAttribute(string name, string[] values)
        {
            if (name == null || name.Trim().Length == 0)
            {
                throw new ArgumentException("name is null or empty");
            }

            if (values == null || values.Length == 0)
            {
                throw new ArgumentException("values is null or empty");
            }

            var trimmedValues = (from v in values select v.Trim()).ToArray();

            attributes.Add( new NominalAttribute(name.Trim(), attributes.Count, trimmedValues) );

            return this;
        }

        public IDataSetBuilder AddData(string[] values)
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
