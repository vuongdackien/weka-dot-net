namespace Weka.NET.Estimators
{
    public interface IEstimator
    {
        void AddValue(double data, double weight);

        double GetProbability(double data);
    }
}
