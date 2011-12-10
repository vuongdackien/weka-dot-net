namespace Weka.NET.Classifiers
{
    using Weka.NET.Core;
    
    public interface IClassifier
    {
        double ClassifyInstance(Instance instance);
    }
}
