namespace Weka.NET.Lang
{
    using System;
    
    public static class Stats
    {
        /** The natural logarithm of 2. */
        public static readonly double log2 = Math.Log(2);

        /// <summary>
        /// Returns the logarithm of a for base 2.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        
        public static double Log_2(double a)
        {
            return Math.Log(a) / log2;
        }
    }
}
