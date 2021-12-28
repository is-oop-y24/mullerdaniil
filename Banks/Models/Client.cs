using System;
using Banks.Models.Banks;

namespace Banks.Models
{
    public class Client
    {
        internal Client(string name, string surname, Guid bankId)
        {
            Name = name;
            Surname = surname;
            BankId = bankId;
            Id = Guid.NewGuid();
        }

        public Guid Id { get; }
        public Guid BankId { get; }
        public string Name { get; }
        public string Surname { get; }
        public string Address { get; set; }
        public string PassportNumber { get; set; }
        public bool HasDebitAccount { get; set; }
        public bool HasDepositAccount { get; set; }
        public bool HasCreditAccount { get; set; }

        public bool IsSecure()
        {
            return Address != null && PassportNumber != null;
        }

        internal void Notify(Bank bank, string message)
        {
            Console.WriteLine("Bank " + bank.Id + " notifies client " + Name + " " + Surname + " : " + message);
        }
    }
}