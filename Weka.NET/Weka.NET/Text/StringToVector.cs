using Weka.NET.Text.Stemmers;
namespace Weka.NET.Text
{
    public interface IStringToVector
    {
        Stemmer Stemmer { get; }

        bool Downcased { get; }

        StopWordsList StopWords { get; }
    }

    public class StringToVector
    {
        public StopWordsList StopWords { get; private set; }

        public Stemmer Stemmer {get; private set;}

        public bool Downcased { get; private set; }

        public IWordDictionary Dictionary { get; private set; }
    }
}
