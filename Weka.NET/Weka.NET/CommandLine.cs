using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Weka.NET.Core;
using System.IO;
using Weka.NET.Core.Parsers;
using Weka.NET.Utils;
using Weka.NET.Associations;

namespace Weka.NET
{
    public static class CommandLine
    {
        static Options options;

        static CommandLine()
        {
            options = new Options();
            options.AddOption('t', true);
            options.AddOption('M', false);
            options.AddOption('C', false);
        }

        static void Main(string[] args)
        {
            var optionArgs = options.ParseArguments(args);

            double minSupport = optionArgs.ContainsKey('M') ? double.Parse(optionArgs['M'].Argument) : .2;

            double minConfidence = optionArgs.ContainsKey('C') ? double.Parse(optionArgs['C'].Argument) : .9; 

            if (false == optionArgs.ContainsKey('t'))
            {
                throw new ArgumentException("Please set data set file name");
            }

            var dataSet = ParseDataSet(optionArgs['t'].Argument);

            var apriori = new Apriori(new ItemSetBuilder(minSupport), new RuleBuilder(minConfidence));

            var rules = apriori.BuildAssociationRules(dataSet);

            DisplayRules(dataSet, rules);
        }

        public static DataSet ParseDataSet(string fileName)
        {
            using (var stream = File.OpenRead(fileName))
            {
                return new ArffParser().Parse(stream);
            }
        }

        private static void DisplayRules(DataSet dataSet, IList<AssociationRule> rules)
        {
            Console.WriteLine("Rules found:");

            int ruleCount = 0;

            foreach (var r in rules)
            {
                ruleCount++;

                var str = new AssociationRuleDisplay(dataSet.Attributes, r).ToDisplayString();

                Console.WriteLine(ruleCount + ": " + str);
            }
        }
    }
}
