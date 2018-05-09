using Demo.Other;
using System;
using System.Collections.Generic;
using System.Text;

namespace Samples.WithBuilder
{
    interface IEntity
    {
        IOwner CurrentOwner { get; }
        IEntity With(Action<Car> builder);
    }

    class Car : IEntity
    {
        IOwner IEntity.CurrentOwner => CurrentOwner;

        public Owner CurrentOwner { get; set; }
        
        public IEntity With(Action<Car> builder)
        {
            var clone = (Car)MemberwiseClone();
            builder(clone);
            return clone;
        }
    }
}
