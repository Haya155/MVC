namespace CafeteriaCodeFirstPractice.Models
{
    public class PurchaseOrders
    {
        public int Id { get; set; }

        public DateTime OrderDate { get; set; }

       
        public string Status { get; set; }

        public int SupplierId { get; set; }

        public Suppliers Supplier { get; set; }

        public ICollection<PurchaseOrderItems> PurchaseOrderItems { get; set; }
    }
}
