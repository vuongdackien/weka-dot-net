using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Weka.NET.Utils
{

    public class OptionArgumentAttribute : Attribute
    {
        public char OptionArgument { get; set; }

        public string OptionDescription { get; set; }

        public OptionArgumentAttribute(char optionArgument, string optionDescription)
        {
            OptionArgument = optionArgument;
            OptionDescription = optionDescription;
        }
    }
}
