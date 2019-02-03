using Demo.Other;
using System;
using System.Collections.Generic;
using System.Text;

namespace Samples.WithBuilder
{
    interface ICar
    {
        IOwner CurrentOwner { get; }
        string Model { get; }
        ICar With(Action<Car> builder);
    }

    class Car : ICar
    {
        IOwner ICar.CurrentOwner => CurrentOwner;

        public string Model { get; set; }

        public Owner CurrentOwner { get; set; }

        public ICar With(Action<Car> builder)
        {
            var clone = (Car)MemberwiseClone();
            builder(clone);
            return clone;
        }

        ICar ChangeModel(ICar car)
        {
            ICar newCar = car.With(
                c => c.Model = "Hyundai Solaris");
            return newCar;
        }
    }
}
