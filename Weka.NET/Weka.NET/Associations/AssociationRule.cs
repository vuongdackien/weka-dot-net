using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Weka.NET.Associations
{
    public class AssociationRule
    {
        public ItemSet Premise { get; private set; }

        public int PremiseCount { get; private set; }

        public ItemSet Consequence { get; private set; }

        public int ConsequenceCount { get; private set; }

        public AssociationRule(ItemSet premise, int premiseCount, ItemSet consequence, int consequenceCount)
        {
            Premise = premise;
            PremiseCount = premiseCount;
            Consequence = consequence;
            ConsequenceCount = consequenceCount;
        }

        public override bool Equals(object obj)
        {
            if (obj is AssociationRule)
            {
                var other = obj as AssociationRule;

                return Premise.Equals(other.Premise)
                    && PremiseCount == other.PremiseCount
                    && Consequence.Equals(other.Consequence)
                    && ConsequenceCount == other.ConsequenceCount;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return 13 * PremiseCount * ConsequenceCount;
        }

        public override string ToString()
        {
            var buff = new StringBuilder();
            buff.Append("AssociationRule[Premise=").Append(Premise.ToString())
                .Append(", PremiseCount=").Append(PremiseCount)
                .Append(", Consequence=").Append(Consequence)
                .Append(", ConsequenceCount=").Append(ConsequenceCount)
                .Append("]");

            return buff.ToString();
        }

    }
}
