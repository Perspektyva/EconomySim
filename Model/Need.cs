using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Need
    {
        public Good Good { get; private set; }
        public double MaxQuantity { get; private set; }
        public double CurrentQuantity { get; private set; }
        public double DecayRate { get; private set; }
        public Need(
            Good good,
            double maxQuantity,
            double currentQuantity,
            double decayRate)
        {
            this.Good = good;
            this.MaxQuantity = maxQuantity;
            this.CurrentQuantity = currentQuantity;
            this.DecayRate = decayRate;
        }

        internal void Decay(double hours)
        {
            this.CurrentQuantity -= (this.DecayRate * hours);
        }

        public override bool Equals(object obj)
        {
            if (Object.ReferenceEquals(this, obj))
                return true;
            if (!(obj is Need))
                return false;
            var other = (Need)obj;
            return
                this.CurrentQuantity == other.CurrentQuantity &&
                this.DecayRate == other.DecayRate &&
                Object.Equals(this.Good, other.Good) &&
                this.MaxQuantity == other.MaxQuantity;
        }

        internal Stack Satisfy(Stack stack, double quantity)
        {
            var deficit = MaxQuantity - CurrentQuantity;
            var normalizedQuantity = Math.Min(quantity, deficit);
            this.CurrentQuantity += normalizedQuantity;
            return stack.Split(normalizedQuantity).Item2;
        }
    }
}
