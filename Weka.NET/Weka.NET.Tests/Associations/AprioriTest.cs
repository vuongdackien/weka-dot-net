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
        }

        [Test]
        public void AprioriFirstRequestItemSetBuilderToBuildItemSetsWithHighSupportAndThenBuildTheRules()
        {
        }

    }
}
