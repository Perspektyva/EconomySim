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
        public IList<IAction> GetActions(Agent agent)
        {
            var needs = AssessAndSuggestNeedSatisfaction(agent.Needs);
            foreach (var need in needs)
            {
#error cont. from here
                // Do we have resource on hand?
                    // Consume
                // else
                    // Are resource available on market?
                        // Do we have enough credits?
                            // Buy cheapest
                            // Consume
                        // else
                            // Attempt to sell things for credits (labour, etc...), have func be mindfull of the ulimate goal of the credit acquisition (the need).
                    // else
                        // Get desperate? Wait...
                        
            }
            // I guess reshuffle our market, if we have any purchasables.
            // Set major goals (credit acquisition ofr founding abussiness).
            // foreach bussiness
            //   manage
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
