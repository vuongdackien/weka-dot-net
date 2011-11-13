using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Weka.NET.Core;

namespace Weka.NET.Tests.Core
{
    public static class TestSets
    {
        public static DataSet WeatherNominal()
        {
            return DataSetBuilder.AnyDataSet()
                .WithRelationName("weather.symbolic")

                .WithNominalAttribute(name:"outlook", values: new[]{"sunny", "overcast", "rainy"})
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
                .AddData(new[] { "sunny", "cool", "normal", "FALSE,yes" })
                .AddData(new[] { "rainy", "mild", "normal", "FALSE,yes" })
                .AddData(new[] { "sunny", "mild", "normal", "TRUE", "yes" })
                .AddData(new[] { "overcast", "mild", "high", "TRUE", "yes" })
                .AddData(new[] { "overcast", "hot", "normal", "FALSE", "yes" })
                .AddData(new[] { "rainy", "mild", "high", "TRUE", "no" })
                
                .Build();
        }
    }
}
