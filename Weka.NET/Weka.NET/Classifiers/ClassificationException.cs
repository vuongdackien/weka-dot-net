using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Weka.NET.Classifiers
{
    public class ClassificationException : Exception
    {
        public ClassificationException(string message)
            : base(message)
        {
        }
    }
}
