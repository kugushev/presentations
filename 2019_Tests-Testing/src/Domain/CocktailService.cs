using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public class CocktailService
    {
        private readonly IBarsProvider barsProvider;

        public CocktailService(IBarsProvider barsProvider) => this.barsProvider = barsProvider;

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

        public Cocktail ManualMix(IReadOnlyCollection<Ingredient> ingredient)
        {
            var allowed = ingredient.Where(i => !i.IsAlcohol).ToList();

            int removedCount = ingredient.Count - allowed.Count;

            string name = $"Cocktail with {ingredient.First()}";

            return new Cocktail();
        }

        public Cocktail MixMilkShake(Ingredient milk, Ingredient iceCream)
        {
            var blender = new Blender();
            blender.Add(milk);
            blender.Add(iceCream);
            var blended = blender.Blend();
            ThrowIfAlcohol(blended);
            return blended.ToCocktail("Milkshake");
        }

        private void ThrowIfAlcohol(Ingredient ingredient)
        {
            if (ingredient.IsAlcohol)
                throw new Exception("No Alcohol allowed");
        }
    }
}
