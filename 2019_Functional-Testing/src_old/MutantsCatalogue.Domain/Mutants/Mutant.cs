using System;

namespace MutantsCatalogue.Domain.Mutants
{
    public class Mutant
    {
        public string Name { get; set; }
        public string RealName { get; set; }
        public string Superpower { get; set; }

        public override string ToString() => $"{Name} ({RealName}): {Superpower}";
    }
}