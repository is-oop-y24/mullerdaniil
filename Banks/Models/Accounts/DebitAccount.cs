using Banks.Models.Banks;

namespace Banks.Models.Accounts
{
    public class DebitAccount : Account
    {
        internal DebitAccount(Client client, BankInfo bankInfo)
            : base(client, bankInfo)
        {
        }

        internal override void AccumulatePercents(int assessmentCount)
        {
            for (int i = 0; i < assessmentCount; i++)
            {
                AccumulatedPercents += Balance * BankInfo.DebitPercentOfAnnual / 100 / 365;
            }
        }

        internal override void PayPercents(int assessmentCount)
        {
            Balance += AccumulatedPercents;
            AccumulatedPercents = 0;
        }
    }
}