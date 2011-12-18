namespace Weka.NET.Filters.Unsupervised.Attribute
{
    using Weka.NET.Text;
    using Weka.NET.Text.Stemmers;
    using System.IO;
    using Weka.NET.Core;

    public interface IStringToVector
    {
        Stemmer Stemmer { get; }

        bool Downcased { get; }

        StopWordsList StopWords { get; }

        IWordDictionary Dictionary { get; }

        int MinTermFrequency { get; }
    }

    public class StringToVector : IStringToVector
    {
        public StopWordsList StopWords { get; private set; }

        public Stemmer Stemmer {get; private set;}

        public bool Downcased { get; private set; }

        public IWordDictionary Dictionary { get; private set; }

        public int MinTermFrequency { get; private set; }

        public IDataSet Filter(Core.DataSet dataSet)
        {
            throw new System.NotImplementedException();
        }
    }
}
