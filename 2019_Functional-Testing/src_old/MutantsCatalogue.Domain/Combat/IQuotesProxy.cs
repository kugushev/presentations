using System.Threading.Tasks;

namespace MutantsCatalogue.Domain.Combat
{
    public interface IQuotesProxy
    {
        Task<string> GetQuoteAsync(string category);
    }
}