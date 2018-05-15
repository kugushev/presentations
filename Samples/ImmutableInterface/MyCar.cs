using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Xunit;

namespace Samples.ImmutableInterface
{
    public class ExampleSuperEntity
    {
        [Fact]
        public void Example()
        {
            var super = new MyCar
            {
                Id = 42,
                Children = new List<Bicycle>
                {
                   new Bicycle { Id = 1, Name = "Entity 1" },
                   new Bicycle { Id = 2, Name = "Entity 2" }
                }
            };

            ExternalMethod(super);
        }

        private void ExternalMethod(IMyCar entity)
        {
            
        }
    }

    public interface IMyCar
    {
        int Id { get; }

        IReadOnlyCollection<IBicycle> Children { get; }
    }

    public class MyCar : IMyCar
    {
        public int Id { get; set; }

        public List<Bicycle> Children { get; set; }

        IReadOnlyCollection<IBicycle> IMyCar.Children => Children;
    }
}
