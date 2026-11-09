namespace UniversityCafeteria5.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Phone { get; set; }
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
