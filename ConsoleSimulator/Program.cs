using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            var food = new Good("Food");
            var goods = new Dictionary<string, Good> 
                {
                    {food.Name, food}
                };
            Simulation sim = new Simulation(goods);
            var foodNeed = new Need(food, 30000, 20000, 150.0);
            var agents = SpawnAgent(new[] { foodNeed }, 500.0).Take(10).ToArray();
            sim.AddAgents(agents);
            var agentsAndTheirBrains = agents
                .ToDictionary(o => o, o => new BasicSurvivor());
            var m = new Market(new[] {
                new Purchasable(1, 0.01, new Stack(food, 1000000))});
            sim.AddMarket(m);

            sim.AgentDied += (s, e) =>
                { agentsAndTheirBrains.Remove(e.Agent); };
            sim.AgentIsPondering += (s, e) =>
            {
                e.Handled = true;
                agentsAndTheirBrains[e.Agent].GetActions(e.Agent, sim)
                    .ToList()
                    .ForEach(e.EnqueueAction);
            };

            for (int i = 0; i < 24 * 1000; ++i)
            {
                sim.Iterate(1.0);

                if (!sim.Agents.Any())
                    break;
            }
            Console.ReadKey();
        }

        public static IEnumerable<Agent> SpawnAgent(
            IEnumerable<Need> template,
            double initialCredits)
        {
            Random r = new Random();
            while (true)
            {
                var mqd = (r.NextDouble() - 0.5) * 10000;
                var cqd = (r.NextDouble() - 0.5) * 15000;
                var newNeeds = template
                    .Select(o => new Need(
                        o.Good, o.MaxQuantity + mqd, o.CurrentQuantity + cqd, o.DecayRate));
                yield return new Agent(newNeeds, initialCredits);
            }
        }
    }
}
