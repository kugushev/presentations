using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleDomainLayer.Sample
{
    public interface ISampleInterface
    {
        string GetOnlyProperty { get; }

        int Execute(int intgr, string str, decimal dcml, IDictionary<int, string> dictionary);

        int Execute(IDictionary<int, string> dictionary);

        bool TryDoSomething(string source, out int result);

        SomeDtoWithVeryLongName LoadSomeDtoWithVeryLongName();

        IEnumerable<SomeDtoWithVeryLongName> LoadItems();
    }

    public class SomeDtoWithVeryLongName
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double VeryLongPropertyButAnywayItMeansValue { get; set; }

        public override bool Equals(object obj)
        {
            var dto = obj as SomeDtoWithVeryLongName;
            return dto != null && dto.Id == Id && dto.Name == Name && dto.VeryLongPropertyButAnywayItMeansValue == VeryLongPropertyButAnywayItMeansValue;
        }
    }


}
