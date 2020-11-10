namespace SearchApp.Web.Models
{
    public class SearchRequestDto
    {
        public string Industry { get; set; }
        public int Headquarters { get; set; }
        public string Geography { get; set; }
        public decimal Revenue { get; set; }
        public decimal Income { get; set; }
        public decimal EBTDA { get; set; }
        public string Owner { get; set; }
        public int InvestmentId { get; set; }
        public string Partner { get; set; }
        public decimal IRR { get; set; }
    }

    class MyClass
    {
        public MyClass()
        {
            new SearchRequestDto
            {
                Industry = default,
                Headquarters = default,
                Geography = default,
                Revenue = default,
                Income = default,
                EBTDA = default,
                Owner = default,
                InvestmentId = default,
                Partner = default,
                IRR = default,
            };
        }
    }
}