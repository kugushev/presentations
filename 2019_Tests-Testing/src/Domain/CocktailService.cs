using System;
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
    }
}
