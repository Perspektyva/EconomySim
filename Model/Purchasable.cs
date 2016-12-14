using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public  class Purchasable
    {
        public int SellersNumber { get; private set; }
        public double PricePerUnit { get; private set; }
        public Stack Stack { get; private set; }
        public Purchasable(int sellerNumber, double pricePerUnit, Stack stack)
        {
            if (sellerNumber == -1)
                throw new InvalidOperationException(
                    "Invalid seller specified (-1).");
            this.SellersNumber = sellerNumber;
            this.PricePerUnit = pricePerUnit;
            this.Stack = stack;
        }

        public void Purchase(Agent buyer, Agent seller, double quantity)
        {
            if (this.SellersNumber != seller.AgentNumber)
                throw new InvalidOperationException(String.Format(
                    "Sellers number ({0}) and actual seller ({1}) mismatch!",
                    this.SellersNumber, seller.AgentNumber));

            var outgoingStack = new Stack(this.Stack.Good, quantity);
            this.Stack = new Stack(
                this.Stack.Good,
                this.Stack.Quantity - quantity);
            var totalPrice = this.PricePerUnit * quantity;
            buyer.GiveItemTo(outgoingStack);
            buyer.WithdrawFrom(totalPrice);
            seller.DepositTo(totalPrice);
        }

        public override bool Equals(object obj)
        {
            if (Object.ReferenceEquals(this, obj))
                return true;
            if (!(obj is Purchasable))
                return false;
            var other = obj as Purchasable;
            return
                this.PricePerUnit == other.PricePerUnit &&
                this.SellersNumber == other.SellersNumber &&
                Object.Equals(this.Stack, other.Stack);
        }


    }
}
