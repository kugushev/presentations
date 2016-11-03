using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleDomainLayer
{
    public interface IInvestmentsRepository
    {
        InvestmentDefaultProcessToken GetTokenForInvestmentDefaultProcess(InvestmentsProcessDirection direction,
             int count, bool includeEmpty, decimal maxAmount);
    }

    public enum InvestmentsProcessDirection
    {
        In,
        Out
    }

    public class InvestmentDefaultProcessToken
    {
        public int Id { get; set; }
        public bool IsValidUserNameAndCredentials { get; set; }
        public IReadOnlyList<decimal> Steps { get; set; }
    }
}
