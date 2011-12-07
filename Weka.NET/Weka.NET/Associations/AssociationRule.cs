namespace Weka.NET.Associations
{
    using System;
    using System.Text;
    using Weka.NET.Lang;
    
    [Immutable]
    public class AssociationRule : IEquatable<AssociationRule>
    {
        public ItemSet FullRule { get; private set; }

        public ItemSet Premisse { get; private set; }

        public ItemSet Consequence {get; private set;}

        public double Confidence { get { return (double)FullRule.Support / (double)Premisse.Support; } }

        /// <summary>
        /// Support of LHS Union RHS
        /// </summary>
        public double Support { get { return FullRule.Support; } }

        public AssociationRule(ItemSet premisse, ItemSet consequence, ItemSet fullRule)
        {
            this.Premisse = premisse;
            this.Consequence = consequence;
            this.FullRule = fullRule;
        }

        public override int GetHashCode()
        {
            return 37 * Premisse.GetHashCode() ^ Consequence.GetHashCode() ^ Confidence.GetHashCode();
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

        public override string ToString()
        {
            return "AssociationRule[Premisse=" + Premisse.ToString()
                + ", Consequence=" + Consequence.ToString()
                + ", Confidence: " + Confidence
                + ", Support: " + Support + "]";
        }
    }
}
