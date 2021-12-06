using System;
using Banks.Models.Banks;
using Banks.Models.Transactions;
using Banks.Tools;

namespace Banks.Models.Accounts
{
    public class DepositAccount : Account
    {
        private int _depositRemainTime;

        internal DepositAccount(Client client, BankInfo bankInfo, int depositRemainTime, decimal initialBalance)
            : base(client, bankInfo)
        {
            _depositRemainTime = depositRemainTime;
            Balance = initialBalance;
        }

        public override Transaction Withdraw(decimal amount)
        {
            if (_depositRemainTime > 0)
            {
                throw new BankException("Withdrawal error. The deposit count is yet closed");
            }

            return base.Withdraw(amount);
        }

        public override Transaction Transfer(decimal amount, Account account)
        {
            if (_depositRemainTime > 0)
            {
                throw new BankException("Transfer error. The deposit count is yet closed");
            }

            return base.Transfer(amount, account);
        }

        internal override void AccumulatePercents(int assessmentCount)
        {
            if (_depositRemainTime > 0)
                _depositRemainTime--;

            for (int i = 0; i < assessmentCount; i++)
            {
                AccumulatedPercents += Balance * BankInfo.DepositPercents(Balance);
            }
        }

        internal override void PayPercents(int assessmentCount)
        {
            Balance += AccumulatedPercents;
            AccumulatedPercents = 0;
        }
    }
}