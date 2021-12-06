using System;
using System.IO;
using Banks.Models;
using Banks.Models.Accounts;
using Banks.Models.Banks;
using Banks.Models.Transactions;
using Banks.Tools;
using NUnit.Framework;

namespace Banks.Tests
{
    public class BankTest
    {
        private CentralBank _centralBank;
        private Func<decimal, decimal> _depositPercents;
        private Bank _bank;
        private Client _client;
        private Account _debitAccount;
        private Client _client2;
        private Account _debitAccount2;

        [SetUp]
        public void SetUp()
        {
            _centralBank = new CentralBank();
            _depositPercents = arg => arg < 20000 ? 3 : 7;
            _bank = _centralBank.CreateBank(3.65m, _depositPercents, 100, 10_000, 5_000);
            _client = _bank.CreateClient("Steven", "Stevenson");
            _client.Address = "LA, California";
            _client.PassportNumber = "2718-281828";
            _debitAccount = _bank.CreateDebitAccount(_client);
            _client2 = _bank.CreateClient("Jason", "Newsted");
            _client2.Address = "San Fransisco, California";
            _client2.PassportNumber = "1989-08-29";
            _debitAccount2 = _bank.CreateDebitAccount(_client2);
        }

        [Test]
        public void CreateBank_CentralBankHasBank()
        {
            Bank bank = _centralBank.CreateBank(3.4m, _depositPercents, 50, 5_000, 1_000);
            Assert.AreSame(bank, _centralBank.FindBankById(bank.Id));
        }

        [Test]
        public void AddAccounts_ClientAndBankHaveAccounts()
        {
            Client client = _bank.CreateClient("John", "Johnson");
            client.Address = "London";
            client.PassportNumber = "8432-323506";
            Account debitAccount = _bank.CreateDebitAccount(client);
            Account depositAccount = _bank.CreateDepositAccount(client, 5, 3_500);
            Assert.IsTrue(client.HasDebitAccount);
            Assert.IsTrue(client.HasDepositAccount);
            Assert.IsFalse(client.HasCreditAccount);
            Assert.AreSame(debitAccount, _bank.FindAccountById(debitAccount.Id));
        }

        [Test]
        public void DepositAndWithdrawMoney_AccountHoldsMoney()
        {
            Account account = _bank.CreateCreditAccount(_client);
            account.Deposit(15_000);
            Assert.AreEqual(15_000, account.Balance);
            account.Withdraw(3_000);
            Assert.AreEqual(12_000, account.Balance);
        }

        [Test]
        public void CreateInsecureClient_ClientCannotTransferOrWithdrawMoreThanLimit()
        {
            Client insecureClient = _bank.CreateClient("Dave", "Mustaine");
            Account insecureAccount = _bank.CreateDebitAccount(insecureClient);
            insecureAccount.Deposit(25_000);
            Assert.Throws<BankException>(() => insecureAccount.Withdraw(24_000));
            Assert.Throws<BankException>(() => insecureAccount.Transfer(12_000, _debitAccount));
        }

        [Test]
        public void ClientHasNotEnoughMoney_WithdrawalFails()
        {
            _debitAccount.Deposit(3_000);
            Assert.Throws<BankException>(() => _debitAccount.Withdraw(5_000));
        }
        
        [Test]
        public void RegisterClientAndChangeBankCreditLimit_ClientIsNotified()
        {
            _bank.RegisterClientOnNotification(_client);
            using var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            Account account = _bank.CreateCreditAccount(_client);
            _bank.CreditLimit = 60_000;
            Assert.IsFalse(string.IsNullOrEmpty(stringWriter.ToString()));
        }

        [Test]
        public void TransactionCancels_MoneyReturnsBack()
        {
            _debitAccount.Deposit(7_000);
            Transaction withdrawal = _debitAccount.Withdraw(2_300);
            Assert.AreEqual(4_700, _debitAccount.Balance);
            withdrawal.Cancel();
            Assert.AreEqual(7_000, _debitAccount.Balance);
        }

        [Test]
        public void CreateDepositAccount_MoneyIncrease()
        {
            decimal initialBalance = 5_000;
            Account depositAccount = _bank.CreateDepositAccount(_client, 50, initialBalance);
            _centralBank.AddDays(60);
            Assert.IsTrue(depositAccount.Balance > initialBalance);
        }

        [Test]
        public void TransferMoneyFromOneToAnotherAccount_MoneyTransferred()
        {
            _debitAccount.Deposit(30_000);
            _debitAccount2.Deposit(14_000);
            _debitAccount.Transfer(9_000, _debitAccount2);
            Assert.AreEqual(21_000, _debitAccount.Balance);
            Assert.AreEqual(23_000, _debitAccount2.Balance);
        }
    }
}