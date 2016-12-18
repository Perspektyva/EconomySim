using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.Action;
namespace Model.Tests
{
    [TestClass()]
    public class SimulationTests
    {
        [TestMethod()]
        public void Simulation_Purchase_CreditsDeduced()
        {
            var goods = new[] { "Food" }
                .ToDictionary(o => o, o => new Good(o));
            var s = new Simulation(goods);
            var f = goods["Food"];
            var ab = new Agent(Enumerable.Empty<Need>(), 100.0);
            var @as = new Agent(Enumerable.Empty<Need>(), 100.0);
            s.AddAgents(new[] { ab, @as });
            var p = new Purchasable(
                @as.AgentNumber, 40.0, new Stack(f, 100.0));
            var m = new Market(new[] { p });
            s.AddMarket(m);
            s.AgentIsPondering += (snd, args) =>
                {
                    args.EnqueueAction(new BuyAction(s, m, p, ab, 2));
                    args.Handled = true;
                };

            s.Iterate();

            Assert.AreEqual(20.0, ab.Credits);
        }
        [TestMethod()]
        public void Simulation_Purchase_ItemAdded()
        {
            var goods =
                new[] { "Food" }
                .ToDictionary(o => o, o => new Good(o));
            var s = new Simulation(goods);
            var f = goods["Food"];
            var ab = new Agent(Enumerable.Empty<Need>(), 100.0);
            var @as = new Agent(Enumerable.Empty<Need>(), 100.0);
            s.AddAgents(new[] { ab, @as });
            var p = new Purchasable(
                @as.AgentNumber, 40.0, new Stack(f, 100.0));
            var m = new Market(new[] { p });
            s.AddMarket(m);
            s.AgentIsPondering += (sender, args) =>
                {
                    args.EnqueueAction(new BuyAction(s, m, p, ab, 2.0));
                    args.Handled = true;
                };

            s.Iterate();

            Assert.AreEqual<int>(1, ab.Assets.Count);
            Assert.AreEqual<Stack>(new Stack(f, 2.0), ab.Assets[f]);
        }
        [TestMethod()]
        public void Simulation_Purchase_CreditsDeposited()
        {
            var goods = new[] { "Food" }
                .ToDictionary(o => o, o => new Good(o));
            var s = new Simulation(goods);
            var f = goods["Food"];
            var ab = new Agent(Enumerable.Empty<Need>(), 100.0);
            var @as = new Agent(Enumerable.Empty<Need>(), 100.0);
            s.AddAgents(new[] { ab, @as });
            var p = new Purchasable(
                @as.AgentNumber, 40.0, new Stack(f, 100.0));
            var m = new Market(new[] { p });
            s.AddMarket(m);
            s.AgentIsPondering += (sender, args) =>
                {
                    args.EnqueueAction(new BuyAction(s, m, p, ab, 2.0));
                    args.Handled = true;
                };

            s.Iterate();

            Assert.AreEqual<double>(180.0, @as.Credits);
        }
        [TestMethod()]
        public void Simulation_Purchase_PurchasableDeduced()
        {
            var goods = new[] { "Food" }
                .ToDictionary(o => o, o => new Good(o));
            var simulation = new Simulation(goods);
            var food = goods["Food"];
            var buyer = new Agent(Enumerable.Empty<Need>(), 100.0);
            var seller = new Agent(Enumerable.Empty<Need>(), 100.0);
            simulation.AddAgents(new[] { buyer, seller });
            var purchasable = new Purchasable(
                seller.AgentNumber, 40.0, new Stack(food, 100.0));
            var market = new Market(new[] { purchasable });
            simulation.AddMarket(market);
            simulation.AgentIsPondering += (sender, args) =>
            {
                if (args.Agent != buyer)
                    return;
                args.EnqueueAction(new BuyAction(
                    simulation, market, purchasable, buyer, 2.0));
                args.Handled = true;
            };

            simulation.Iterate();

            Assert.AreEqual(1, market.Purchasables.Count());
            Assert.AreEqual(98.0, market.Purchasables.First().Stack.Quantity);
        }

        [TestMethod()]
        public void Simulation_Iterate_NeedDeteriorates()
        {
            var goods = new[] { "Food" }
                .ToDictionary(o => o, o => new Good(o));
            var s = new Simulation(goods);
            var f = goods["Food"];
            var need = new Need(f, 30000, 20000, 150.0);
            var a = new Agent(new[] { need }, 100.0);
            s.AddAgents(new[] { a });

            s.Iterate(1.0);

            Assert.AreEqual(1, s.Agents.Count());
            var aa = s.Agents.First().Value;
            Assert.AreEqual<double>(
                20000.0 - 150.0,
                aa.Needs[f].CurrentQuantity);

            s.Iterate(2.0);

            Assert.AreEqual(1, s.Agents.Count());
            aa = s.Agents.First().Value;
            Assert.AreEqual<double>(
                20000.0 - (150.0 * 3.0),
                aa.Needs[f].CurrentQuantity);
        }

