using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Action
{
    public interface IAction
    {
        ActionType ActionType { get; }
    }
}
