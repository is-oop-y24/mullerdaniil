using System;

namespace Banks.Models.Banks
{
    public class BankInfo
    {
        internal BankInfo()
        {
        }

        internal decimal DebitPercentOfAnnual { get; set; }
        internal Func<decimal, decimal> DepositPercents { get; set; }
        internal decimal CreditCommission { get; set; }
        internal decimal CreditLimit { get; set; }
        internal decimal InsecureAccountWithdrawalLimit { get; set; }
    }
}