using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleDomainLayer
{
    public class AccountingInvestmentValidationCriteria
    {
        public decimal Amount { get; set; }
        public Guid RequestId { get; set; }
        public int RequestHashCode { get; set; }
        public Mark CashMark { get; set; }
    }

    public class Mark
    {
        public Mark(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }

}
