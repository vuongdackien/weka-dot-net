using System;
using System.Collections.Generic;
using System.Linq;

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

        internal static void NotSupportedStringAttributes(IList<Attribute> attributes)
        {
            if (attributes.Any(attribute => attribute is StringAttribute))
            {
                throw new ArgumentException("Can't handle string attributes");
            }
        }
    }
}
