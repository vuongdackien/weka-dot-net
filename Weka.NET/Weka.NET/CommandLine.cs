using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Weka.NET.Associations;
using Weka.NET.Core;

namespace Weka.NET
{
    public class CommandLine
    {
        static void Main(string[] args)
        {
            //Given
            var dataSet = DataSetBuilder.AnyDataSet()
                .WithNominalAttribute
                    (name: "first_attribute", values: new[] { "first0", "first1" })
                .WithNominalAttribute
                    (name: "second_attribute", values: new[] { "second0", "second1" })
                .WithNominalAttribute
                    (name: "third_attribute", values: new[] { "third0", "third1" })

                .AddData(values: new[] { "first0", "second0", "third0" })
                .AddData(values: new[] { "first0", "second1", "third0" })
                .AddData(values: new[] { "first0", "second1", "third1" })
                .AddData(values: new[] { "first0", "second1", "third1" })
                .AddData(values: new[] { "first1", "second1", "third1" })
                .AddData(values: new[] { "first1", "second1", "third1" })

                .Build();

            //When
            var apriori = new Apriori(.3);

            var actualRules = apriori.GenerateAllRulesWithOneItemInTheConsequence(new ItemSet(new int?[] { 0, 1, 0 }), dataSet);

            actualRules.ToArray();
        }
    }
}
