using Demo.Other;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Samples.WithBuilder
{
    public class WithAbuser
    {
        [Fact]
        void Test_SellCar()
        {
            ICar entity = new Car
            {
                CurrentOwner = new Owner
                {
                    Name = "Aleksandr Kugushev"
                }
            };
            ICar newValue = entity.With(e
                => e.CurrentOwner.Name = "Jon Skeet");

            Assert.Equal(
                "Aleksandr Kugushev",
                entity.CurrentOwner.Name);
        }
    }
}
