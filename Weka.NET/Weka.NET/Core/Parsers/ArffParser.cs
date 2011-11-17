using System;
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

        int currentLineNumber = -1;

        public DataSet Parse(Stream stream)
        {
            var dataSetBuilder = new DataSetBuilder();

            var reader = new StreamReader(stream);

            ParseRelationName(dataSetBuilder, reader);

            ParseAttributes(dataSetBuilder, reader);

            ParseInstances(dataSetBuilder, reader);

            return dataSetBuilder.Build();        
        }

        protected void ParseInstances(DataSetBuilder dataSetBuilder, StreamReader reader)
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

        protected void ParseAttributes(DataSetBuilder dataSetBuilder, StreamReader reader)
        {
            if (currentLine.Trim().Length == 0)
            {
                while ((currentLine = reader.ReadLine()).Trim().Length == 0) ;
            }

            do
            {
                if (Regex.IsMatch(currentLine, NominalAttributePattern))
                {
                    int bracketOpen = currentLine.IndexOf('{');

                    int bracketClose = currentLine.IndexOf('}');

                    var attributeNameTokens = currentLine.Substring(0, bracketOpen).Split();

                    var attributeName = attributeNameTokens[1];

                    var pattern = @"\{(\w+,)*\w+\}";

                    var matches = Regex.Matches(currentLine.Substring(bracketOpen, bracketClose), pattern, RegexOptions.ExplicitCapture);

                    var nominalValues = new List<string>();

                    foreach(Match match in matches)
                    {
                        nominalValues.Add(match.Value);
                    }
                }

                currentLine = reader.ReadLine();

            } while (currentLine != null && currentLine.Trim().Length > 0 && currentLine.StartsWith("@attribute"));
        }

        protected void ParseRelationName(DataSetBuilder dataSetBuilder, StreamReader reader)
        {
            while ((currentLine = reader.ReadLine()).Trim().Length == 0);

            var tokens = currentLine.Split();

            if (tokens.Length != 2)
            {
                throw new ArgumentException("[Line " + currentLine + "] Expecting string with '@relation RELATION_NAME' format but found: " + currentLine);
            }

            if ("@relation".Equals(tokens[0]))
            {
                throw new ArgumentException("[Line " + currentLine + "] Expecting string starting with '@relation ...' but found: " + currentLine);
            }

            dataSetBuilder.WithRelationName(tokens[1]);
        }


    }
}
