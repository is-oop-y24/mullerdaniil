namespace Shops.Models
{
    internal class ShopItem
    {
        public ShopItem(Product product, int count, double price)
        {
            Product = product;
            Count = count;
            Price = price;
        }

        public Product Product { get; }
        public int Count { get; set; }
        public double Price { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            else
            {
                return Product.Equals(((ShopItem)obj).Product);
            }
        }

        public override int GetHashCode()
        {
            return Product.GetHashCode();
        }
    }
}