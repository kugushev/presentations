using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleDomainLayer
{
    class CashFlowTransactionsFilter
    {
        public int MaxTokens { get; internal set; }
        public Guid RequestId { get; internal set; }
    }
}
