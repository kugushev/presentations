using Doq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleDomainLayer;
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
    public class SampleTests
    {
        [TestMethod]
        public void Sample_Property()
        {
            dynamic securityService = new DynamicMock();
            securityService.SecurityToken = "TKN";

            ISecurityService result = securityService;
            Assert.AreEqual("TKN", result.SecurityToken);
        }

        [TestMethod]
        public void Sample_MethodWithGround()
        {
            dynamic investmentsRepository = new DynamicMock();
            investmentsRepository.GetTokenForInvestmentDefaultProcess(_, _, _, _)
                .Returns(new InvestmentDefaultProcessToken
                {
                    Id = 1,
                    IsValidUserNameAndCredentials = true
                });

            IInvestmentsRepository result = investmentsRepository;
            var actual = result.GetTokenForInvestmentDefaultProcess(InvestmentsProcessDirection.In, 11, true, 100m);

            Assert.AreEqual(1, actual.Id);
            Assert.IsTrue(actual.IsValidUserNameAndCredentials);
        }

        [TestMethod]
        public void Sample_MethodWithLambda()
        {
            dynamic accountingRepository = new DynamicMock();
            accountingRepository.GetCashFlowByCriteria(If(c => c.RequestHashCode == 1)).Returns(10m);
            accountingRepository.GetCashFlowByCriteria(If(c => c.RequestHashCode == 2)).Returns(20m);
            accountingRepository.GetCashFlowByCriteria(If(c => c.RequestHashCode == 3)).Returns(30m);

            IAccountingRepository result = accountingRepository;

            var actual1 = result.GetCashFlowByCriteria(new AccountingInvestmentValidationCriteria { RequestHashCode = 1 });
            var actual2 = result.GetCashFlowByCriteria(new AccountingInvestmentValidationCriteria { RequestHashCode = 2 });
            var actual3 = result.GetCashFlowByCriteria(new AccountingInvestmentValidationCriteria { RequestHashCode = 3 });

            Assert.AreEqual(10, actual1);
            Assert.AreEqual(20, actual2);
            Assert.AreEqual(30, actual3);
        }

        [TestMethod]
        public void Sample_MethodWithOut()
        {
            dynamic moneyValidator = new DynamicMock();
            moneyValidator.ValidateAndGetDelta(_, 1m).Returns(true);

            IMoneyValidator result = moneyValidator;

            decimal actualOut;
            var actual = result.ValidateAndGetDelta(new[] { 150m }, out actualOut);

            Assert.AreEqual(true, actual);
            Assert.AreEqual(1m, actualOut);
        }

        [TestMethod]
        public void Sample_MethodResultIsDto()
        {
            dynamic investmentsRepository = new DynamicMock();
            investmentsRepository.GetTokenForInvestmentDefaultProcess(_, _, _, _)
                .Returns(new { Id = 1, IsValidUserNameAndCredentials = true });

            IInvestmentsRepository result = investmentsRepository;

            var actual = result.GetTokenForInvestmentDefaultProcess(InvestmentsProcessDirection.In, 11, true, 100m);

            Assert.AreEqual(1, actual.Id);
            Assert.IsTrue(actual.IsValidUserNameAndCredentials);
            Assert.IsNull(actual.Steps);
        }

        [TestMethod]
        public void Sample_MethodResultEnumerableDto()
        {
            dynamic accountingCriteriaFactory = new DynamicMock();
            accountingCriteriaFactory.CreateAccountingCriteria(_, _, _, _).Returns(new[]
            { new { RequestHashCode = 1 }, new { RequestHashCode = 2 }, new { RequestHashCode = 3 } });

            IAccountingCriteriaFactory result = accountingCriteriaFactory;

            var actual = result.CreateAccountingCriteria(AccountingEntityType.Cashflow, new decimal[0], null, "");

            CollectionAssert.AreEquivalent(new[] { 1, 2, 3 }, actual.Select(c => c.RequestHashCode).ToList());
        }

        [TestMethod]
        public void Sample_MethodResultSimplifiedNames1()
        {
            dynamic investmentsRepository = new DynamicMock();
            investmentsRepository.GetToken_(_, _, _, _).Returns(new { Id = 1, IsValid_ = true });

            IInvestmentsRepository result = investmentsRepository;

            var actual = result.GetTokenForInvestmentDefaultProcess(InvestmentsProcessDirection.In, 11, true, 100m);

            Assert.AreEqual(1, actual.Id);
            Assert.IsTrue(actual.IsValidUserNameAndCredentials);
            Assert.IsNull(actual.Steps);
        }

        [TestMethod]
        public void Sample_MethodResultSimplifiedNames2()
        {
            dynamic accountingCriteriaFactory = new DynamicMock();
            accountingCriteriaFactory.Create_Criteria(_, _, _, _)
                .Returns(new[] { new { _Code = 1 }, new { _Code = 2 }, new { _Code = 3 } });

            IAccountingCriteriaFactory result = accountingCriteriaFactory;

            var actual = result.CreateAccountingCriteria(AccountingEntityType.Cashflow, new decimal[0], null, "");

            CollectionAssert.AreEquivalent(new[] { 1, 2, 3 }, actual.Select(c => c.RequestHashCode).ToList());
        }
    }
}
