using System;
using System.Collections.Generic;
using System.Text;

namespace Samples.WithBuilder
{
    interface IEntity
    {
        AnotherEntity Agg { get; }
        IEntity With(Action<Entity> builder);
    }

    class Entity : IEntity
    {
        public AnotherEntity Agg { get; set; }

        public IEntity With(Action<Entity> builder)
        {
            var clone = (Entity)MemberwiseClone();
            builder(clone);
            return clone;
        }
    }
}
