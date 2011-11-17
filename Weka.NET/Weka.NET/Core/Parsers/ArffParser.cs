﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace Weka.NET.Core.Parsers
{
    public interface IArffParser
    {
        DataSet Parse(Stream stream);
    }

    public class ArffParser : IArffParser 
    {
        const string NominalAttributePattern = @"@attribute \w \{(\w+,)*\w+\}";
        
        string currentLine;

        int currentLineNumber = 0;

        public DataSet Parse(Stream stream)
        {
            var dataSetBuilder = new DataSetBuilder();

            var reader = new StreamReader(stream);

            ParseRelationName(dataSetBuilder, reader);

            ParseAttributes(dataSetBuilder, reader);

            ParseInstances(dataSetBuilder, reader);

            return dataSetBuilder.Build();        
        }

        protected void ParseInstances(IDataSetBuilder dataSetBuilder, StreamReader reader)
        {
            if (currentLine.Trim().Length == 0)
            {
                while ((currentLine = reader.ReadLine()).Trim().Length == 0) ;
            }

            if (false == "@data".Equals(currentLine))
            {
                throw new ArgumentException("[Line " + currentLine + "] Expecting '@data' but found: " + currentLine);
            }

            if (currentLine.Trim().Length == 0)
            {
                while ((currentLine = reader.ReadLine()).Trim().Length == 0) ;
            }

            while (currentLine != null && currentLine.Trim().Length > 0)
            {
                var instanceValues = currentLine.Split(',');

                dataSetBuilder.AddData(instanceValues);

                currentLine = reader.ReadLine();
            }

        }

        protected void ParseAttributes(IDataSetBuilder dataSetBuilder, StreamReader reader)
        {
            while (currentLine != null && currentLine.Trim().Length == 0)
            {
                ReadLine(reader);
            }

            Console.WriteLine("currentLine: " + currentLine);

            if (false == currentLine.StartsWith("@attribute"))
            {
                throw new ArgumentException("[Line " + currentLineNumber + "] Expecting line to start with '@attribute ...' found: " + currentLine);
            }

            do
            {
                int bracketOpen = currentLine.IndexOf('{');

                int bracketClose = currentLine.IndexOf('}');

                var name = currentLine.Substring(10, bracketOpen);

                var values = currentLine.Substring(bracketOpen, bracketClose);

                dataSetBuilder.WithNominalAttribute(name, values.Split(','));

                ReadLine(reader);
            }
            while (currentLine.StartsWith("@attribute"));
        }

        private void ReadLine(StreamReader reader)
        {
            currentLine = reader.ReadLine();
            if (currentLine != null)
            {
                currentLine = currentLine.Trim();
            }
            currentLineNumber++;
        }

        public void ParseRelationName(IDataSetBuilder dataSetBuilder, StreamReader reader)
        {
            ReadLine(reader);

            while (currentLine != null && currentLine.Trim().Length == 0)
            {
                ReadLine(reader);
            }

            if (currentLine == null)
            {
                throw new ArgumentException("Invalid Arff file - no @relation tag found");
            }

            if (false == currentLine.StartsWith("@relation"))
            {
                Console.WriteLine("starts with: " + currentLine);

                throw new ArgumentException("[Line " + currentLineNumber + "] Expecting string starting with '@relation ...' but found: " + currentLine);
            }

            var relationName = currentLine.Substring(10).Trim();

            dataSetBuilder.WithRelationName(relationName);
        }


    }
}
