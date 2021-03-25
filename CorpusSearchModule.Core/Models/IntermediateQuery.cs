using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorpusSearchModule.Core.Models
{
    public class IntermediateQuery
    {
        public string ExactForm {get; set; }
        public List<QueryToken> Tokens { get; set; }

        public IntermediateQuery()
        {
            Tokens = new List<QueryToken>();
        }
    }

    public class QueryToken
    {
        public List<string> GrammarParameters { get; set; }
        public List<string> SemanticParameters { get; set; }
        public int MinRange { get; set; }
        public int MaxRange { get; set; }

        public QueryToken()
        {
            GrammarParameters = new List<string>();
            SemanticParameters = new List<string>();
        }
    }
}
