using Banks.Models.Accounts;

namespace Banks.Models.Transactions
{
    public class TransferTransaction : Transaction
    {
        private readonly Account _from;
        private readonly Account _to;
        private readonly decimal _amount;

        internal TransferTransaction(Account from, Account to, decimal amount)
        {
            _from = from;
            _to = to;
            _amount = amount;
        }

        public override void Cancel()
        {
            _from.ChangeBalance(+_amount);
            _to.ChangeBalance(-_amount);
        }

        internal override void Execute()
        {
            _from.ChangeBalance(-_amount);
            _to.ChangeBalance(+_amount);
        }
    }
}