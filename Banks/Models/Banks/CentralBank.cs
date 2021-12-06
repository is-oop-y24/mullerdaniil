using System;
using System.Collections.Generic;
using Banks.Models.Accounts;
using Banks.Models.Transactions;
using Banks.Tools;

namespace Banks.Models.Banks
{
    public class CentralBank
    {
        private readonly List<Bank> _banks = new List<Bank>();
        private readonly DateTime _dateTime = DateTime.Today;

        public Bank CreateBank(
            decimal debitPercentOfAnnual,
            Func<decimal, decimal> depositPercents,
            decimal creditCommission,
            decimal creditLimit,
            decimal insecureAccountWithdrawalLimit)
        {
            var bank = new Bank(
                debitPercentOfAnnual,
                depositPercents,
                creditCommission,
                creditLimit,
                insecureAccountWithdrawalLimit);
            _banks.Add(bank);
            return bank;
        }

        public Transaction Transfer(Client clientFrom, Account accountFrom, Client clientTo, Account accountTo, decimal amount)
        {
            if (!clientFrom.IsSecure() || !clientTo.IsSecure())
            {
                if (amount > FindBankById(clientFrom.BankId).InsecureAccountWithdrawalLimit)
                {
                    throw new BankException("Transfer error. Client is not secure. The amount is too big");
                }
            }

            if (amount > accountFrom.Balance)
            {
                throw new BankException("Transfer error. Not enough money on the account " + accountFrom);
            }

            var transaction = new TransferTransaction(accountFrom, accountTo, amount);
            transaction.Execute();
            return transaction;
        }

        public Bank FindBankById(Guid id)
        {
            return _banks.Find(bank => bank.Id == id);
        }

        public void AddDays(int daysCount)
        {
            DateTime updatedDateTime = _dateTime.AddDays(daysCount);
            for (DateTime currentDateTime = _dateTime;
                DateTime.Compare(currentDateTime, updatedDateTime) < 0;
                currentDateTime = currentDateTime.AddDays(1))
            {
                DateTime lastDayOfMonth = new DateTime(_dateTime.Year, _dateTime.Month, 1).AddMonths(1).AddDays(-1);
                _banks.ForEach(bank => bank.AccumulatePercents(1));
                _banks.ForEach(bank => bank.AssessCommission(1));
                if (currentDateTime.Date == lastDayOfMonth.Date)
                {
                    _banks.ForEach(bank => bank.PayPercents(1));
                }
            }
        }
    }
}