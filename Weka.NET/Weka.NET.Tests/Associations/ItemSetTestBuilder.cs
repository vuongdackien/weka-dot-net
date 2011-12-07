using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Weka.NET.Associations;

namespace Weka.NET.Tests.Associations
{
    internal class ItemSetTestBuilder
    {
        readonly Random random = new Random();

        readonly IDictionary<string, object> propertyValues = new Dictionary<string, object>();

        public static ItemSetTestBuilder NewItemSet()
        {
            return new ItemSetTestBuilder();
        }

        public ItemSetTestBuilder WithItems(params double?[] items)
        {
            propertyValues["Items"] = items;

            return this;
        }

        public ItemSetTestBuilder WithAbsoluteSupport(int support)
        {
            propertyValues["Support"] = support;

            return this;
        }

        public ItemSet Build()
        {
            if (false == propertyValues.ContainsKey("Support"))
            {
                throw new ArgumentException("Support not defined");
            }

            if (false == propertyValues.ContainsKey("Items"))
            {
                throw new ArgumentException("Items not defined");
            }

            return new ItemSet(items: propertyValues["Items"] as double?[], support: (int)propertyValues["Support"]);
        }

        internal ItemSetTestBuilder WithAnySupport()
        {
            WithAbsoluteSupport(random.Next(10));

            return this;
        }

        internal IList<ItemSet> BuildMany(int count)
        {
            var itemSets = new List<ItemSet>();

            for (int i = 0; i < count; i++)
            {
                itemSets.Add(Build());
            }

            return itemSets;
        }

    }
}
