using System;
using System.Collections.Generic;
using Banks.Models.Accounts;

namespace Banks.Models.Banks
{
    public class Bank
    {
        private const string NotifyMessageTemplate = "{0} has changed the value from {1} to {2}.";
        private readonly BankInfo _bankInfo = new BankInfo();
        private readonly List<Client> _registeredOnNotificationsClients = new List<Client>();
        private readonly List<Account> _accounts = new List<Account>();

        internal Bank(
            decimal debitPercentOfAnnual,
            Func<decimal, decimal> depositPercents,
            decimal creditCommission,
            decimal creditLimit,
            decimal insecureAccountWithdrawalLimit)
        {
            DebitPercentOfAnnual = debitPercentOfAnnual;
            DepositPercents = depositPercents;
            CreditCommission = creditCommission;
            CreditLimit = creditLimit;
            InsecureAccountWithdrawalLimit = insecureAccountWithdrawalLimit;
        }

        public Guid Id { get; } = Guid.NewGuid();
        public decimal DebitPercentOfAnnual
        {
            get => _bankInfo.DebitPercentOfAnnual;
            set
            {
                NotifyClients(
                    string.Format(NotifyMessageTemplate, "Debit percent of annual", _bankInfo.DebitPercentOfAnnual, value),
                    client => client.HasDebitAccount);
                _bankInfo.DebitPercentOfAnnual = value;
            }
        }

        public Func<decimal, decimal> DepositPercents
        {
            get => _bankInfo.DepositPercents;
            set
            {
                NotifyClients(
                    string.Format(NotifyMessageTemplate, "Deposit percents", _bankInfo.DepositPercents, value),
                    client => client.HasDepositAccount);
                _bankInfo.DepositPercents = value;
            }
        }

        public decimal CreditCommission
        {
            get => _bankInfo.CreditCommission;
            set
            {
                NotifyClients(
                    string.Format(NotifyMessageTemplate, "Credit commission", _bankInfo.CreditCommission, value),
                    client => client.HasCreditAccount);
                _bankInfo.CreditCommission = value;
            }
        }

        public decimal CreditLimit
        {
            get => _bankInfo.CreditLimit;
            set
            {
                NotifyClients(
                    string.Format(NotifyMessageTemplate, "Credit limit", _bankInfo.CreditLimit, value),
                    client => client.HasCreditAccount);
                _bankInfo.CreditLimit = value;
            }
        }

        public decimal InsecureAccountWithdrawalLimit
        {
            get => _bankInfo.InsecureAccountWithdrawalLimit;
            set
            {
                NotifyClients(
                    string.Format(NotifyMessageTemplate, "Insecure account withdrawal limit", _bankInfo.InsecureAccountWithdrawalLimit, value),
                    client => client.HasDebitAccount);
                _bankInfo.InsecureAccountWithdrawalLimit = value;
            }
        }

        public Client CreateClient(string name, string surname)
        {
            return new Client(name, surname, Id);
        }

        public Account CreateDebitAccount(Client client)
        {
            client.HasDebitAccount = true;
            Account account = new DebitAccount(client, _bankInfo);
            _accounts.Add(account);
            return account;
        }

        public Account CreateDepositAccount(Client client, int depositDuration, decimal initialBalance)
        {
            client.HasDepositAccount = true;
            Account account = new DepositAccount(client, _bankInfo, depositDuration, initialBalance);
            _accounts.Add(account);
            return account;
        }

        public Account CreateCreditAccount(Client client)
        {
            client.HasCreditAccount = true;
            Account account = new CreditAccount(client, _bankInfo);
            _accounts.Add(account);
            return account;
        }

        public void RegisterClientOnNotification(Client client)
        {
            if (!_registeredOnNotificationsClients.Contains(client))
            {
                _registeredOnNotificationsClients.Add(client);
            }
        }

        public Account FindAccountById(Guid id)
        {
            return _accounts.Find(account => account.Id == id);
        }

        internal virtual void AccumulatePercents(int assessmentCount)
        {
            _accounts.ForEach(account => account.AccumulatePercents(assessmentCount));
        }

        internal virtual void PayPercents(int assessmentCount)
        {
            _accounts.ForEach(account => account.PayPercents(assessmentCount));
        }

        internal virtual void AssessCommission(int assessmentCount)
        {
            _accounts.ForEach(account => account.AssessCommission(assessmentCount));
        }

        private void NotifyClients(string message, Predicate<Client> predicate)
        {
            _registeredOnNotificationsClients.FindAll(predicate).ForEach(client => client.Notify(this, message));
        }
    }
}