namespace AcademyGraduationPreparation.Models
{
    public class Offer
    {
        public string Category { get; set; }
        public string SupplierName{  get; set; }
        public string OfferName { get; set; }
        public int Quantity { get; set; }
               
        public decimal UnitPrice { get; set; }
    }
}
