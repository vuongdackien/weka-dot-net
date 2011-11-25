using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Weka.NET.Lang
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class NotThreadSafe : Attribute
    {
    }
}
