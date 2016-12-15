using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Action
{
    public class BuyAction : IAction
    {
        public ActionType ActionType
        {
            get { return Action.ActionType.Buy; }
        }
        public Simulation Simulation { get; private set; }
        public Market Market { get; private set; }
        public Purchasable Purchasable { get; private set; }
        public Agent Buyer { get; private set; }
        public double Quantity { get; private set; }
        public BuyAction( Simulation simulation,
            Market market, Purchasable purchasable,
            Agent buyer, double quantity)
        {
            this.Simulation = simulation;
            this.Market = market;
            this.Purchasable = purchasable;
            this.Buyer = buyer;
            this.Quantity = quantity;
        }

        public void PerformAction(Agent guy)
        {
            var sn = this.Purchasable.SellersNumber;
            var seller = this.Simulation.Agents[sn];

            this.Market.Purchase(
                guy,
                seller,
                this.Purchasable,
                this.Quantity);
        }
    }
}
