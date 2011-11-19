﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Weka.NET.Associations;
using Weka.NET.Core;
using System.IO;
using Weka.NET.Core.Parsers;

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
            var apriori = new Apriori(.7);

            var rules = apriori.BuildAssociationRules(Weka.NET.Utils.TestSets.WeatherNominal());

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
