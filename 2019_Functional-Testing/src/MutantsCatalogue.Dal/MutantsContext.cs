using Microsoft.EntityFrameworkCore;
using MutantsCatalogue.Domain.Mutants;

namespace MutantsCatalogue.Dal
{
    internal class MutantsContext: DbContext
    {
        public DbSet<Mutant> Mutants { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=mutants.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Mutant>().HasKey(m => m.Name);
        }
    }
}