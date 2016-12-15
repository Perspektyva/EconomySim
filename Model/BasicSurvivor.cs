using Model.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class BasicSurvivor : IAgentController
    {
        public IList<IAction> GetActions(Agent agent, Simulation simulation)
        {
            var consumeActions = TopUp(agent).Cast<IAction>();
            var stockpilingActions = GetStockpilingActions(agent, simulation);
            var credits = agent.Credits;
            var validStockPilingActions = stockpilingActions.TakeWhile(ba =>
                {
                    var p = ba.Quantity * ba.Purchasable.PricePerUnit;
                    if (credits > p)
                    {
                        credits -= p;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                })
                .Cast<IAction>();
            return consumeActions.Concat(validStockPilingActions).ToList();
        }

        private IEnumerable<ConsumeAction> TopUp(Agent agent)
        {
            return agent.GetNeedDeficits()
                .Select(o => ComputeConsumeActionOrNull(o.Item1, o.Item2, agent))
                .Where(o => o != null);
        }

        private ConsumeAction ComputeConsumeActionOrNull(Good good, double deficit, Agent agent)
        {
            var availableQuantity = agent.GetPossesedQuantity(good);
            if (availableQuantity <= 0)
            {
                return null;
            }
            var consumableQuantity = Math.Min(deficit, availableQuantity);
            return new ConsumeAction(agent, good, consumableQuantity);
        }

        private IEnumerable<BuyAction> GetStockpilingActions(
            Agent agent,
            Simulation simulation)
        {
            var shoppingList = agent.Needs
                .Select(o => new
                {
                    Good = o.Key,
                    Available = agent.GetPossesedQuantity(o.Key),
                    Threshold = 48 * o.Value.DecayRate,
                    RaiseTo = 72 * o.Value.DecayRate
                })
                .Where(o => o.Available < o.Threshold);
            return shoppingList.SelectMany(o =>
                AttemptToBuySome(
                    simulation,
                    simulation.Market,
                    agent,
                    o.Good,
                    o.RaiseTo - o.Available));
        }

        private IEnumerable<BuyAction> AttemptToBuySome(
            Simulation simulation,
            Market market,
            Agent buyer,
            Good good,
            double amount)
        {
            var potentialPurchases = market.Purchasables
                .Where(o => o.Stack.Good == good)
                .OrderBy(o => o.PricePerUnit);
            var neededQuantity = amount;
            foreach (var pp in potentialPurchases)
            {
                if (neededQuantity <= 0.0)
                {
                    yield break;
                }
                else if (pp.Stack.Quantity > neededQuantity)
                {
                    yield return new BuyAction(simulation, market, pp, buyer, neededQuantity);
                    yield break;
                }
                else
                {
                    yield return new BuyAction(simulation, market, pp, buyer, pp.Stack.Quantity);
                    neededQuantity -= pp.Stack.Quantity;
                }
            }
        }

        /// <summary>
        /// Checks which needs need fulfilling and suggests a quantity of
        /// resource that ought to be used to satisfy that need.
        /// </summary>
        /// <param name="readOnlyDictionary"></param>
        /// <returns></returns>
        private IEnumerable<Tuple<Good, double>> AssessAndSuggestNeedSatisfaction(
            IReadOnlyDictionary<Good, Need> readOnlyDictionary)
        {
            return readOnlyDictionary
                .Where(o => o.Value.CurrentQuantity < (o.Value.MaxQuantity / 2.0))
                .Select(o => Tuple.Create(o.Key, o.Value.MaxQuantity * 0.25));
        }
    }
}
