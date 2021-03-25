using CorpusSearchModule.Core.Models;
using CorpusSearchModule.Core.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CorpusSearchModule.Core.Services
{
    public class QueryParser : IQueryParser
    {
        const string exactFormPattern = @"^FIND\s+EXACT\s+([\s|\S]+)";
        const string grammarParametersPattern = @"GRAMM\s+\((.+?)\)(?:\r|)"; 
        const string semanticParametersPattern = @"SEMANTIC\s+\((.+)\)\s*";
        const string rangeParametersPattern = @"WITHRANGE\s+FROM\s+(\d+)\s+TO\s+(\d+)\s+";
        const string tokenPattern = @"(?:(?:^FIND\r|(?:WITHRANGE\s+FROM\s+(\d{1,4}))\s+TO\s+(\d{1,4})\s*\r)(?:\n(?:GRAMM|SEMANTIC)\s+\((.+?)\)(?:\s|){1}){1,2})+";


        public IntermediateQuery ParseQuery(string query)
        {
            IntermediateQuery queryObj;
            if (TryParseExactQuery(query, out queryObj))
            {
                return queryObj;
            }


            if (!IsValidParameterQuery(query, out string unnesessarySyntax))
            {
                throw new InvalidQuerySyntaxException("Query contains unnecessary syntax that does not match with query tokens", unnesessarySyntax);
            }

            queryObj = ParseParameterQuery(query);
            return queryObj;            
        }

        private List<QueryToken> ParseParameterTokens(string query)
        {
            var tokens = new List<QueryToken>();
            Regex tokenRegex = new Regex(tokenPattern);
           

            var tokenMatches = tokenRegex.Matches(query);

            foreach (Match m in tokenMatches)
            {
                var token = new QueryToken();
                var grammPars = GetGrammarParameters(m.Value);
                var semanticPars = GetSemanticParameters(m.Value);
                var rangePars = GetRangeParameters(m.Value);

                token.GrammarParameters = grammPars ?? new List<string>();
                token.SemanticParameters = semanticPars ?? new List<string>();

                if (rangePars != null)
                {
                    token.MinRange = rangePars.Item1;
                    token.MaxRange = rangePars.Item2;
                }
                tokens.Add(token);
            }
            return tokens;
        }

        public IntermediateQuery ParseParameterQuery(string query)
        {
            var queryObj = new IntermediateQuery();

            queryObj.Tokens = ParseParameterTokens(query);

            return queryObj;
        }


        private bool TryParseExactQuery(string query, out IntermediateQuery queryObj)
        {
            Regex exactRegex = new Regex(exactFormPattern);
            queryObj = new IntermediateQuery();
            
            if (!exactRegex.IsMatch(query))
            {
                return false;
            }

            var match = exactRegex.Match(query);
            string exactForm = match.Groups[1].Value;
            queryObj.ExactForm = exactForm;
            return true;
        }

        private bool IsValidParameterQuery(string query, out string unnesessarySyntax)
        {
            var tokens = new List<QueryToken>();
            Regex tokenRegex = new Regex(tokenPattern);
            string newString = tokenRegex.Replace(query, "");
            newString = newString.Trim();
            unnesessarySyntax = newString;
            return newString == "" || newString == ";";
        }

        private List<string> GetGrammarParameters(string query)
        {
            Regex grammarRegex = new Regex(grammarParametersPattern);

            if (!grammarRegex.IsMatch(query))
            {
                return null;
            }

            var grammarMatch = grammarRegex.Match(query);         
            string gramPars = grammarMatch.Groups[1].Value;
            var parsList = gramPars.Split('|').ToList();
            
            return parsList;
        }

        private List<string> GetSemanticParameters(string query)
        {
            Regex semanticRegex = new Regex(semanticParametersPattern);

            if (!semanticRegex.IsMatch(query))
            {
                return null;
            }

            var semanticMatch = semanticRegex.Match(query);
            string gramPars = semanticMatch.Groups[1].Value;
            var parsList = gramPars.Split('|').ToList();

            return parsList;
        }

        private Tuple<int, int> GetRangeParameters(string query)
        {
            Regex rangeRegex = new Regex(rangeParametersPattern);

            if (!rangeRegex.IsMatch(query))
            {
                return null;
            }

            var rangeMatch = rangeRegex.Match(query);           
            int fromRange = int.Parse(rangeMatch.Groups[1].Value);
            int toRange = int.Parse(rangeMatch.Groups[2].Value); 

            return new Tuple<int, int>(fromRange, toRange);
        }
    }
}
