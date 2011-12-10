namespace Weka.NET.Core.Parsers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using System.Text.RegularExpressions;
    
    public interface IArffParser
    {
        DataSet Parse(Stream stream);
    }

    public class LineReader
    {
        readonly StreamReader streamReader;

        public string CurrentLine { get; private set; }

        public int CurrentLineNumber { get; private set; }

        public LineReader(StreamReader streamReader)
        {
            this.streamReader = streamReader;
        }

        public void ReadLine()
        {
            CurrentLine = streamReader.ReadLine();
            if (CurrentLine != null)
            {
                CurrentLine = CurrentLine.Trim();
            }
            CurrentLineNumber++;
        }
    }

    public class ArffParser : IArffParser 
    {
        const string NominalAttributePattern = @"@attribute \w \{(\w+,)*\w+\}";
        
        public DataSet Parse(Stream stream)
        {
            var reader = new LineReader(new StreamReader(stream));

            var dataSetBuilder = new DataSetBuilder();

            ParseRelationName(dataSetBuilder, reader);

            ParseAttributes(dataSetBuilder, reader);

            ParseInstances(dataSetBuilder, reader);

            return dataSetBuilder.Build();        
        }

        public void ParseInstances(IDataSetBuilder dataSetBuilder, LineReader reader)
        {
            if (reader.CurrentLine == null)
            {
                return;
            }

            while (reader.CurrentLine != null && reader.CurrentLine.Trim().Length == 0)
            {
                reader.ReadLine();
            }

            if (false == "@data".Equals(reader.CurrentLine))
            {
                throw new ArgumentException("[Line " + reader.CurrentLineNumber + "] Expecting '@data' but found: " + reader.CurrentLine);
            }

            reader.ReadLine();

            while (reader.CurrentLine != null && reader.CurrentLine.Trim().Length == 0)
            {
                reader.ReadLine();
            }

            while (reader.CurrentLine != null && reader.CurrentLine.Trim().Length > 0)
            {
                var instanceValues = reader.CurrentLine.Split(',');

                var trimmedInstanceValues = (from v in instanceValues select v.Trim()).ToArray();

                dataSetBuilder.AddData(instanceValues);

                reader.ReadLine();
            }

        }

        public void ParseAttributes(IDataSetBuilder dataSetBuilder, LineReader reader)
        {
            while (reader.CurrentLine != null && reader.CurrentLine.Trim().Length == 0)
            {
                reader.ReadLine();
            }

            if (false == reader.CurrentLine.StartsWith("@attribute"))
            {
                throw new ArgumentException("[Line " + reader.CurrentLineNumber + "] Expecting line to start with '@attribute ...' found: " + reader.CurrentLine);
            }

            do
            {
                int bracketOpen = reader.CurrentLine.IndexOf('{');

                int bracketClose = reader.CurrentLine.IndexOf('}');

                var name = reader.CurrentLine.Substring(10, bracketOpen - 10);

                var valuesString = reader.CurrentLine.Substring(bracketOpen+1, bracketClose - bracketOpen - 1);

                var values = valuesString.Split(',');

                var trimmedValues = (from v in values select v.Trim()).ToArray();

                dataSetBuilder.WithNominalAttribute(name.Trim(), trimmedValues);

                reader.ReadLine();
            }
            while (reader.CurrentLine != null && reader.CurrentLine.StartsWith("@attribute"));
        }

        public void ParseRelationName(IDataSetBuilder dataSetBuilder, LineReader reader)
        {
            reader.ReadLine();

            while (reader.CurrentLine != null && reader.CurrentLine.Trim().Length == 0)
            {
                reader.ReadLine();
            }

            if (reader.CurrentLine == null)
            {
                throw new ArgumentException("Invalid Arff file - no @relation tag found");
            }

            if (false == reader.CurrentLine.StartsWith("@relation"))
            {
                Console.WriteLine("starts with: " + reader.CurrentLine);

                throw new ArgumentException("[Line " + reader.CurrentLineNumber + "] Expecting string starting with '@relation ...' but found: " + reader.CurrentLine);
            }

            var relationName = reader.CurrentLine.Substring(10).Trim();

            dataSetBuilder.WithRelationName(relationName);

            reader.ReadLine();
        }


    }
}
