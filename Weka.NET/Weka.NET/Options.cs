namespace Weka.NET
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    struct OptionArgument
    {
        public char ShortIdentifier { set; get; }

        public string Argument { set; get; }
    }
    
    sealed class Options
    {
        readonly HashSet<char> options = new HashSet<char>();

        readonly IDictionary<char, OptionArgument> optionArgs = new Dictionary<char, OptionArgument>();

        public Options AddOption(char option, bool required)
        {
            options.Add(option);

            return this;   
        }

        public IDictionary<char, OptionArgument> ParseArguments(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Equals("-t"))
                {
                    i++;

                    optionArgs['t'] = new OptionArgument{ShortIdentifier='t', Argument=args[i]};

                    continue;
                }

                if (args[i].Equals("-M"))
                {
                    i++;

                    optionArgs['M'] = new OptionArgument { ShortIdentifier = 'M', Argument = args[i] };

                    continue;
                }

                if (args[i].Equals("-C"))
                {
                    i++;

                    optionArgs['C'] = new OptionArgument { ShortIdentifier = 'C', Argument = args[i] };

                    continue;
                }

            }

            return optionArgs;
        }


    }
}
