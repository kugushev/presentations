using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleDomainLayer
{
    public interface IMoneyValidator
    {
        bool ValidateAndGetDelta(IEnumerable<decimal> cashflow, out decimal delta);
    }
}
