using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Action
{
    public class SellAction : IAction
    {
        public ActionType ActionType
        {
            get { return Action.ActionType.Sell; }
        }

        public Agent Seller { get; private set; }
        public Good Good { get; private set; }
        public double Quantity { get; private set; }
        public Market Market { get; private set; }
        public double PricePerUnit { get; private set; }

        public SellAction(
            Agent seller, Good good, double quantity,
            Market market, double pricePerUnit)
        {
            this.Seller = seller;
            this.Good = good;
            this.Quantity = quantity;
            this.Market = market;
            this.PricePerUnit = pricePerUnit;
        }


        public void PerformAction(Agent guy)
        {
            var taken = guy.TakeItemFrom(
                this.Good, this.Quantity);
            this.Market.Sell(guy, taken, this.PricePerUnit);
        }
    }
}
