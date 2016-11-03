using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doq
{
    public class MethodData
    {
        internal string Name { get; set; }
        internal object[] Args { get; set; }
        internal object ReturnedValue { get; private set; }
        public void Returns(object value) => ReturnedValue = value;
    }
}
