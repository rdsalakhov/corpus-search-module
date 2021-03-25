using CorpusSearchModule.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorpusSearchModule.Core.Services
{
    public interface IQueryParser
    {
        public IntermediateQuery ParseQuery(string query);
    }
}
