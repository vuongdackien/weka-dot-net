using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Weka.NET.Lang
{
    public static class DoubleExt
    {
        readonly public static double SMALL = 1e-6;

        public static bool GreaterOrEqualsTo(this double a, double b)
        {
            return (b - a < SMALL);
        }
    }
}
