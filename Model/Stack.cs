using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Stack
    {
        public Good Good { get; private set; }
        public double Quantity { get; private set; }
        public Stack(Good good, double quantity)
        {
            this.Good = good;
            this.Quantity = quantity;
        }
        public Tuple<Stack, Stack> Split(double quantityOfItem1)
        {
            if (quantityOfItem1 > this.Quantity)
                throw new InvalidOperationException(String.Format(
                    "Cannot split stack of size '{0}' into ('{1},*')",
                    this.Quantity, quantityOfItem1));
            if (quantityOfItem1 == this.Quantity)
                return new Tuple<Stack, Stack>(
                    new Stack(this.Good, quantityOfItem1),
                    null);

            var g = this.Good;
            var q1 = quantityOfItem1;
            var q2 = this.Quantity - quantityOfItem1;
            return new Tuple<Stack, Stack>(
                new Stack(g, q1),
                new Stack(g, q2));
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Stack))
                return false;
            Stack other = (Stack)obj;
            return
                Object.Equals(this.Good, other.Good) &&
                this.Quantity == other.Quantity;
        }
    }
}
