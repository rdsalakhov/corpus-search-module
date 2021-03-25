using CorpusSearchModule.Core.Models;
using CorpusSearchModule.Core.Services.DriverInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorpusSearchModule.Core.Services.TextCorpusSystemDriver
{
    public class TextCorpusSystemValidator : IDriverValidator
    {
        public bool ValidateQuery(IntermediateQuery query, out string message)
        {
            if (query.ExactForm == null && query.Tokens.Count() == 0) 
            {
                message = "empty query";
                return false;
            }
            if (query.ExactForm != null && query.Tokens.Count() > 0)
            {
                message = "message can't contain exact form and parameter tokens at the same time";
                return false;
            }
            if (query.ExactForm != null)
            {
                message = "";
                return true;
            }
            if (!IsValidTokenCount(query.Tokens))
            {
                message = "parameter token count must be in range 1..2";
                return false;
            }
            if (!IsValidParameterTypes(query.Tokens))
            {
                message = "parameters should be all grammar or all semantic";
                return false;
            }
            if (!IsValidRangeParameters(query.Tokens))
            {
                message = "invalid range parameters";
                return false;
            }
            message = "";
            return true;
        }     

        private bool IsValidTokenCount(List<QueryToken> tokens)
        {
            return tokens.Count <= 2 && tokens.Count > 0;
        }

        private bool IsValidParameterTypes(List<QueryToken> tokens)
        {
            for (int i = 1; i < tokens.Count(); i++)
            {
                bool isDifferentParameterTypes = (tokens[i].SemanticParameters.Count() > 0 && tokens[i - 1].GrammarParameters.Count() > 0) ||
                    (tokens[i].GrammarParameters.Count() > 0 && tokens[i - 1].SemanticParameters.Count() > 0);
                if (isDifferentParameterTypes)
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsValidRangeParameters(List<QueryToken> tokens)
        {
            // not validating first token because it doesn't have range parameters
            for (int i=1; i < tokens.Count(); i++)
            {
                if (tokens[i].MaxRange < 1 || tokens[i].MinRange < 1)
                {
                    return false;
                }
                if (tokens[i].MaxRange < tokens[i].MinRange)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
