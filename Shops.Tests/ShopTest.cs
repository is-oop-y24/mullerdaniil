using NUnit.Framework;
using Shops.Models;

namespace Shops.Tests
{
    [TestFixture]
    public class ShopTests
    {
        private ShopManager _shopManager;

        [SetUp]
        public void Setup()
        {
            _shopManager = new ShopManager();
        }

        [Test]
        public void RegisterAndAddProducts_AbleToBuyProducts()
        {
            const double moneyBefore = 12;
            const double productPrice = 1.99;
            const int productCount = 5;
            const int productToBuyCount = 3;
            
            Shop shop = _shopManager.Create("City Mall", "New York City");
            Product product = _shopManager.RegisterProduct("Coca Cola");
            Assert.IsTrue(shop.AddProducts(product, productCount, productPrice));

            Person buyer = new Person("John Travolta", moneyBefore);
            Assert.IsTrue(shop.Buy(buyer, product, productToBuyCount));

            Assert.AreEqual(moneyBefore - productPrice  * productToBuyCount, buyer.Money);
            Assert.AreEqual(productCount - productToBuyCount , shop.GetProductInfo(product).Count);
        }

        [Test]
        public void ChangeProductPrice_PriceChanged()
        {
            const int count = 230;
            const double oldPrice = 47.70;
            const double newPrice = 45.34;

            Shop shop = _shopManager.Create("Lenta", "SPb City");
            Product product = _shopManager.RegisterProduct("Jack Daniels");

            shop.AddProducts(product, count, oldPrice);
            shop.ChangePrice(product, newPrice);

            Assert.AreEqual(newPrice, shop.GetProductInfo(product).Price);
        }

        [Test]
        public void FindCheapestShop_CheapestFound()
        {
            Shop shop1 = _shopManager.Create("Pyaterochka", "Moscow");
            Shop shop2 = _shopManager.Create("Ashan", "Omsk");
            Shop shop3 = _shopManager.Create("Magnit", "Sochi");

            Product product = _shopManager.RegisterProduct("Schweppes Spritz Aperitivo");
            shop1.AddProducts(product, 10, 58.99);
            shop2.AddProducts(product, 45, 65.99);
            shop3.AddProducts(product, 40, 89.99);
            
            Assert.AreSame(shop2, _shopManager.FindCheapest(product, 30));


            Product product2 = _shopManager.RegisterProduct("KitKat");
            shop1.AddProducts(product2, 5, 18.80);
            shop2.AddProducts(product2, 7, 32.50);
            
            Assert.IsNull(_shopManager.FindCheapest(product2, 10));
        }

        [Test]
        public void TryToBuyProductIfNotEnoughMoney_UnableToBuyProduct()
        {
            double moneyBefore = 170;
            int productCount = 1;
            
            Shop shop = _shopManager.Create("Perekrestok", "Vladivostok");
            Product product = _shopManager.RegisterProduct("Pringles");
            shop.AddProducts(product, productCount, 250);
            Person person = new Person("Bastian Schweinsteiger", moneyBefore);
            
            Assert.IsFalse(shop.Buy(person, product, 1));
            Assert.AreEqual(moneyBefore, person.Money);
            Assert.AreEqual(productCount, shop.GetProductInfo(product).Count);
        }
        
    }
}