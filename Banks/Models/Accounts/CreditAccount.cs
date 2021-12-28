using Banks.Models.Banks;
using Banks.Models.Transactions;
using Banks.Tools;

namespace Banks.Models.Accounts
{
    public class CreditAccount : Account
    {
        internal CreditAccount(Client client, BankInfo bankInfo)
            : base(client, bankInfo)
        {
        }

        public override Transaction Withdraw(decimal amount)
        {
            if (!Client.IsSecure())
            {
                if (amount > BankInfo.InsecureAccountWithdrawalLimit)
                {
                    throw new BankException("Withdrawal error. The client is not secure. The amount is too big");
                }
            }

            if (Balance - amount < BankInfo.CreditLimit)
            {
                throw new BankException("Withdrawal error. The amount exceeds the credit limit");
            }

            var transaction = new WithdrawalTransaction(this, amount);
            transaction.Execute();
            return transaction;
        }

        public override Transaction Transfer(decimal amount, Account account)
        {
            if (!Client.IsSecure())
            {
                if (amount > BankInfo.InsecureAccountWithdrawalLimit)
                {
                    throw new BankException("Transfer error. The client is not secure. The amount is too big");
                }
            }

            if (Balance - amount < BankInfo.CreditLimit)
            {
                throw new BankException("Transfer error. The amount exceeds the credit limit");
            }

            if (Id != account.Id)
            {
                throw new BankException("Transfer error. The accounts are from different banks");
            }

            var transaction = new TransferTransaction(this, account, amount);
            transaction.Execute();
            return transaction;
        }

        internal override void AssessCommission(int assessmentCount)
        {
            decimal updatedBalance = Balance - BankInfo.CreditCommission;
            if (updatedBalance >= -BankInfo.CreditLimit)
            {
                Balance = updatedBalance;
            }
        }
    }
}