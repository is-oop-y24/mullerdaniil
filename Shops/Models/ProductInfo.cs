namespace Shops.Models
{
    public class ProductInfo
    {
        public ProductInfo(string name, double price, int count)
        {
            Name = name;
            Price = price;
            Count = count;
        }

        public string Name { get; }
        public double Price { get; }
        public int Count { get; }
    }
}