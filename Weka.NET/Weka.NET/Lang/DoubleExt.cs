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

        /**
 * Round a data value using the defined precision for this estimator
 *
 * @param data the value to round
 * @return the rounded data value
 */
        public static double round(double data, int precision)
        {

            return Math.Round(data, precision);
        }

    }
}
