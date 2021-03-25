using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CorpusSearchModule.Models
{
    public class SavedQuery
    {
        public static List<SavedQuery> queries = new List<SavedQuery>();

        public DateTime CreatedAt { get; set; }

        public string Query { get; set; }
    }
}