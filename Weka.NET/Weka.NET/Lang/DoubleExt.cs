namespace Weka.NET.Lang
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    
    public static class DoubleExt
    {
        readonly public static double SMALL = 1e-6;

        public static bool GreaterOrEqualsTo(this double a, double b)
        {
            return (b - a < SMALL);
        }


    }
}
