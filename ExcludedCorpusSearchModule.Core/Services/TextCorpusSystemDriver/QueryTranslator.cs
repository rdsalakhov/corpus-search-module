using CorpusSearchModule.Core.Models;
using CorpusSearchModule.Core.Services.TextCorpusSystemDriver.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorpusSearchModule.Core.Services.TextCorpusSystemDriver
{
    public class QueryTranslator
    {
        public Query TranslateQuery(IntermediateQuery intQuery)
        {
            if (IsExactSearchQuery(intQuery))
            {
                return TranslateExactQuery(intQuery);
            }
            else
            {
                return TranslatePairQuery(intQuery);
            }
        }

        private Query TranslatePairQuery(IntermediateQuery intQuery)
        {
            var pairQuery = new PairQuery();
            if (IsLemmaQuery(intQuery))
            {
                pairQuery.Type = PairQueryType.LemmaPair;
                pairQuery.First = intQuery.Tokens[0].SemanticParameters[0];
                pairQuery.Second = intQuery.Tokens[1].SemanticParameters[0];
                pairQuery.MinRange = intQuery.Tokens[1].MinRange;
                pairQuery.MaxRange = intQuery.Tokens[1].MaxRange;
            }
            else
            {
                pairQuery.Type = PairQueryType.TagPair;
                pairQuery.First = intQuery.Tokens[0].GrammarParameters[0];
                pairQuery.Second = intQuery.Tokens[1].GrammarParameters[0];
                pairQuery.MinRange = intQuery.Tokens[1].MinRange;
                pairQuery.MaxRange = intQuery.Tokens[1].MaxRange;
            }
            return pairQuery;
        }

        private Query TranslateExactQuery(IntermediateQuery intQuery)
        {
            return new ExactQuery
            {
                ExactForm = intQuery.ExactForm,
            };
        }

        private bool IsExactSearchQuery(IntermediateQuery query)
        {
            return query.ExactForm != "";
        }

        private bool IsLemmaQuery(IntermediateQuery intQuery)
        {
            return intQuery.Tokens[0].SemanticParameters.Count() > 0;
        }
    }
}
