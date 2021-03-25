using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpusSearchModule.Core.Models.Exceptions
{
    public class EmptyQueryException : Exception
    {
        public EmptyQueryException() : base("Empty query is not allowed")
        {

        }
    }
}
