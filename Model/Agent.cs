namespace Model
{
    using Action;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

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

        public Agent(
            IEnumerable<Need> needs,
            double credits)
        {
            needs = needs ?? Enumerable.Empty<Need>();
            this.needs = needs
                .ToDictionary(o => o.Good, o => o);
            this.Credits = credits;
            this.agentNumber = -1;
        }

        public double GetPossesedQuantity(Good good)
        {
            if (this.Assets.ContainsKey(good))
                return this.Assets[good].Quantity;
            else
                return 0.0;
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
            var good = stack.Good;
            var quantity = this.assets.ContainsKey(good) ? this.assets[good].Quantity : 0.0;
            assets[good] = new Stack(good, stack.Quantity + quantity);
        }
        public void DepositTo(double credits)
        {
            this.Credits += credits;
        }
        internal void Consume(Good good, double quantity)
        {
            this.assets[good] = this.needs[good].Satisfy(
                this.assets[good],
                quantity);
            if (this.assets[good] == null)
                this.assets.Remove(good);
        }
        internal Stack TakeItemFrom(Good good, double quantity)
        {
            var sp = this.assets[good].Split(quantity);
            if (sp.Item2 == null)
                this.assets.Remove(good);
            else
                this.assets[good] = sp.Item2;
            return sp.Item1;
        }
        public IEnumerable<Tuple<Good, double>> GetNeedDeficits()
        {
            return this.Needs.Select(o => Tuple.Create<Good, double>(
                o.Value.Good,
                o.Value.MaxQuantity - o.Value.CurrentQuantity));
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
                this.Assets.SequenceEqual(other.Assets);
        }
        public override string ToString()
        {
            var num = this.agentNumber.ToString();
            var needs = "[" + string.Join(", ", this.Needs.Values) + "]";
            var c = this.Credits.ToString();
            var a = "[" + string.Join(", ", this.Assets.Values) + "]";

            return string.Format(
                "Agent: num: '{0}', needs: '{1}', credit: '{2}', assets: '{3}'", num, needs, c, a);
        }
    }
}
