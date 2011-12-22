namespace Weka.NET.Classifiers
{
    using Weka.NET.Core;
    
    public interface IClassifier
    {
        Core.Attribute ClassAttribute { get; }

        int ClassAttributeIndex { get; }

        double ClassifyInstance(Instance instance);
    }
}
