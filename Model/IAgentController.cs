using Model.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    interface IAgentController
    {
        IList<IAction> GetActions(
            Agent agent,
            Simulation simulation);
    }
}
