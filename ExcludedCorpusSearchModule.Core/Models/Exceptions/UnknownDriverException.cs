using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorpusSearchModule.Core.Models.Exceptions
{
    public class UnknownDriverException : Exception
    {
        public string DriverName { get; set; }

        public UnknownDriverException(string message, string driverName) : base(message)
        {
            DriverName = driverName;
        }
    }
}
