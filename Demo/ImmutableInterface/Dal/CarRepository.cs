using Demo.ImmutableInterface.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Demo.ImmutableInterface.Dal
{
    class CarRepository : ICarRepository
    {
        private readonly CarContext carContext = new CarContext();

        public IReadOnlyList<ICar> Filter(Expression<Func<Car, bool>> isSatisfiedBy)
        {
            return carContext.Cars.Where(isSatisfiedBy).ToList();
        }
    }
}
