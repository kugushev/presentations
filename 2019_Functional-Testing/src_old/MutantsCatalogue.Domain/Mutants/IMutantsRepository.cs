namespace MutantsCatalogue.Domain.Mutants
{
    public interface IMutantsRepository
    {
        Mutant Get(string name);
        void Add(Mutant mutant);
    }
}