using Model.Action;
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

        public Simulation(Dictionary<string, Good> goods)
        {
            this.Goods = goods ?? new Dictionary<string, Good>();
            this.Agents = new Dictionary<int, Agent>();
        }
        public void AddAgents(IEnumerable<Agent> agents)
        {
            var newAgents = agents.ToList();
            foreach(var agent in agents)
            {
                agent.AgentNumber = this.nextValidAgentNumber;
                this.nextValidAgentNumber++;
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
            if (deadAgents.Any())
            {
                Console.WriteLine(String.Format(
                    "Agents [{0}] have died.",
                    String.Join(",", deadAgents.Select(o=>o.Value.AgentNumber))));
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
                if (actions == null) continue;
                while (actions.Any())
                {
                    var a = actions.Dequeue();
                    Log(a);
                    HandleAction(agent.Value, a);
                }
            }
        }

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
                        Console.WriteLine(String.Format(
                            "Agent {0} is consuming {1} units of {2}.",
                            act.Consumer.AgentNumber,
                            act.Quantity,
                            act.Good.Name));
                    }
                    break;
                case ActionType.Buy:
                    {
                        var act = (BuyAction)action;
                        Console.WriteLine(String.Format(
                            "Agent {0} is buying {1} units of {2} from {3} for {4} credits total ({5} credits per unit).",
                            act.Buyer.AgentNumber,
                            act.Quantity,
                            act.Purchasable.Stack.Good.Name,
                            act.Purchasable.SellersNumber,
                            act.Purchasable.PricePerUnit * act.Quantity,
                            act.Purchasable.PricePerUnit));
                    }
                    break;
                case ActionType.Sell:
                    {
                        var act = (SellAction)action;
                        Console.WriteLine(String.Format(
                            "Agent {0} is selling {1} units of {2} at {3} credits per unit.",
                            act.Seller.AgentNumber,
                            act.Quantity,
                            act.Good.Name,
                            act.PricePerUnit));
                    }
                    break;
                default:
                    Console.WriteLine(String.Format(
                        "Action '{0}' not logged.", action.ActionType));
                    break;
            }
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

        private int nextValidAgentNumber = 1;

        public event EventHandler<AgentEventArgs> AgentDied;
        public event EventHandler<AgentActionEventArgs> AgentIsPondering;

        private void HandleAction(Agent agent, Action.IAction action)
        {
            switch (action.ActionType)
            {
                case Model.Action.ActionType.Consume:
                    HandleConsumeAction(agent, (ConsumeAction)action);
                    break;
                case Model.Action.ActionType.Buy:
                    HandleBuyAction(agent, (BuyAction)action);
                    break;
                case Model.Action.ActionType.Sell:
                    HandleSellAction(agent, (SellAction)action);
                    break;
                default:
                    throw new NotImplementedException(String.Format(
                        "Handling of agent action '{0}' not implemented yet.",
                        action.ActionType));
            }
        }

        private void HandleSellAction(Agent agent, SellAction sellAction)
        {
            var taken = agent.TakeItemFrom(
                sellAction.Good, sellAction.Quantity);
            sellAction.Market.Sell(agent, taken, sellAction.PricePerUnit);
        }
        private void HandleBuyAction(
            Agent actor, BuyAction buyAction)
        {
            var sn = buyAction.Purchasable.SellersNumber;
            var seller = this.Agents[sn];

            buyAction.Market.Purchase(
                actor,
                seller,
                buyAction.Purchasable,
                buyAction.Quantity);
        }
        private void HandleConsumeAction(
            Agent agent,
            ConsumeAction consumeAction)
        {
            agent.Consume(
                consumeAction.Good,
                consumeAction.Quantity);
        }

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
                "Simulation:\r\n agents: {0}\r\n goods: {1}\r\n"+
                " market: {2}\r\n number: {3}",
                a, g, m, n);
        }
    }
}
