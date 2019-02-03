using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleDomainLayer;
using Doq;
using static Doq.DynamicMock;

namespace SampleTests
{
    [TestClass]
    public class NewModernUnitTest
    {
        [TestMethod]
        public void GetCashFlowTransactionsSumByFilter_DefaultFilter_ReturnsCashflowSum()
        {
            var agent = new DynamicTestAgent<CashFlowManager>();
            // arrange            
            agent.Arrange.SecurityToken = "TKN";
            agent.Arrange.GetToken_(_, _, _, _).Returns(new { Id = 1, IsValid_ = true });
            agent.Arrange.CreateAccountingCriteria(_, _, _, _).Returns(new[] 
            { new { _HashCode = 1 }, new { _HashCode = 2 }, new { _HashCode = 3 }, });
            agent.Arrange.GetCashFlow_(If(c => c.RequestHashCode == 1)).Returns(10m);
            agent.Arrange.GetCashFlow_(If(c => c.RequestHashCode == 2)).Returns(20m);
            agent.Arrange.GetCashFlow_(If(c => c.RequestHashCode == 3)).Returns(30m);
            agent.Arrange.ValidateAndGetDelta(_, 1m).Returns(true);

            // act            
            var actual = agent.Act.GetCashFlowTransactionsSumByFilter(
                new CashFlowTransactionsFilter());

            // assert
            Assert.AreEqual(61m, actual);
        }

    }
}
