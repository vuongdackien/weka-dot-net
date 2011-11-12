using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Weka.NET.Core
{
    public abstract class NumericAttribute : Weka.NET.Core.Attribute
    {
        public NumericAttribute(string name, int index)
            : base(name, index)
        {
        }
    }
}
