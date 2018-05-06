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
            var super = new SuperEntityImpl
            {
                Id = 42,
                Children = new List<Entity>
                {
                   new Entity { Id = 1, Name = "Entity 1" },
                   new Entity { Id = 2, Name = "Entity 2" }
                }
            };

            ExternalMethod(super);
        }

        private void ExternalMethod(ISuperEntity entity)
        {
            
        }
    }

    public interface ISuperEntity
    {
        int Id { get; }

        IReadOnlyCollection<IEntity> Children { get; }
    }

    public class SuperEntityImpl : ISuperEntity
    {
        public int Id { get; set; }

        public List<Entity> Children { get; set; }

        IReadOnlyCollection<IEntity> ISuperEntity.Children => Children;
    }
}
