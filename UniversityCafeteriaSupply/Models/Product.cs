namespace UniversityCafeteriaSupply.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }

        public virtual ICollection<RequestItem> requestItems { get; set; } = new List<RequestItem>();


    }
}
