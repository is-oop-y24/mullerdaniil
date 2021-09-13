namespace Shops.Models
{
    public class Product
    {
        internal Product(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            else
            {
                return Name == ((Product)obj).Name;
            }
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}