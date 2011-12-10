namespace Weka.NET.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Weka.NET.Core;
    
    public static class TestSets
    {
        public static DataSet BooleanDataSet()
        {
            return DataSetBuilder.AnyDataSet()
                .WithRelationName("super.simple")

                .WithNominalAttribute(name: "pen", values: new[] { "T", "F" })
                .WithNominalAttribute(name: "ink", values: new[] { "T", "F" })
                .WithNominalAttribute(name: "diary", values: new[] { "T", "F" })
                .WithNominalAttribute(name: "soap", values: new[] { "T", "F" })

                .AddData(new[] { "T", "T", "T", "T" })
                .AddData(new[] { "T", "T", "T", "F" })
                .AddData(new[] { "T", "F", "T", "F" })
                .AddData(new[] { "T", "T", "F", "T" })

                .Build();
        }

        public static DataSet WeatherNominal()
        {
            return DataSetBuilder.AnyDataSet()
                .WithRelationName("weather.symbolic")

                .WithNominalAttribute(name: "outlook", values: new[] { "sunny", "overcast", "rainy" })
                .WithNominalAttribute(name: "temperature", values: new[] { "hot", "mild", "cool" })
                .WithNominalAttribute(name: "humidity", values: new[] { "high", "normal" })
                .WithNominalAttribute(name: "windy", values: new[] { "TRUE", "FALSE" })
                .WithNominalAttribute(name: "play", values: new[] { "yes", "no" })

                .AddData(new[] { "sunny", "hot", "high", "FALSE", "no" })
                .AddData(new[] { "sunny", "hot", "high", "TRUE", "no" })
                .AddData(new[] { "overcast", "hot", "high", "FALSE", "yes" })
                .AddData(new[] { "rainy", "mild", "high", "FALSE", "yes" })
                .AddData(new[] { "rainy", "cool", "normal", "FALSE", "yes" })
                .AddData(new[] { "rainy", "cool", "normal", "TRUE", "no" })
                .AddData(new[] { "overcast", "cool", "normal", "TRUE", "yes" })
                .AddData(new[] { "sunny", "mild", "high", "FALSE", "no" })
                .AddData(new[] { "sunny", "cool", "normal", "FALSE", "yes" })
                .AddData(new[] { "rainy", "mild", "normal", "FALSE", "yes" })
                .AddData(new[] { "sunny", "mild", "normal", "TRUE", "yes" })
                .AddData(new[] { "overcast", "mild", "high", "TRUE", "yes" })
                .AddData(new[] { "overcast", "hot", "normal", "FALSE", "yes" })
                .AddData(new[] { "rainy", "mild", "high", "TRUE", "no" })

                .Build();
        }
    }
}
