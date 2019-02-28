using MutantsCatalogue.Domain;
using System;
using Xunit;

namespace MutantsCatalogue.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var cls = new Class1();

            var result = cls.Test(3, 4, 0);

            Assert.Equal(7, result);
        }
    }
}
