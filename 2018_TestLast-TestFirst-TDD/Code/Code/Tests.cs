using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Code
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void TestDraw_Returns7Items()
        {
            // arrange
            var expert = new TheExpert();
            // act
            var items = expert.Draw();
            // assert
            Assert.AreEqual(7, items.Length);
        }

        [Test]
        public void TestDraw_ReturnsRed() { }


        [Test]
        public void TestDraw_ReturnsLines() { }


    }
}
