using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public class Blender
    {
        private readonly List<Ingredient> ingridients = new List<Ingredient>();

        public void Add(Ingredient ingridient) => ingridients.Add(ingridient);

        public Ingredient Blend()
        {
            return new Ingredient
            {
                Amount = ingridients.Sum(x => x.Amount),
                IsAlcohol = ingridients.Any(x => x.IsAlcohol)
            };
        }
    }
}
