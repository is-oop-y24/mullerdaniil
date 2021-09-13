using System;
using System.Collections.Generic;

namespace Shops.Models
{
    public class ShopManager
    {
        private static int _currentId;
        private ProductRegister _productRegister;
        private List<Shop> _shops = new List<Shop>();

        public ShopManager()
        {
            _productRegister = new ProductRegister();
        }

        public Shop Create(string name, string address)
        {
            var newShop = new Shop(name, address, _currentId++, _productRegister);
            _shops.Add(newShop);
            return newShop;
        }

        public Product RegisterProduct(string name)
        {
            return _productRegister.RegisterProduct(new Product(name));
        }

        public Shop FindCheapest(Product product, int count)
        {
            if (!_productRegister.IsProductRegistered(product))
            {
                Console.Error.WriteLine("Can not find cheapest shop. Product is not registered.");
                return null;
            }

            int? findResult = _productRegister.FindCheapest(product, count);
            if (findResult == null)
            {
                Console.Error.WriteLine("Can not find cheapest shop. Product is not available.");
                return null;
            }
            else
            {
                return _shops.Find(shop => shop.Id == findResult);
            }
        }
    }
}