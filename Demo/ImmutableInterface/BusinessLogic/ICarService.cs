using Demo.Other;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.ImmutableInterface.BusinessLogic
{
    public interface ICarService
    {
        void RegisterCar(ICar car);
        bool UnregisterCar(ICar car);
        IOwner SellCar(ICar car);
        ICar BuyCar(ICar car);
        ICar RepairCar(ICar car);
        ICar UtilizeCar(ICar car);
    }
}
