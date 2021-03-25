using CorpusSearchModule.Core.Models;
using CorpusSearchModule.Core.Models.Exceptions;
using CorpusSearchModule.Core.Services.DriverInterfaces;
using CorpusSearchModule.Core.Services.TextCorpusSystemDriver;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorpusSearchModule.Core.Services
{
    public class DriverManager : IDriverManager
    {
        ConcurrentDictionary<string, IDriverFacade> drivers;

        public DriverManager()
        {
            drivers = new ConcurrentDictionary<string, IDriverFacade>();
            drivers.GetOrAdd("TextCorpusSystemDriver", new TextCorpusSystemFacade());
        }

        private IDriverFacade ResolveDriver(string driverName)
        {
            IDriverFacade driver = null;
            drivers.TryGetValue(driverName, out driver);
            return driver;
        }

        public List<string> GetAvailableDrivers()
        {
            return drivers.Keys.ToList();
        }

        public List<QueryResult> ExecuteQuery(IntermediateQuery query, string driverName)
        {
            var driver = ResolveDriver(driverName);
            if (driver == null)
            {
                throw new UnknownDriverException(driverName, "Unknown driver");
            }
            List<QueryResult> results;
            try
            {
                results = driver.ExecuteQuery(query);
            }
            catch (Exception e )
            {

                throw new Exception($"Driver failed to execute query: {e.Message}", e);
            }
            return results;
        }
    }
}
