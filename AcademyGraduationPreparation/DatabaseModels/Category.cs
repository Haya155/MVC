using System;
using System.Collections.Generic;

namespace AcademyGraduationPreparation.DatabaseModels;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();
}
