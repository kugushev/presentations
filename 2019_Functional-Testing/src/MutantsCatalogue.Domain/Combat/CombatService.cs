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
        private readonly DomainSettings settings;

        public CombatService(IQuotesProxy quotesProxy, DomainSettings settings)
        {
            this.quotesProxy = quotesProxy;
            this.settings = settings;
        }

        public async Task<CombatResult> ExecuteCombatAsync(IReadOnlyCollection<string> mutants, bool epic)
        {
            var result = new CombatResult
            {
                Copyright = $"{settings.Copyright} {settings.CopyrightYear}"
            };
            if (mutants.Contains(Wolverine) && mutants.Contains(Magneto))
                result.Winner = Magneto;
            else if (mutants.Contains(Wolverine))
                result.Winner = Wolverine;
            else
                result.Winner = "Unknown";

            if (epic)
            {
                var category = result.Winner == Wolverine ? "life" : null;
                result.VictoryPhrase = await quotesProxy.GetQuoteAsync(category);
            }
            
            return result;
        }
    }
}