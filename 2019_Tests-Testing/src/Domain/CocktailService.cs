using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public class CocktailService
    {
        private readonly IBarsProvider barsProvider;

        public CocktailService(IBarsProvider barsProvider)
        {
            this.barsProvider = barsProvider;
        }

        public Cocktail Mix(string name, out decimal? bestPrice)
        {
            var id = barsProvider.FindCocktailId(name);
            if (id != null)
            {
                var prices = barsProvider.FindPrices(id.Value);
                bestPrice = prices.Min();
            }
            else
                bestPrice = null;

            switch (name)
            {
                case "mojito":
                    var cocktail = new Cocktail
                    {
                        Name = name,
                        Size = 0.5,
                        PreparationTime = TimeSpan.FromMinutes(5),
                        TotalTime = TimeSpan.FromMinutes(5),
                        Instructions = "The same as usual mojito but without alcohol"
                    };
                    return cocktail;
                default:
                    return null;
            }
        }

        public decimal? FindBestPrice(string name, decimal? currentPrice)
        {
            var id = barsProvider.FindCocktailId(name);
            if (id != null || currentPrice == null)
            {
                var prices = barsProvider
                    .FindPrices(id != null ? id.Value : 0);
                currentPrice = prices.Min();
            }
            return currentPrice;
        }

        public Cocktail ManualMix(IReadOnlyCollection<Ingridient> ingridients)
        {
            var allowed = ingridients.Where(i => !i.IsAlcohol).ToList();

            int removedCount = ingridients.Count - allowed.Count;

            string name = $"Cocktail with {ingridients.First()}";

            return new Cocktail();
        }
    }
}
