using System;
using System.Collections.Generic;

namespace AcademyGraduationPreparation.DatabaseModels;

public partial class Offer
{
    public int OfferId { get; set; }

    public string OfferName { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public int SupplierId { get; set; }

    public virtual Supplier Supplier { get; set; } = null!;
}
