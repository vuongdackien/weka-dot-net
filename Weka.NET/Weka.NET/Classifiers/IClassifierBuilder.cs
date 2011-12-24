using Weka.NET.Core;
namespace Weka.NET.Classifiers
{
    public interface IClassifierBuilder<T> where T : IClassifier
    {
        T BuildClassifier(IDataSet trainingData, int classAttributeIndex);
    }
}
