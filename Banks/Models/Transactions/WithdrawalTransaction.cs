using System;
using Banks.Models.Accounts;

namespace Banks.Models.Transactions
{
    public class WithdrawalTransaction : Transaction
    {
        private readonly Account _account;
        private readonly decimal _amount;

        internal WithdrawalTransaction(Account account, decimal amount)
        {
            _account = account;
            _amount = amount;
        }

        public override void Cancel()
        {
            _account.ChangeBalance(+_amount);
        }

        internal override void Execute()
        {
            _account.ChangeBalance(-_amount);
        }
    }
}