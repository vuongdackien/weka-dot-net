namespace Weka.NET.Tests.Associations
{
    using NUnit.Framework;
    using Weka.NET.Associations;
    using Moq;
    using Weka.NET.Core;
    using System.Collections.Generic;
    
    [TestFixture]
    public class AprioriTest
    {
        Apriori apriori;

        Mock<IItemSetBuilder> itemSetBuilder;
        
        Mock<IRuleBuilder> ruleBuilder;

        Mock<IDataSet> dataSet;

        [SetUp]
        public void SetupApriori()
        {
            itemSetBuilder = new Mock<IItemSetBuilder>();

            ruleBuilder = new Mock<IRuleBuilder>();

            dataSet = new Mock<IDataSet>();

            apriori = new Apriori(itemSetBuilder.Object, ruleBuilder.Object);
        }

        [Test]
        public void AprioriFirstRequestItemSetBuilderToBuildItemSetsWithHighSupportAndThenBuildTheRules()
        {
            //Given
            var indexedItemSets = new Dictionary<int, IList<ItemSet>>();

            indexedItemSets[1] = new List<ItemSet>
            {
                ItemSetTestBuilder.NewItemSet().WithItems(1d, null).WithAnySupport().Build()
                ,
                ItemSetTestBuilder.NewItemSet().WithItems(null, 1d).WithAnySupport().Build()
            };

            indexedItemSets[2] = new List<ItemSet>
            {
                ItemSetTestBuilder.NewItemSet().WithItems(1d, 1d).WithAnySupport().Build()
            };

            itemSetBuilder.Setup(b => b.BuildItemSets(It.IsAny<IDataSet>())).Returns(indexedItemSets);

            //When
            apriori.BuildAssociationRules(dataSet.Object);

            //Then
            ruleBuilder.Verify(r => r.BuildRules(indexedItemSets), Times.Once());
        }

    }
}
