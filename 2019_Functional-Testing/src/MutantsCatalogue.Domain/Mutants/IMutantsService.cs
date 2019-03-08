namespace MutantsCatalogue.Domain.Mutants
{
    public interface IMutantsService
    {
        Mutant Retrieve(string name);
        void Add(Mutant mutant);
    }
}