using System;

namespace Weka.NET.Lang
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class NonThreadSafeAttribute : Attribute
    {
    }
}
