using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleDomainLayer
{
    public interface IAccountingCriteriaFactory
    {
        IEnumerable<AccountingInvestmentValidationCriteria> CreateAccountingCriteria(AccountingEntityType type, IReadOnlyList<decimal> amountSteps,
            InvestmentDefaultProcessToken investmentTokens, string securityToken);
    }

    public enum AccountingEntityType
    {
        Cashflow
    }



}
