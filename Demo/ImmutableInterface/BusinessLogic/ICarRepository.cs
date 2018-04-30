using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Demo.ImmutableInterface.BusinessLogic;

namespace Demo.ImmutableInterface.BusinessLogic
{
    interface ICarRepository
    {
        IReadOnlyList<ICar> Filter(Expression<Func<Car, bool>> isSatisfiedBy);
    }
}