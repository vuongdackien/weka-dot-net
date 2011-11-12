using Weka.NET.Core;
namespace Weka.NET.Classifiers
{
    public interface IClassifier
    {
        double ClassifyInstance(Instance instance);
    }
}
