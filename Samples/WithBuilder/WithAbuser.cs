using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Samples.WithBuilder
{
    class WithAbuser
    {
        void DoSomething()
        {
            IEntity entity = new Entity
            {
                Agg = new AnotherEntity
                {
                    Name = "old value"
                }
            };            
            IEntity newValue = entity
                .With(e => e.Agg.Name = "new value");
            Assert.Equal("old value", entity.Agg.Name);
        }
    }
}
