using Model.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Agent
    {
        public int AgentNumber
        {
            get
            {
                return this.agentNumber;
            }
            set
            {
                if (this.agentNumber != -1)
                    throw new InvalidOperationException("Number reasignbment not allowed.");
                this.agentNumber = value;
            }
        }
        public IReadOnlyDictionary<Good, Need> Needs
        {
            get
            {
                return this.needs;
            }
        }
        public double Credits { get; private set; }
        public IReadOnlyDictionary<Good, Stack> Assets
        {
            get
            {
                return this.assets;
            }
        }
        public Queue<IAction> Actions { get; private set; }

        public Agent(
            IEnumerable<Need> needs,
            double credits)
        {
            this.needs = needs
                .ToDictionary(o => o.Good, o => o);
            this.Credits = credits;
            this.Actions = new Queue<IAction>();
            this.agentNumber = -1;
        }

        public void SetNumber(int number)
        {

        }
        public void WithdrawFrom(double totalPrice)
        {
            this.Credits -= totalPrice;
        }
        public void GiveItemTo(Stack stack)
        {
            var g = stack.Good;
            var q = assets.ContainsKey(g) ? assets[g].Quantity : 0.0;
            assets[g] = new Stack(g, stack.Quantity + q);
        }
        public void DepositTo(double totalPrice)
        {
            this.Credits += totalPrice;
        }
        internal void Consume(Good good, double quantity)
        {
            this.assets[good] = this.needs[good].Satisfy(
                this.assets[good],
                quantity);
        }
        internal Stack TakeItemFrom(Good good, double quantity)
        {
            var sp = this.assets[good].Split(quantity);
            this.assets[good] = sp.Item2;
            return sp.Item1;
        }

        private Dictionary<Good, Stack> assets = new Dictionary<Good, Stack>();
        private Dictionary<Good, Need> needs = new Dictionary<Good, Need>();
        private int agentNumber = -1;

        public override bool Equals(object obj)
        {
            if (Object.ReferenceEquals(this, obj))
                return true;
            if (!(obj is Agent))
                return false;
            var other = obj as Agent;
            return
                this.agentNumber == other.agentNumber &&
                this.Needs.SequenceEqual(other.Needs) &&
                this.Credits == other.Credits &&
                this.Assets.SequenceEqual(other.Assets) &&
                this.Actions.SequenceEqual(other.Actions);
        }
        public override string ToString()
        {
            var num = this.agentNumber.ToString();
            var needs = "[" + String.Join(", ", this.Needs.Values) + "]";
            var c = this.Credits.ToString();
            var a = "[" + String.Join(", ", this.Assets.Values) + "]";
            var aa = "[" + String.Join(", ", this.Actions) + "]";

            return String.Format(
                "Agent: num: '{0}', needs: '{1}', credit: '{2}', assets: '{3}', actions: '{4}'",
                num, needs, c, a, aa);
        }


    }
}
