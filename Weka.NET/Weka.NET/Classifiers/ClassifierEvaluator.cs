namespace Weka.NET.Classifiers
{
    public interface IClassifierEvaluator
    {
        ClassifierEvaluation EvaluateModel(IClassifier classifier);
    }
    
    public class ClassifierEvaluator : IClassifierEvaluator
    {

        public ClassifierEvaluation EvaluateModel(IClassifier classifier)
        {
            throw new System.NotImplementedException();
        }
    }

    public class ClassifierEvaluation
    {
        public double IncorrectWeight {get; private set;}

        public double CorrectWeight { get; private set; }

        public ClassifierEvaluation(double incorrectWeight, double correctWeight)
        {
            IncorrectWeight = incorrectWeight;
            CorrectWeight = correctWeight;
        }
    }
}
