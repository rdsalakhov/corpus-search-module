using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorpusSearchModule.Core.Models.Exceptions
{
    public class InvalidQuerySyntaxException : Exception
    {
        public string UnnesessarySyntax { get; set; }
        public InvalidQuerySyntaxException(string message, string unnecessarySyntax) : base(message)
        {
            UnnesessarySyntax = unnecessarySyntax;
        }
    }
}
