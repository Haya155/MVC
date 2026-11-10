namespace UniversityCafeteriaSupply.Models
{
    public class RequestItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public int SupplyRequestId { get; set; }

        public int ProductId  { get; set; }

        public virtual Product? product { get; set; }

        public virtual SupplyRequest? supplyRequest { get; set; }

    }
}
