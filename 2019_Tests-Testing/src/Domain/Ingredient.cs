using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Ingredient
    {
        public bool IsAlcohol { get; set; }
        public double Amount { get; set; }

        public Cocktail ToCocktail(string name) => new Cocktail { Name = name, Size = Amount };
    }
}
