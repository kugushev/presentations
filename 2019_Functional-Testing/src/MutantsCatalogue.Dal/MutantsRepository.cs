using System;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using MutantsCatalogue.Domain.Mutants;

namespace MutantsCatalogue.Dal
{
    internal class MutantsRepository: IMutantsRepository
    {
        private readonly MutantsContext context;

        public MutantsRepository(MutantsContext context)
        {
            this.context = context;
        }
        
        public Mutant Get(string name)
        {
            return context.Mutants.Find(name);
        }

        public void Add(Mutant mutant)
        {
            context.Mutants.Add(mutant);
            context.SaveChanges();
        }
    }
}
