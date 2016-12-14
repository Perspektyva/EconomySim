using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class AgentEventArgs : EventArgs
    {
        public Agent Agent { get; private set; }
        public AgentEventArgs(Agent agent)
        {
            this.Agent = agent;
        }
    }
}
