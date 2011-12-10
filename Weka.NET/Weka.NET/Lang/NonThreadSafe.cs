namespace Weka.NET.Lang
{
    using System;
    
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class NonThreadSafeAttribute : Attribute
    {
    }
}
