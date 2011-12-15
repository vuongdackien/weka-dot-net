namespace Weka.NET.Associations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Weka.NET.Core;
    using Weka.NET.Utils;
    using Weka.NET.Lang;

    public interface IApriori
    {
        IList<AssociationRule> BuildAssociationRules(IDataSet dataSet);
    }

    [OptionArgument('M', "Set minimum support")]
    public class Apriori : IApriori
    {
        readonly IItemSetBuilder itemSetBuilder;

        readonly IRuleBuilder ruleBuilder;

        public Apriori(IItemSetBuilder iItemSetBuilder, IRuleBuilder ruleBuilder)
        {
            this.itemSetBuilder = iItemSetBuilder;
            this.ruleBuilder = ruleBuilder;
        }

        public IList<AssociationRule> BuildAssociationRules(IDataSet dataSet)
        {
            var itemSets = itemSetBuilder.BuildItemSets(dataSet);

            var rules = ruleBuilder.BuildRules(dataSet, itemSets);

            return rules;
        }
    }
}