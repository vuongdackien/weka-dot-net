namespace Weka.NET.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Weka.NET.Core;
    using System.IO;
    
    public interface IDataSetBuilder
    {
        IDataSetBuilder WithRelationName(string name);

        IDataSetBuilder WithNumericAttribute(string name);

        IDataSetBuilder WithNominalAttribute(string name, string[] values);

        IDataSetBuilder AddInstance(params string[] values);

        IDataSetBuilder WithStringAttribute(string name);

        IDataSetBuilder AddWeightedInstance(double weight, params string[] values);

        StreamReader BuildAsStreamReader();

        IList<IDataSet> SplitDataSet(IDataSet dataSet, int attributeIndex);

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

        public IDataSetBuilder AddWeightedInstance(double weight, params string[] values)
        {
            var instance = CreateInstance(weight, values);

            instances.Add(instance);

            return this;
        }
        
        public IDataSetBuilder WithStringAttribute(string name)
        {
            attributes.Add(new StringAttribute(name: name));

            return this;
        }

        public IDataSetBuilder WithNumericAttribute(string name)
        {
            attributes.Add(new NumericAttribute(name: name));

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

            attributes.Add( new NominalAttribute(name.Trim(), trimmedValues) );

            return this;
        }

        public IDataSetBuilder AddInstance(params string[] values)
        {
            var instance = CreateInstance(Instance.DefaultWeight, values);

            instances.Add(instance);

            return this;

        }

        Instance CreateInstance(double weight, params string[] values)
        {
            var encoded = new double[values.Length];

            for(int i=0;i<values.Length;i++)
            {
                encoded[i] = attributes[i].Encode(values[i]);               
            }

            var instance = new Instance(weight, encoded);

            return instance;
        }

        public DataSet Build()
        {
            return new DataSet(name:relationName, attributes: attributes, instances: instances);
        }

        public StreamReader BuildAsStreamReader()
        {
            throw new NotImplementedException();
        }

        public IDataSet Duplicate(IDataSet trainingData)
        {
            var builder = AnyDataSet();

            builder.WithRelationName(trainingData.RelationName);

            foreach (var attribute in trainingData.Attributes)
            {
                builder.WithAttribute(attribute);
            }

            foreach (var instance in trainingData.Instances)
            {
                builder.AddInstance(instance);
            }

            return builder.Build();
        }

        public void AddInstance(Instance instance)
        {
            instances.Add(instance);
        }

        public void WithAttribute(Attribute attribute)
        {
            attributes.Add(attribute);
        }

        public IList<IDataSet> SplitDataSet(IDataSet dataSet, int attributeIndex)
        {
            if (false == dataSet.Attributes[attributeIndex] is NominalAttribute)
            {
                throw new ArgumentException("Can only split dataset by nominal attribute");
            }

            var attributeToSplit = dataSet.Attributes[attributeIndex] as NominalAttribute;

            var builders = new DataSetBuilder[attributeToSplit.Values.Length];

            for(int i=0;i<builders.Length;i++)
            {
                builders[i] = new DataSetBuilder();

                builders[i].WithRelationName(dataSet.RelationName);

                builders[i].WitAttributes(dataSet.Attributes);
            }

            foreach (var instance in dataSet.Instances)
            {
                builders[ (int) instance[attributeIndex] ].AddInstance(instance);
            }

            var dataSets = new List<IDataSet>();

            foreach (var builder in builders)
            {
                var splitted = builder.Build();

                dataSets.Add(splitted);
            }

            return dataSets;
        }

        public void WitAttributes(IList<Attribute> attributes)
        {
            foreach (var attribute in attributes)
            {
                WithAttribute(attribute);
            }
        }

        public static bool EqualsAttributes(IDataSet first, IDataSet second)
        {
            if (first.Attributes.Count != second.Attributes.Count)
            {
                return false;
            }

            for (int i = 0; i < first.Attributes.Count; i++)
            {
                if (false == first.Attributes[i].Equals(second.Attributes[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool EqualsInstances(IDataSet first, IDataSet second)
        {
            if (first.Instances.Count != second.Instances.Count)
            {
                return false;
            }

            for (int i = 0; i < first.Instances.Count; i++)
            {
                if (false == first.Instances[i].Equals(second.Instances[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
