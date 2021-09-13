using System;
using System.Collections.Generic;
using System.Linq;

namespace Shops.Models
{
    internal class ProductRegister
    {
        private HashSet<Product> _products = new HashSet<Product>();
        private Dictionary<int, HashSet<ShopItem>> _shopItems = new Dictionary<int, HashSet<ShopItem>>();

        public Product RegisterProduct(Product product)
        {
            if (_products.Contains(product))
            {
                Console.Error.WriteLine("Product is already registered.");
                return null;
            }
            else
            {
                _products.Add(product);
                return product;
            }
        }

        public bool AddProduct(ShopItem shopItem, Shop shop)
        {
            if (!IsProductRegistered(shopItem.Product))
            {
                Console.Error.WriteLine("Product has not been registered yet.");
                return false;
            }

            var shopItems = new HashSet<ShopItem>();
            if (!_shopItems.ContainsKey(shop.Id))
            {
                _shopItems.Add(shop.Id, shopItems);
            }
            else
            {
                shopItems = _shopItems[shop.Id];
            }

            if (shopItems.Contains(shopItem))
            {
                ShopItem item = shopItems.First(item => item.Equals(shopItem));
                item.Count += shopItem.Count;
            }
            else
            {
                shopItems.Add(shopItem);
            }

            return true;
        }

        public void TakeProduct(Product product, int count, Shop shop)
        {
            ShopItem shopItem = _shopItems[shop.Id].First(item => item.Product.Equals(product));
            shopItem.Count -= count;
        }

        public ShopItem FindShopItem(Product product, Shop shop)
        {
            try
            {
                return _shopItems[shop.Id].First(item => item.Product.Equals(product));
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public int? FindCheapest(Product product, int count)
        {
            int? cheapestShop = null;
            double cheapestPrice = 0;

            foreach (int shop in _shopItems.Keys)
            {
                if (_shopItems[shop].Contains(new ShopItem(product, 0, 0)))
                {
                    ShopItem item = _shopItems[shop].First(item => item.Product.Equals(product));
                    if (item.Count >= count)
                    {
                        if (cheapestShop == null)
                        {
                            cheapestShop = shop;
                            cheapestPrice = item.Price;
                        }

                        if (item.Price < cheapestPrice)
                        {
                            cheapestShop = shop;
                            cheapestPrice = item.Price;
                        }
                    }
                }
            }

            return cheapestShop;
        }

        public bool IsProductRegistered(Product product)
        {
            return _products.Contains(product);
        }
    }
}