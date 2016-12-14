using Model.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    class BrainDead : IAgentController
    {
        public IList<IAction> GetActions(Agent agent)
        {
            return new List<IAction>();
        }
    }
}
