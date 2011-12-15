namespace Weka.NET.Tests.Associations
{
    using NUnit.Framework;
    using Weka.NET.Associations;
    using Moq;
    using Weka.NET.Core;
    using System.Collections.Generic;
    using System;

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
            var supportByItems = new Dictionary<ItemSet, int>();

            itemSetBuilder.Setup(b => b.BuildItemSets(It.IsAny<IDataSet>())).Returns(supportByItems);

            //When
            apriori.BuildAssociationRules(dataSet.Object);

            //Then
            ruleBuilder.Verify(r => r.BuildRules(dataSet.Object, supportByItems), Times.Once());
        }

    }
}
