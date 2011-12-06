using System;
using System.Text;

namespace Weka.NET.Associations
{
    public class AssociationRule : IEquatable<AssociationRule>
    {
        private ItemSet rule;

        public ItemSet Premisse { get; private set; }

        public ItemSet Consequence {get; private set;}

        public double Confidence { get { return (double)rule.Support / (double)Premisse.Support; } }

        /// <summary>
        /// Support of LHS Union RHS
        /// </summary>
        public double Support { get { return rule.Support; } }

        public AssociationRule(ItemSet premisse, ItemSet consequence, ItemSet rule)
        {
            this.Premisse = premisse;
            this.Consequence = consequence;
            this.rule = rule;
        }

        public override int GetHashCode()
        {
            return 37 * Confidence.GetHashCode() * Premisse.GetHashCode() ^ Consequence.GetHashCode();
        }

        public bool Equals(AssociationRule other)
        {
            return
                Premisse.Equals(other.Premisse)
                && Consequence.Equals(other.Consequence)
                && Confidence.Equals(other.Confidence);
        }

        public override bool Equals(object other)
        {
            var otherRule = other as AssociationRule;

            return Equals(otherRule);
        }

        public double CalculateConfidence()
        {
            return -1;
        }

        public override string ToString()
        {
            return "[" + Premisse.ToString() + " => " + Consequence.ToString() + ", Confidence: " + Confidence + "]";
        }
    }
}
