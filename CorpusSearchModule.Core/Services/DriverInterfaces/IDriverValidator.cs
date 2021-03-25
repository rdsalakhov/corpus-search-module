using CorpusSearchModule.Core.Models;

namespace CorpusSearchModule.Core.Services.DriverInterfaces
{
    public interface IDriverValidator
    {
        public bool ValidateQuery(IntermediateQuery query, out string message);
    }
}