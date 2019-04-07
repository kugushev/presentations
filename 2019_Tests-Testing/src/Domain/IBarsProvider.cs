using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public interface IBarsProvider
    {
        int? FindCocktailId(string cocktailName);
        IReadOnlyCollection<decimal> FindPrices(int cocktailId);
    }
}
