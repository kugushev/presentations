using Doq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleDomainLayer.Sample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Doq.DynamicMock;

namespace SampleTests
{
    [TestClass]
    public class SampleTestsOld
    {
        [TestMethod]
        public void Sample_Property()
        {
            dynamic isolated = new DynamicMock();
            isolated.GetOnlyProperty = "Hello world";

            ISampleInterface result = isolated;

            Assert.AreEqual("Hello world", result.GetOnlyProperty);
        }

        [TestMethod]
        public void Sample_MethodWithGround()
        {
            dynamic isolated = new DynamicMock();
            isolated.Execute(_, _, _, _).Returns(42);

            ISampleInterface result = isolated;

            var actual = result.Execute(1, "", 10m, null);

            Assert.AreEqual(42, actual);
        }

        [TestMethod]
        public void Sample_MethodWithLambda()
        {
            dynamic isolated = new DynamicMock();
            isolated.Execute(_, _, If(a => a > 0m), _).Returns(42);
            isolated.Execute(_, _, If(a => a < 0m), _).Returns(18);

            ISampleInterface result = isolated;

            var actual1 = result.Execute(1, "", 10m, null);
            var actual2 = result.Execute(1, "", -5m, null);

            Assert.AreEqual(42, actual1);
            Assert.AreEqual(18, actual2);
        }

        [TestMethod]
        public void Sample_MethodWithOut()
        {
            dynamic isolated = new DynamicMock();
            isolated.TryDoSomething(_, 42).Returns(true);

            ISampleInterface result = isolated;

            int actualOut;
            var actual = result.TryDoSomething("subbotnik", out actualOut);

            Assert.AreEqual(true, actual);
            Assert.AreEqual(42, actualOut);
        }

        [TestMethod]
        public void Sample_MethodResultIsDto()
        {
            dynamic isolated = new DynamicMock();
            isolated.LoadSomeDtoWithVeryLongName().Returns(new { Id = 42, Name = "subbotnik" });

            ISampleInterface result = isolated;

            var actual = result.LoadSomeDtoWithVeryLongName();

            var expected = new SomeDtoWithVeryLongName { Id = 42, Name = "subbotnik" };
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Sample_MethodResultEnumerableDto()
        {
            dynamic isolated = new DynamicMock();
            isolated.LoadItems().Returns(new[] { new { Id = 1, Name = "1" }, new { Id = 2, Name = "2" } });

            ISampleInterface result = isolated;

            var actual = result.LoadItems();

            Assert.IsTrue(actual.SequenceEqual(new[]
            {
                new SomeDtoWithVeryLongName { Id = 1, Name = "1" },
                new SomeDtoWithVeryLongName { Id = 2, Name = "2" }
            }));
        }

        [TestMethod]
        public void Sample_MethodResultSimplifiedNames()
        {
            dynamic isolated = new DynamicMock();
            isolated.Load_().Returns(new { _Value = 156 });

            ISampleInterface result = isolated;

            var actual = result.LoadSomeDtoWithVeryLongName();

            var expected = new SomeDtoWithVeryLongName { VeryLongPropertyButAnywayItMeansValue = 156 };
            Assert.AreEqual(expected, actual);
        }
    }
}
