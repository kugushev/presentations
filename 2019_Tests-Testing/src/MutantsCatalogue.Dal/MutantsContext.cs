using Microsoft.EntityFrameworkCore;
using MutantsCatalogue.Domain.Mutants;

namespace MutantsCatalogue.Dal
{
    public class MutantsContext : DbContext
    {
        public MutantsContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Mutant> Mutants { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Mutant>().HasKey(m => m.Name);
        }
    }
}