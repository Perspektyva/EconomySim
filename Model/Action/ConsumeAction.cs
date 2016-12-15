using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Action
{
    public class ConsumeAction : IAction
    {
        public Agent Consumer { get; private set; }
        public Good Good { get; private set; }
        public double Quantity { get; private set; }
        public ConsumeAction(Agent consumer, Good good, double quantity)
        {
            this.Consumer = consumer;
            this.Good = good;
            this.Quantity = quantity;
        }



        public ActionType ActionType
        {
            get { return Action.ActionType.Consume; }
        }


        public void PerformAction(Agent guy)
        {
            guy.Consume(
                this.Good,
                this.Quantity);
        }
    }
}