        [TestMethod()]
        public void Simulation_Iterate_AgentsDie()
        {
            var goodsa = new[] { "Food" }
                .ToDictionary(o => o, o => new Good(o));
            var sa = new Simulation(goodsa);
            var fa = goodsa["Food"];
            var needa = new Need(fa, 30000, 0.0, 150.0);
            var aa = new Agent(new[] { needa }, 100.0);
            sa.AddAgents(new[] { aa });
            var pa = new Purchasable(
                aa.AgentNumber, 4.0, new Stack(fa, 20.0));
            var ma = new Market(new[] { pa });
            sa.AddMarket(ma);

            sa.Iterate(1.0);

            var goodse = new[] { "Food" }
                .ToDictionary(o => o, o => new Good(o));
            var se = new Simulation(goodse);
            var fe = goodse["Food"];
            var me = new Market(Enumerable.Empty<Purchasable>());
            se.AddMarket(me);

            Assert.AreEqual(se, sa);
        }
        [TestMethod()]
        public void Simulation_Iterate_AgentConsumes()
        {
            var goodsa = new[] { "Food" }
               .ToDictionary(o => o, o => new Good(o));
            var sa = new Simulation(goodsa);
            var fa = goodsa["Food"];
            var needa = new Need(fa, 30000, 20000.0, 150.0);
            var aa = new Agent(new[] { needa }, 100.0);
            aa.GiveItemTo(new Stack(fa, 100000.0));
            sa.AddAgents(new[] { aa });
            var ma = new Market(Enumerable.Empty<Purchasable>());
            sa.AddMarket(ma);
            sa.AgentIsPondering += (sender, args) =>
                {
                    args.EnqueueAction(new ConsumeAction(aa, fa, 2000.0));
                    args.Handled = true;
                };

            sa.Iterate(1.0);

            var goodse = new[] { "Food" }
               .ToDictionary(o => o, o => new Good(o));
            var se = new Simulation(goodse);
            var fe = goodsa["Food"];
            var neede = new Need(
                fe, 30000, 20000.00 + 2000.0 - 150.0, 150.0);
            var ae = new Agent(new[] { neede }, 100.0);
            ae.GiveItemTo(new Stack(fa, 100000.0 - 2000.0));
            se.AddAgents(new[] { ae });
            var me = new Market(Enumerable.Empty<Purchasable>());
            se.AddMarket(me);

            Assert.AreEqual(se, sa);
        }

        Simulation SimWithAgentWithSellAction()
        {
            var goods = new[] { "Food" }
               .ToDictionary(o => o, o => new Good(o));
            var sim = new Simulation(goods);
            var food = goods["Food"];
            var agent = new Agent(Enumerable.Empty<Need>(), 100.0);
            agent.GiveItemTo(new Stack(food, 100000.0));
            sim.AddAgents(new[] { agent });
            var market = new Market(Enumerable.Empty<Purchasable>());
            sim.AddMarket(market);
            sim.AgentIsPondering += (snd, ea) =>
            {
                ea.EnqueueAction(new SellAction(
                    agent, food, 80000.0, market, 0.01));
                ea.Handled = true;
            };
            return sim;
        }
        Simulation SimWithAgentFoodInMarket()
        {
            var goods = new[] { "Food" }
               .ToDictionary(o => o, o => new Good(o));
            var sim = new Simulation(goods);
            var food = goods["Food"];
            var agent = new Agent(Enumerable.Empty<Need>(), 100.0);
            agent.GiveItemTo(new Stack(food, 20000.0));
            sim.AddAgents(new[] { agent });
            var purchasable = new Purchasable(
                agent.AgentNumber,
                0.01, new Stack(food, 80000.0));
            var market = new Market(new[] { purchasable });
            sim.AddMarket(market);
            return sim;
        }

        [TestMethod()]
        public void Simulation_Iterate_AgentAttemptsToSell()
        {
            var actual = SimWithAgentWithSellAction();
            actual.Iterate(1.0);
            var expected = SimWithAgentFoodInMarket();
            Assert.AreEqual(expected, actual);
        }

        Simulation SimulationWithSellerWithPurchAndBuyer()
        {
            var food = new Good("food");
            var goods = new[] { food }.ToDictionary(o => o.Name, o => o);
            var simulation = new Simulation(goods);
            var seller = new Agent(null, 0.0);
            simulation.AddAgents(new[] { seller });
            var purchasable = new Purchasable(
                seller.AgentNumber, 10.0, new Stack(food, 10.0));
            var market = new Market(new[] { purchasable });
            simulation.AddMarket(market);
            var buyer = new Agent(null, 200.0);
            simulation.AddAgents(new[] { buyer });
            simulation.AgentIsPondering += (s, e) =>
            {
                if (e.Agent == buyer)
                {
                    e.EnqueueAction(new BuyAction(
                        simulation,
                        market,
                        purchasable,
                        buyer,
                        10.0));
                    e.Handled = true;
                }
            };
            return simulation;
        }

        [TestMethod()]
        public void Simulation_Iterate_PurchasableStackGetsDeleted()
        {
            var actual = SimulationWithSellerWithPurchAndBuyer();
            actual.Iterate();
            Assert.AreEqual(0, actual.Market.Purchasables.Count());
        }
    }
}
