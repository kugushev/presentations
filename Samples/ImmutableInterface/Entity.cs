using System;
using System.Collections.Generic;
using System.Text;

namespace Samples.ImmutableInterface
{
    public interface IEntity
    {
        int Id { get; }

        string Name { get; }

        IEntity With(Action<Entity> builder);
    }

    public class Entity : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEntity With(Action<Entity> builder)
        {
            var clone = (Entity)MemberwiseClone();
            builder(clone);
            return clone;
        }
    }
}
