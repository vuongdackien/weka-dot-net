using NUnit.Framework;
using Weka.NET.Utils;
using Weka.NET.Associations;

namespace Weka.NET.Tests.Associations
{
    [TestFixture]
    [Category("Scenario Tests")]
    public class AprioriScenariosTest
    {
        /*
        Weka execution:

         Apriori
         =======

         Minimum support: 0.2 (3 instances)
         Minimum metric <confidence>: 0.9
         Number of cycles performed: 16

         Generated sets of large itemsets:

         Size of set of large itemsets L(1): 12

         Size of set of large itemsets L(2): 26

         Size of set of large itemsets L(3): 4

         Best rules found:

          1. outlook=overcast 4 ==> play=yes 4    conf:(1)
          2. temperature=cool 4 ==> humidity=normal 4    conf:(1)
          3. humidity=normal windy=FALSE 4 ==> play=yes 4    conf:(1)
          4. outlook=sunny play=no 3 ==> humidity=high 3    conf:(1)
          5. outlook=sunny humidity=high 3 ==> play=no 3    conf:(1)
          6. outlook=rainy play=yes 3 ==> windy=FALSE 3    conf:(1)
          7. outlook=rainy windy=FALSE 3 ==> play=yes 3    conf:(1)
          8. temperature=cool play=yes 3 ==> humidity=normal 3    conf:(1)
         */
        [Test]
        public void Apriori_ConfidenceIsDot9_SupportIsDot3()
        {
            //Given
            var dataSet = TestSets.WeatherNominal();

            var apriori = new Apriori(new ItemSetBuilder(minSupport: 0.75), new RuleBuilder(minConfidence: .9));

            //When
            var rules = apriori.BuildAssociationRules(dataSet);

            //Then
            Assert.AreEqual(8, rules.Count);
        }

    }
}
