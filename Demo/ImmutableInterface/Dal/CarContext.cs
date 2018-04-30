using Demo.ImmutableInterface.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace Demo.ImmutableInterface.Dal
{
    internal class CarContext : DbContext
    {
        internal DbSet<Car> Cars { get; set; }
    }
}
