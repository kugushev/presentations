using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public class Blender
    {
        private readonly List<Ingridient> ingridients = new List<Ingridient>();

        public void Add(Ingridient ingridient) => ingridients.Add(ingridient);

        public Ingridient Blend()
        {
            return new Ingridient
            {
                Amount = ingridients.Sum(x => x.Amount),
                IsAlcohol = ingridients.Any(x => x.IsAlcohol)
            };
        }
    }
}
