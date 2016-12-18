using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Clock : ITimeProvider
    {
        public DateTime CurrentTime
        {
            get
            {
                return this.now;
            }
        }
        private DateTime now;

        public Clock(DateTime startingTime)
        {
            this.now = startingTime;
        }

        public void AdvanceByHours(double hours)
        {
            this.now += TimeSpan.FromHours(hours);
        }
    }
}
