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
        public Market Market { get; set; }
        public Purchasable Purchasable { get; set; }
        public Agent Buyer { get; set; }
        public double Quantity { get; set; }
        public BuyAction(
            Market market, Purchasable purchasable,
            Agent buyer, double quantity)
        {
            this.Market = market;
            this.Purchasable = purchasable;
            this.Buyer = buyer;
            this.Quantity = quantity;
        }
    }
}
