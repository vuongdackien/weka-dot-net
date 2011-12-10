namespace Weka.NET.Tests.Core.Parser
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NUnit.Framework;
    using Weka.NET.Core.Parsers;
    using System.IO;
    using Weka.NET.Core;
    using Moq;

    [TestFixture]
    public class ArffParserTest
    {
        TesteableArffParser parser;

        Mock<IDataSetBuilder> dataSetBuilder;

        [SetUp]
        public void SetupArffParser()
        {
            dataSetBuilder = new Mock<IDataSetBuilder>(); 
            
            parser = new TesteableArffParser();
        }

        [Test]
        public void CanParseInstance()
        {
            ////Given
            var dataSetString = GivenStringStream(
                "@relation weather.symbolic "+
                "\n @attribute outlook {sunny, overcast, rainy}"
                +"\n @data"
                + "\n sunny,hot,high,FALSE,no");

            var dataSetStream = new LineReader(new StreamReader(dataSetString));

            //When
            parser.ParseRelationName(dataSetBuilder.Object, dataSetStream);
            parser.ParseAttributes(dataSetBuilder.Object, dataSetStream);
            parser.ParseInstances(dataSetBuilder.Object, dataSetStream);

            //Then
            dataSetBuilder.Verify(builder => builder.AddData(new string[] { "sunny","hot","high","FALSE","no" }));
        }

        [Test]
        public void CanParseNominalAttribute()
        {
            ////Given
            var dataSetString = GivenStringStream("@relation weather.symbolic \n @attribute outlook {sunny, overcast, rainy}");
            var dataSetStream = new LineReader(new StreamReader(dataSetString));

            //When
            parser.ParseRelationName(dataSetBuilder.Object, dataSetStream);
            parser.ParseAttributes(dataSetBuilder.Object, dataSetStream);

            //Then
            dataSetBuilder.Verify(builder => builder.WithNominalAttribute("outlook", new string[] { "sunny", "overcast", "rainy" }));
        }

        [Test]
        public void CanParseRelationName()
        {
            ////Given
            var dataSetFileContent = GivenStringStream("@relation somerelation");

            //When
            parser.ParseRelationName(dataSetBuilder.Object, new LineReader(new StreamReader(dataSetFileContent) ));

            //Then
            dataSetBuilder.Verify(builder => builder.WithRelationName("somerelation"));
        }

        private static MemoryStream GivenStringStream(string str)
        {
            byte[] byteArray = Encoding.ASCII.GetBytes(str);
            return new MemoryStream(byteArray);
        }

        private class TesteableArffParser : ArffParser
        {
            public new void ParseRelationName(IDataSetBuilder builder, LineReader reader)
            {
                base.ParseRelationName(builder, reader);
            }

            public new void ParseAttributes(IDataSetBuilder builder, LineReader reader)
            {
                base.ParseAttributes(builder, reader);
            }
        }
    }
}
