using System;

namespace Shops.Models
{
    public class Shop
    {
        private ProductRegister _productRegister;
        internal Shop(string name, string address, int id, ProductRegister productRegister)
        {
            Name = name;
            Address = address;
            Id = id;
            _productRegister = productRegister;
        }

        public string Name { get; }
        public string Address { get; }
        public int Id { get; }

        public bool AddProducts(Product product, int count, double price)
        {
            return _productRegister.AddProduct(new ShopItem(product, count, price), this);
        }

        public bool Buy(Person person, Product product, int count)
        {
            ShopItem shopItem = _productRegister.FindShopItem(product, this);

            if (shopItem == null)
            {
                Console.Error.WriteLine("Can not buy the product. No such product in the shop.");
                return false;
            }

            double expectedMoney = shopItem.Price * count;
            double actualMoney = person.Money;

            if (count > shopItem.Count)
            {
                Console.Error.WriteLine("Can not buy the product. Not enough product in the shop.");
            }

            if (actualMoney >= expectedMoney)
            {
                person.WithdrawMoney(expectedMoney);
                _productRegister.TakeProduct(product, count, this);
                return true;
            }
            else
            {
                Console.Error.WriteLine("Can not buy the product. Not enough money.");
                return false;
            }
        }

        public ProductInfo GetProductInfo(Product product)
        {
            ShopItem item = _productRegister.FindShopItem(product, this);
            return new ProductInfo(item.Product.Name, item.Price, item.Count);
        }

        public void ChangePrice(Product product, double newPrice)
        {
            ShopItem item = _productRegister.FindShopItem(product, this);
            item.Price = newPrice;
        }
    }
}