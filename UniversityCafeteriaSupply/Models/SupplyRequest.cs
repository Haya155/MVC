namespace UniversityCafeteriaSupply.Models
{
    public class SupplyRequest
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public DateTime RequestDate { get; set; }
        public double TotalPrice { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public virtual ICollection<RequestItem> requestItems { get; set; } = new List<RequestItem>();

    }
}
 