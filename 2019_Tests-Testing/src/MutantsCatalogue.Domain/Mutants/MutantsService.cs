namespace MutantsCatalogue.Domain.Mutants
{
    internal class MutantsService: IMutantsService
    {
        private readonly IMutantsRepository repository;

        public MutantsService(IMutantsRepository repository)
        {
            this.repository = repository;
        }

        public Mutant Retrieve(string name) => repository.Get(name);

        public void Add(Mutant mutant) => repository.Add(mutant);
    }
}