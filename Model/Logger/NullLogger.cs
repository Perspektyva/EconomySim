using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Logger
{
    class NullLogger : ILogger
    {
        public static NullLogger Instance
        {
            get
            {
                return NullLogger.instance;
            }
        }

        private static readonly NullLogger instance = new NullLogger();
        public void Log(string format, params object[] parameters)
        {
        }
    }
}
