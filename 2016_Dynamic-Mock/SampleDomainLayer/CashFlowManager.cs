using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleDomainLayer
{
    internal class CashFlowManager
    {
        private const decimal MaxAmount = 1000;
        private const int ExpectedCriteriaCount = 3;

        private readonly ISecurityService _securityService;
        private readonly IInvestmentsRepository _investmentsRepository;
        private readonly IAccountingCriteriaFactory _accountingCriteriaFactory;
        private readonly IAccountingRepository _accountingRepository;
        private readonly IMoneyValidator _moneyValidator;
        private readonly IUnnecessaryDependency _unnecessaryDependency;

        public CashFlowManager(ISecurityService securityService, IInvestmentsRepository investmentsRepository,
            IAccountingCriteriaFactory accountingCriteriaFactory, IAccountingRepository accountingRepository,
            IMoneyValidator moneyValidator, IUnnecessaryDependency unnecessaryDependency)
        {
            _securityService = securityService;
            _investmentsRepository = investmentsRepository;
            _accountingCriteriaFactory = accountingCriteriaFactory;
            _accountingRepository = accountingRepository;
            _moneyValidator = moneyValidator;
            _unnecessaryDependency = unnecessaryDependency;
        }

        public decimal GetCashFlowTransactionsSumByFilter(CashFlowTransactionsFilter filter)
        {
            string securityToken = _securityService.SecurityToken;
            InvestmentDefaultProcessToken investmentToken = _investmentsRepository
                .GetTokenForInvestmentDefaultProcess(InvestmentsProcessDirection.In, filter.MaxTokens, false, MaxAmount);

            if (!investmentToken.IsValidUserNameAndCredentials)
                throw new InvalidOperationException();

            IEnumerable<AccountingInvestmentValidationCriteria> criteria =
                _accountingCriteriaFactory.CreateAccountingCriteria(AccountingEntityType.Cashflow, investmentToken.Steps, investmentToken, securityToken);

            if (criteria.Count() != ExpectedCriteriaCount)
                throw new InvalidOperationException();

            if (criteria.Any(c => c.RequestHashCode <= 0))
                throw new InvalidOperationException();

            var cashflow = criteria.Select(_accountingRepository.GetCashFlowByCriteria).ToList();

            decimal delta;
            _moneyValidator.ValidateAndGetDelta(cashflow, out delta);

            return cashflow.Sum() + delta;
        }
    }
}
