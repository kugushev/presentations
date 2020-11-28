using System;
using System.Collections.Generic;
using System.Linq;

namespace SearchApp.Web.Models
{
    public class SearchRequestDto : ISearchRequest,
        ISearchFinanceRequest, ISearchInvestmentRequest
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

    class SearchRefinementsDto: ISearchRefinements
    {
        public List<RefinementDto> Refinements
        {
            get; set;
        }

        IReadOnlyList<IRefinement> ISearchRefinements.Refinements => Refinements;
    }

    class RefinementDto: IRefinement
    {
        public string Name { get; set; }
        public int Amount { get; set; }
    }

    interface ISearchRefinements
    {
        IReadOnlyList<IRefinement> Refinements { get; }
    }

    interface IRefinement
    {
        string Name { get; }
        int Amount { get; }
    }

    public class SearchFinanceRequest
    {
        public decimal Revenue { get; set; }
        public decimal Income { get; set; }
        public decimal EBTDA { get; set; }
    }

    public class SearchInvestmentRequest
    {
        public int InvestmentId { get; set; }
        public string Partner { get; set; }
        public decimal IRR { get; set; }
    }

    public interface ISearchFinanceRequest
    {
        public decimal Revenue { get; set; }
        public decimal Income { get; set; }
        public decimal EBTDA { get; set; }
    }

    public interface ISearchInvestmentRequest
    {
        public int InvestmentId { get; set; }
        public string Partner { get; set; }
        public decimal IRR { get; set; }
    }


    interface ISearchBuilder
    {
        void SearchFinance(SearchFinanceRequest request);
        void SearchInvestment(SearchInvestmentRequest request);
        void SearchFinance(ISearchFinanceRequest request);
        void SearchInvestment(ISearchInvestmentRequest request);
    }

    public class SearchRequestEntity
    {
        public SearchRequestEntity(string industry, int headquarters, string geography, decimal revenue, decimal income,
            decimal ebtda, string owner, int investmentId, string partner, decimal irr)
        {
            Industry = industry;
            Headquarters = headquarters;
            Geography = geography;
            Revenue = revenue;
            Income = income;
            EBTDA = ebtda;
            Owner = owner;
            InvestmentId = investmentId;
            Partner = partner;
            IRR = irr;
        }

        public SearchRequestEntity(SearchRequestDto dto)
        {
            Industry = dto.Industry;
            Headquarters = dto.Headquarters;
            Geography = dto.Geography;
            Revenue = dto.Revenue;
            Income = dto.Income;
            EBTDA = dto.EBTDA;
            Owner = dto.Owner;
            InvestmentId = dto.InvestmentId;
            Partner = dto.Partner;
            IRR = dto.IRR;
        }

        public string Industry { get; }
        public int Headquarters { get; }
        public string Geography { get; }
        public decimal Revenue { get; }
        public decimal Income { get; }
        public decimal EBTDA { get; }
        public string Owner { get; }
        public int InvestmentId { get; }
        public string Partner { get; }
        public decimal IRR { get; }
    }

    public interface ISearchRequest
    {
        public string Industry { get; }
        public int Headquarters { get; }
        public string Geography { get; }
        public decimal Revenue { get; }
        public decimal Income { get; }
        public decimal EBTDA { get; }
        public string Owner { get; }
        public int InvestmentId { get; }
        public string Partner { get; }
        public decimal IRR { get; }
    }

    class MyClass
    {
        public MyClass()
        {
            ISearchRequest e = new SearchRequestDto
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
            Console.WriteLine(e.Industry);
            Console.WriteLine(e.Headquarters);
            Console.WriteLine(e.Geography);
            Console.WriteLine(e.Revenue);
            Console.WriteLine(e.Income);
            Console.WriteLine(e.EBTDA);
            Console.WriteLine(e.Owner);
            Console.WriteLine(e.InvestmentId);
            Console.WriteLine(e.Partner);
            Console.WriteLine(e.IRR);

            new SearchFinanceRequest
            {
                Income = default,
                Revenue = default,
                EBTDA = default
            };

            new SearchInvestmentRequest
            {
                Partner = default,
                InvestmentId = default,
                IRR = default
            };
            Search(null, null, null);
            Search(null, null);

            ISearchRefinements a = new SearchRefinementsDto
            {
                Refinements = new List<RefinementDto>
                {
                    new RefinementDto
                    {
                        Amount = default,
                        Name = default
                    }
                }
            };

            Console.WriteLine(a.Refinements.Select(s => (s.Amount, s.Name)));
        }

        void Search(ISearchBuilder searchBuilder, IMapper mapper, SearchRequestDto request)
        {
            searchBuilder.SearchFinance(mapper.Map<SearchFinanceRequest>(request));
            searchBuilder.SearchInvestment(mapper.Map<SearchInvestmentRequest>(request));
        }

        void Search(ISearchBuilder searchBuilder, SearchRequestDto request)
        {
            searchBuilder.SearchFinance(request);
            searchBuilder.SearchInvestment(request);
        }
    }

    interface IMapper
    {
        T Map<T>(object obj);
    }
}