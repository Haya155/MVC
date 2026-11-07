using Microsoft.CodeAnalysis;
using System.ComponentModel.DataAnnotations;

namespace CafeteriaCodeFirstPractice.Models
{
    public class Suppliers
    {
        public int Id { get; set; }

      
        public string Name { get; set; }

       
        public string? Email { get; set; }

        public string? Phone { get; set; }

        public ICollection<Products> Products { get; set; }

        public ICollection<PurchaseOrders> PurchaseOrders { get; set; }
    }
}
