using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Weka.NET.Core
{
    public class CodeContract
    {
        internal static void NotSupportedNumericAttributes(IList<Attribute> attributes)
        {
            if (attributes.Any(attribute => attribute is NumericAttribute))
            {
                throw new ArgumentException("Can't handle numeric attributes");
            }

        }
    }
}
