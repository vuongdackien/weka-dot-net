using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Weka.NET.Associations;
using Weka.NET.Core;
using System.IO;
using Weka.NET.Core.Parsers;
using Weka.NET.Utils;

namespace Weka.NET
{

    public static class CommandLine
    {
        static Options options;

        static CommandLine()
        {
            options = new Options();
            options.AddOption('t', true);
        }


        static void Main(string[] args)
        {
            //Given
            var dataSet = TestSets.WeatherNominal();

            //And
            var builder = new ItemSetBuilder(0.1);

            var singletons = builder.BuildSingletons(dataSet.Attributes);

            var stuff = new ItemSetBuilder.ItemSetBuilderStuff(dataSet, 6);

            stuff.AddAll(singletons);

            //When
            builder.FindSets(1, stuff);

            Console.WriteLine(stuff.ItemSets);
        }


        static void Main2(string[] args)
        {
 



          //  var apriori = new Apriori { MinSupport = 1, Delta = .05 };

          //  var rules = apriori.BuildAssociationRules(Weka.NET.Utils.TestSets.WeatherNominal(), 10);

            var optionArgs = options.ParseArguments(args);

            if (false == optionArgs.ContainsKey('t'))
            {
                throw new ArgumentException("Please set data set file name");
            }

            var dataSet = ParseDataSet(optionArgs['t'].Argument);

            var d = from i in dataSet.Instances select i.ToString();



           /* var apriori = new Apriori(.3);

            var rules = apriori.BuildAssociationRules(dataSet);

            Console.WriteLine("Generated Rules: ");
            foreach (var rule in rules)
            {
                Console.WriteLine(rule.ToString());
            }*/
        }

        public static DataSet ParseDataSet(string fileName)
        {
            using (var stream = File.OpenRead(fileName))
            {
                return new ArffParser().Parse(stream);
            }
        }
    }
}
