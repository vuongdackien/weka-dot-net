namespace Weka.NET.Clusterers
{
    using Weka.NET.Core;

    public interface IClusterer
    {
        void BuildClusterer(IDataSet dataSet);

        Cluster ClusterInstance(Instance instance);
    }
}
