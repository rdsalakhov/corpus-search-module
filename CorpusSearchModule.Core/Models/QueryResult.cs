using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorpusSearchModule.Core.Models
{
    public class QueryResult
    {
        static int _cutOffset = 90;
        public string TextName { get; set; }
        public string Result { get; set; }

        public List<int> StartPositions { get; set; }
        public List<int> EndPositions { get; set; }

        public QueryResult()
        {
            StartPositions = new List<int>();
            EndPositions = new List<int>();
        }

        public QueryResult(string textName, string text, List<int> startPositions, List<int> endPositions)
        {
            this.TextName = textName;
            this.Result = text;
            this.StartPositions = startPositions;
            this.EndPositions = endPositions;
        }       
    }
}
