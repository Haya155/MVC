using System.ComponentModel.DataAnnotations.Schema;

namespace CafeteriaCodeFirstPractice.Models
{
    public class Products
    {
        public int Id { get; set; }

     
        public string Name { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public int CategoryId { get; set; }

        public int SupplierId { get; set; }

        public Categories Category { get; set; }

        public Suppliers Supplier { get; set; }

        public ICollection<PurchaseOrderItems> PurchaseOrderItems { get; set; }
    }
}
