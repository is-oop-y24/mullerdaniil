namespace Shops.Models
{
    public class Person
    {
        private double _money;

        public Person(string name, double startingMoney)
        {
            Name = name;
            _money = startingMoney;
        }

        public string Name { get; }
        public double Money { get => _money; }

        internal void WithdrawMoney(double money)
        {
            _money -= money;
        }
    }
}