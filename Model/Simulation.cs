using Model.Action;
using Model.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Simulation
    {
        public Dictionary<int, Agent> Agents { get; private set; }
        public Dictionary<string, Good> Goods { get; private set; }
        public Market Market { get; private set; }
        public Clock Clock { get; private set; }

        public Simulation(
            Dictionary<string, Good> goods,
            Clock clock = null,
            ILogger logger = null)
        {
            this.Goods = goods ?? new Dictionary<string, Good>();
            this.Agents = new Dictionary<int, Agent>();
            this.Clock = clock ?? new Clock(DateTime.MinValue);
            this.logger = logger ?? NullLogger.Instance;
        }
        public void AddAgents(IEnumerable<Agent> agents)
        {
            foreach(var agent in agents)
            {
                agent.AgentNumber = this.nextValidAgentNumber++;
                this.Agents[agent.AgentNumber] = agent;
            }
        }
        public void AddMarket(Market market)
        {
            this.Market = market;
        }
        public void Iterate(double hours = 1.0)
        {
            var deadAgents = this.Agents
                .Where(agent => agent.Value.Needs
                    .Any(need => need.Value.CurrentQuantity <= 0.0))
                .ToList();
            this.Clock.AdvanceByHours(hours);
            if (deadAgents.Any())
            {
                var message = GetDeadAgentReport(deadAgents);
                this.logger.Log(message);
            }
            deadAgents
                .ForEach(RemoveAgent);

            foreach (var agent in this.Agents)
            {
                foreach(var need in agent.Value.Needs)
                {
                    need.Value.Decay(hours);
                }
            }

            foreach(var agent in this.Agents)
            {
                var actions = RaiseAgentIsPondering(agent.Value);
                if (actions == null)
                {
                    continue;
                }
                while (actions.Any())
                {
                    var action = actions.Dequeue();
                    Log(action);
                    action.PerformAction(agent.Value);
                }
            }
        }

        private string GetDeadAgentReport(List<KeyValuePair<int, Agent>> deadAgents)
        {
            var message = String.Format(
                "Agents [{0}] have died.",
                String.Join(",", deadAgents.Select(o => o.Value.AgentNumber)));
            return message;
        }
        public event EventHandler<AgentEventArgs> AgentDied;
        public event EventHandler<AgentActionEventArgs> AgentIsPondering;
        public override bool Equals(object obj)
        {
            if (Object.ReferenceEquals(this, obj))
                return true;
            if (!(obj is Simulation))
                return false;
            Simulation other = obj as Simulation;

            return
                this.Agents.SequenceEqual(other.Agents) &&
                this.Goods.SequenceEqual(other.Goods) &&
                Object.Equals(this.Market, other.Market);
        }
        public override string ToString()
        {
            var a = "[" + String.Join(", ", this.Agents) + "]";
            var g = "[" + String.Join(", ", this.Goods) + "]";
            var m = this.Market.ToString();
            var n = this.nextValidAgentNumber.ToString();
            return String.Format(
                "Simulation:\r\n agents: {0}\r\n goods: {1}\r\n" +
                " market: {2}\r\n number: {3}",
                a, g, m, n);
        }

        private int nextValidAgentNumber = 1;
        readonly ILogger logger = NullLogger.Instance;

        private Queue<IAction> RaiseAgentIsPondering(
            Agent agent)
        {
            var args = new AgentActionEventArgs(agent);
            if (this.AgentIsPondering != null)
                this.AgentIsPondering(this, args);
            return args.Actions;
        }
        private void Log(IAction action)
        {
            switch (action.ActionType)
            {
                case ActionType.Consume:
                    {
                        var act = (ConsumeAction)action;
                        var report = GetConsumeActionReport(act);
                        this.logger.Log(report);
                    }
                    break;
                case ActionType.Buy:
                    {
                        var act = (BuyAction)action;
                        var report = GetBuyActionReport(act);
                        this.logger.Log(report);
                    }
                    break;
                case ActionType.Sell:
                    {
                        var act = (SellAction)action;
                        var report = GetSellActionReport(act);
                        this.logger.Log(report);
                    }
                    break;
                default:
                    {
                        var report = GetUnhandledActionReport(action);
                        this.logger.Log(report);
                    }
                    break;
            }
        }

        private static string GetUnhandledActionReport(IAction action)
        {
            return String.Format(
                "Action '{0}' was not handled.", action.ActionType);
        }

        private static string GetSellActionReport(SellAction act)
        {
            return String.Format(
                "Agent {0} is selling {1} units of {2} at {3} credits per unit.",
                act.Seller.AgentNumber,
                act.Quantity,
                act.Good.Name,
                act.PricePerUnit);
        }

        private static string GetBuyActionReport(BuyAction act)
        {
            return String.Format(
                "Agent {0} is buying {1} units of {2} from {3} for {4} credits total ({5} credits per unit).",
                act.Buyer.AgentNumber,
                act.Quantity,
                act.Purchasable.Stack.Good.Name,
                act.Purchasable.SellersNumber,
                act.Purchasable.PricePerUnit * act.Quantity,
                act.Purchasable.PricePerUnit);
        }

        private static string GetConsumeActionReport(ConsumeAction act)
        {
            return String.Format(
                "Agent {0} is consuming {1} units of {2}.",
                act.Consumer.AgentNumber,
                act.Quantity,
                act.Good.Name);
        }
        private void RemoveAgent(KeyValuePair<int, Agent> obj)
        {
            this.Agents.Remove(obj.Key);
            this.Market.RemoveFromMarketSoldBy(obj.Value);
            RaiseAgentDied(obj.Value);
        }
        private void RaiseAgentDied(Agent agent)
        {
            if (this.AgentDied != null)
                this.AgentDied(this, new AgentEventArgs(agent));
        }
    }
}
