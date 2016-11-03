using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleDomainLayer;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

namespace SampleTests
{
    [TestClass]
    public class PlainOldUnitTest
    {

        [TestMethod]
        public void GetCashFlowTransactionsSumByFilter_DefaultFilter_ReturnsCashflowSum()
        {
            // arrange
            var securityService = new Mock<ISecurityService>();
            securityService.Setup(s => s.SecurityToken).Returns("TKN");

            var investmentsRepository = new Mock<IInvestmentsRepository>();
            investmentsRepository.Setup(s => s.GetTokenForInvestmentDefaultProcess(
                It.IsAny<InvestmentsProcessDirection>(), It.IsAny<int>(),
                It.IsAny<bool>(), It.IsAny<decimal>()))
                .Returns(new InvestmentDefaultProcessToken
                {
                    Id = 1,
                    IsValidUserNameAndCredentials = true
                });

            var accountingCriteriaFactory = new Mock<IAccountingCriteriaFactory>();
            accountingCriteriaFactory.Setup(f => f.CreateAccountingCriteria(
                It.IsAny<AccountingEntityType>(),
                It.IsAny<IReadOnlyList<decimal>>(),
                It.IsAny<InvestmentDefaultProcessToken>(),
                It.IsAny<string>()
                )).Returns(new[]
                {
                    new AccountingInvestmentValidationCriteria { RequestHashCode = 1 },
                    new AccountingInvestmentValidationCriteria { RequestHashCode = 2 },
                    new AccountingInvestmentValidationCriteria { RequestHashCode = 3 },
                });

            var accountingRepository = new Mock<IAccountingRepository>();
            accountingRepository.Setup(r => r.GetCashFlowByCriteria(
                It.Is<AccountingInvestmentValidationCriteria>(c => c.RequestHashCode == 1)
                )).Returns(10);
            accountingRepository.Setup(r => r.GetCashFlowByCriteria(
                It.Is<AccountingInvestmentValidationCriteria>(c => c.RequestHashCode == 2)
                )).Returns(20);
            accountingRepository.Setup(r => r.GetCashFlowByCriteria(
                It.Is<AccountingInvestmentValidationCriteria>(c => c.RequestHashCode == 3)
                )).Returns(30);

            var moneyValidator = new Mock<IMoneyValidator>();
            decimal delta = 1;
            moneyValidator.Setup(
                v => v.ValidateAndGetDelta(It.IsAny<IEnumerable<decimal>>(), out delta))
                .Returns(true);

            var cashflowManager = new CashFlowManager(securityService.Object,
                investmentsRepository.Object,
                accountingCriteriaFactory.Object,
                accountingRepository.Object,
                moneyValidator.Object,
                Mock.Of<IUnnecessaryDependency>());
            // act            
            var actual = cashflowManager.GetCashFlowTransactionsSumByFilter(new CashFlowTransactionsFilter());

            // assert
            Assert.AreEqual(61m, actual);
        }
    }
}
