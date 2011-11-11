using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Weka.NET.Associations;
using Weka.NET.Tests.Core;
using NUnit.Framework;

namespace Weka.NET.Tests.Associations
{
    [TestFixture]
    public class AprioriTest
    {
        Apriori apriori;

        [SetUp]
        public void SetupApriori()
        {
            apriori = new Apriori();
        }

        [Test]
        public void CanBuildAssociationRules()
        {
            var dataSet = DataSetTestBuilder.AnyDataSet().Build();

            apriori.BuildAssociationRules(dataSet);
        }
    }
}
