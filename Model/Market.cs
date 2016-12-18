using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Market
    {
        public IEnumerable<Purchasable> Purchasables
        {
            get
            {
                return this.purchasables;
            }
        }
        public Market(IEnumerable<Purchasable> purchasables)
        {
            this.purchasables = purchasables.ToList();
        }

        public void Purchase(Agent buyer, Agent seller,
            Purchasable purchasable, double quantity)
        {
            var p = this.purchasables
                .Where(o => o == purchasable)
                .First();
            p.Purchase(buyer, seller, quantity);
            if (p.Stack.Quantity <= 0.0)
                this.purchasables.Remove(p);
        }

        List<Purchasable> purchasables = new List<Purchasable>();

        internal IEnumerable<Purchasable> RemoveFromMarketSoldBy(Agent agent)
        {
            var unownedGoods = this.Purchasables
                .Where(o => o.SellersNumber == agent.AgentNumber)
                .ToArray();
            this.purchasables
                .RemoveAll(o => o.SellersNumber == agent.AgentNumber);
            return unownedGoods;
        }

        public override bool Equals(object obj)
        {
            if (Object.ReferenceEquals(this, obj))
                return true;
            if (!(obj is Market))
                return false;
            var other = obj as Market;
            return
                this.Purchasables.SequenceEqual(other.Purchasables);
        }
        public override int GetHashCode()
        {
            return this.purchasables.GetHashCode();
        }
        public override string ToString()
        {
            var p = "[" + String.Join(", ", this.purchasables) + "]";
            return String.Format("{0}", p);
        }

        internal void Sell(Agent seller, Stack goodsToSell, double pricePerUnit)
        {
            var agentPurchasables = purchasables
                .FirstOrDefault(o =>
                    o.SellersNumber == seller.AgentNumber &&
                    o.Stack.Good == goodsToSell.Good &&
                    o.PricePerUnit == pricePerUnit);

            if (agentPurchasables == null)
            {
                purchasables.Add(new Purchasable(
                    seller.AgentNumber,
                    pricePerUnit,
                    goodsToSell));
            }
            else
            {
                var newStack = new Stack(
                    goodsToSell.Good,
                    goodsToSell.Quantity + agentPurchasables.Stack.Quantity);
                var newP = new Purchasable(
                    seller.AgentNumber,
                    pricePerUnit,
                    newStack);
            }
        }
    }
}
