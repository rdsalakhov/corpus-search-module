using CorpusSearchModule.Core.Models;
using CorpusSearchModule.Core.Services.DriverInterfaces;
using CorpusSearchModule.Core.Services.TextCorpusSystemDriver.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorpusSearchModule.Core.Services.TextCorpusSystemDriver
{
    public class TextCorpusSystemFacade : IDriverFacade
    {
        TextCorpusSystemValidator validator = new TextCorpusSystemValidator();
        TextCorpusClient client = new TextCorpusClient();
        QueryTranslator translator = new QueryTranslator();

        public string DriverName { get => "TextCorpusSystemDriver"; }

        public List<QueryResult> ExecuteQuery(IntermediateQuery intQuery)
        {          
            if (!validator.ValidateQuery(intQuery, out string message))
            {
                throw new Exception($"Unsupported query: {message}");
            }
            Query query = translator.TranslateQuery(intQuery);
            return client.SendQuery(query);
        }
    }
}
