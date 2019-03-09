using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MutantsCatalogue.Domain.Combat
{
    internal class CombatService : ICombatService
    {
        private const string Wolverine = "Wolverine";
        private const string Magneto = "Magneto";
        
        private readonly IQuotesProxy quotesProxy;

        public CombatService(IQuotesProxy quotesProxy)
        {
            this.quotesProxy = quotesProxy;
        }

        public async Task<CombatResult> ExecuteCombatAsync(IReadOnlyCollection<string> mutants, bool epic)
        {
            var result = new CombatResult();
            if (mutants.Contains(Wolverine) && mutants.Contains(Magneto))
                result.Winner = Magneto;
            else if (mutants.Contains(Wolverine))
                result.Winner = Wolverine;
            else
                return new CombatResult { Winner = "Unknown" };

            if (epic)
            {
                var category = result.Winner == Wolverine ? "life" : null;
                result.VictoryPhrase = await quotesProxy.GetQuoteAsync(category);
            }
            
            return result;
        }
    }
}