using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Moq;
using Weka.NET.Associations;

namespace Weka.NET.Tests.Associations
{
    [TestFixture]
    public class RuleBuilderTest
    {

        [Test]
        public void CanBuildBasicRules()
        {
            //Given
            var ruleBuilder = new RuleBuilder(minConfidence: .4);

            var itemSets = new List<ItemSet>
                { 
                   ItemSetTestBuilder.NewItemSet().WithItems(1,null).WithAbsoluteSupport(2).Build()
                   ,
                   ItemSetTestBuilder.NewItemSet().WithItems(null,1).WithAbsoluteSupport(3).Build()
                };

            var fullRule = ItemSetTestBuilder.NewItemSet().WithItems(1,1).WithAbsoluteSupport(2).Build();

            //When
            var actual = ruleBuilder.BuildRules(fullRule, itemSets);

            //Then
            var expected = new List<AssociationRule>
            {
                new AssociationRule(
                    premisse: ItemSetTestBuilder.NewItemSet().WithItems(1,null).WithAbsoluteSupport(2).Build()
                    ,
                    consequence: ItemSetTestBuilder.NewItemSet().WithItems(1,null).WithAbsoluteSupport(3).Build()
                    ,
                    fullRule: ItemSetTestBuilder.NewItemSet().WithItems(1,1).WithAbsoluteSupport(2).Build()
                    )
            };

            Assert.AreEqual(expected, actual);
        }
    }
}
