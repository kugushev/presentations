using System.Collections.Generic;
using System.Threading.Tasks;

namespace MutantsCatalogue.Domain.Combat
{
    public interface ICombatService
    {
        Task<CombatResult>  ExecuteCombatAsync(IReadOnlyCollection<string> mutants, bool epic);
    }
}