using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Weka.NET
{
    struct OptionArgument
    {
        public char ShortIdentifier;

        public string Argument { set; get; }
    }
    
    sealed class Options
    {
        readonly IDictionary<char, OptionArgument> options = new Dictionary<char, OptionArgument>();

        public Options AddOption(char option, bool required)
        {
            return this;   
        }

        public IDictionary<char, OptionArgument> ParseArguments(string[] args)
        {
            return null;
        }


    }
}
