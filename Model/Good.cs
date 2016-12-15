using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Good
    {
        public int UniqueId { get; private set; }
        public string Name { get; private set; }
        public Good(string name)
        {
            this.Name = name;
            this.UniqueId = ComputeId(this.Name);
        }

        private int ComputeId(string name)
        {
            return name.GetHashCode();
        }
        private int uniqueId = 0;
        public override bool Equals(object obj)
        {
            if (Object.ReferenceEquals(this, obj))
                return true;
            if (!(obj is Good))
                return false;
            var other = (Good)obj;
            return this.Name == other.Name;
        }
        public override int GetHashCode()
        {
            return this.uniqueId;
        }
    }
}
