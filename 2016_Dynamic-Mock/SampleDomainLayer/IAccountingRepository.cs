using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleDomainLayer
{
    public interface IAccountingRepository
    {
        decimal GetCashFlowByCriteria(AccountingInvestmentValidationCriteria criteria);
    }
}
