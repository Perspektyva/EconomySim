using Model;
using Model.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSimulator
{
    class FileLogger : ILogger
    {
        public FileLogger(ITimeProvider timeProvider, string fileName)
        {
            this.fileName = fileName;
            this.timeProvider = timeProvider;
        }

        string fileName;
        ITimeProvider timeProvider;
        readonly string format = "[{0}]=== {1}";

        public void Log(string format, params object[] parameters)
        {
            var message = FormatMessage(format, parameters);
            WriteToFile(message);
        }

        private void WriteToFile(string message)
        {
            using (var stream = File.Open(this.fileName, FileMode.Append))
            using (var writer = new StreamWriter(stream))
            {
                writer.WriteLine(message);
            }
        }

        private string FormatMessage(string format, object[] parameters)
        {
            var userMessage = String.Format(format, parameters);
            var fullMessage = String.Format(
                this.format,
                this.timeProvider.CurrentTime,
                userMessage);
            return fullMessage;
        }
    }
}
