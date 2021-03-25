using CorpusSearchModule.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorpusSearchModule.Core.Services.DriverInterfaces
{
    public interface IDriverFacade
    {
        public string DriverName { get; }

        public List<QueryResult> ExecuteQuery(IntermediateQuery intQuery);
    }
}
