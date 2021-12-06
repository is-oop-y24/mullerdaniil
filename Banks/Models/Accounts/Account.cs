using System;
using Banks.Models.Banks;
using Banks.Models.Transactions;
using Banks.Tools;

namespace Banks.Models.Accounts
{
    public abstract class Account
    {
        protected internal Account(Client client, BankInfo bankInfo)
        {
            Client = client;
            BankInfo = bankInfo;
            Id = Guid.NewGuid();
        }

        public Guid Id { get; }

        public decimal Balance { get; internal set; }

        protected Client Client { get; }
        protected BankInfo BankInfo { get; }
        protected decimal AccumulatedPercents { get; set; }

        public virtual Transaction Deposit(decimal amount)
        {
            var transaction = new DepositTransaction(this, amount);
            transaction.Execute();
            return transaction;
        }

        public virtual Transaction Withdraw(decimal amount)
        {
            if (!Client.IsSecure())
            {
                if (amount > BankInfo.InsecureAccountWithdrawalLimit)
                {
                    throw new BankException("Withdrawal error. The client is not secure. The amount is too big");
                }
            }

            if (amount > Balance)
            {
                throw new BankException("Withdrawal error. Not enough money on the account " + Id);
            }

            var transaction = new WithdrawalTransaction(this, amount);
            transaction.Execute();
            return transaction;
        }

        public virtual Transaction Transfer(decimal amount, Account account)
        {
            if (!Client.IsSecure())
            {
                if (amount > BankInfo.InsecureAccountWithdrawalLimit)
                {
                    throw new BankException("Transfer error. The client is not secure. The amount is too big");
                }
            }

            if (amount > Balance)
            {
                throw new BankException("Transfer error. Not enough money on the account " + Id);
            }

            if (Client.BankId != account.Client.BankId)
            {
                throw new BankException("Transfer error. The accounts are from different banks");
            }

            var transaction = new TransferTransaction(this, account, amount);
            transaction.Execute();
            return transaction;
        }

        internal virtual void AccumulatePercents(int assessmentCount)
        {
        }

        internal virtual void PayPercents(int assessmentCount)
        {
        }

        internal virtual void AssessCommission(int assessmentCount)
        {
        }

        internal void ChangeBalance(decimal amount)
        {
            Balance += amount;
        }
    }
}