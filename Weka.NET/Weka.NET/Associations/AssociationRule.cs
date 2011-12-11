using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Weka.NET.Lang;
using Weka.NET.Core;

namespace Weka.NET.Associations
{
    [Immutable]
    [Serializable]
    public class AssociationRule : IEquatable<AssociationRule>
    {
        public ItemSet Premisse { get; private set; }

        public ItemSet Consequence { get; private set; }

        public double Confidence { get; private set; }

        public AssociationRule(ItemSet premisse, ItemSet consequence, double confidence)
        {
            Premisse = premisse;
            Consequence = consequence;
            Confidence = confidence;
        }

        public bool Equals(AssociationRule other)
        {
            return Premisse.Equals(other.Premisse)
                && Consequence.Equals(other.Consequence)
                && Confidence.Equals(other.Confidence);
        }

        public override bool Equals(object other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (other.GetType() != typeof(AssociationRule))
            {
                return false;
            }

            return Equals(other as AssociationRule);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return 13 ^ Premisse.GetHashCode() ^ Consequence.GetHashCode();
            }
        }
    }

    public class AssociationRuleDisplay : IDisplayable
    {
        readonly AssociationRule rule;

        readonly IList<Core.Attribute> attributes;

        public AssociationRuleDisplay(IList<Core.Attribute> attributes, AssociationRule rule)
        {
            this.attributes = attributes;
            this.rule = rule;
        }

        public string ToDisplayString()
        {
            var buff = new StringBuilder("IF ");

            DisplayItemSet(buff, rule.Premisse);

            buff.Append(" THEN ");

            DisplayItemSet(buff, rule.Consequence);

            buff.Append(" [Confidence=").Append(rule.Confidence).Append("]");

            return buff.ToString();
        }

        private void DisplayItemSet(StringBuilder buff, ItemSet items)
        {
            bool first = true;

            for (int i = 0; i < items.Length; i++)
            {
                if (false == items[i].HasValue)
                {
                    continue;
                }

                if (first == false)
                {
                    buff.Append(" AND ");
                }

                buff.Append(attributes[i].Name).Append("=").Append(attributes[i].Decode(items[i]));

                first = false;
            }

        }
    }
}
