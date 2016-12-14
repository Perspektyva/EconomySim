using Model.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class AgentActionEventArgs : AgentEventArgs
    {
        public bool Handled { get; set; }
        public Queue<IAction> Actions { get; private set; }

        public AgentActionEventArgs(Agent agent)
            : base(agent)
        {
            this.Handled = false;
            this.Actions = new Queue<IAction>();
        }
        public void EnqueueAction(IAction action) {
            if (action == null)
                throw new ArgumentNullException("action");
            this.Actions.Enqueue(action);
        }

    }
}
