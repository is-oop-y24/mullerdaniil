using System;

namespace Banks.Models.Transactions
{
    public abstract class Transaction
    {
        public Guid Id { get; } = Guid.NewGuid();
        public abstract void Cancel();
        internal abstract void Execute();
    }
}