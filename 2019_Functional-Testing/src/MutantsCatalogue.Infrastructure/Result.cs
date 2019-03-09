namespace MutantsCatalogue.Infrastructure
{
    internal class Result
    {
        public ContentsDto Contents { get; set; }
    }

    internal class ContentsDto
    {
        public QuoteDto[] Quotes { get; set; }
    }

    internal class QuoteDto
    {
        public string Quote { get; set; }
        public string Category { get; set; }
    }
}