using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Weka.NET.Core
{
    public abstract class StringAttribute : Weka.NET.Core.Attribute
    {
        public StringAttribute(string name, int index) : base(name, index)
        {
        }
    }
}
