using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Weka.NET.Core.Parsers
{
    public interface IArffParser
    {
        DataSet Parse(Stream stream);
    }
}
