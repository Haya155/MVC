using System;
using System.Collections.Generic;

namespace UniversityCafeteria7.Models;

public partial class Order
{
    public int Id { get; set; }

    public string? CustomerName { get; set; }

    public DateOnly? OrderDate { get; set; }

    public decimal? TotalPrice { get; set; }

    public string? Status { get; set; }

    public int? EmployeeId { get; set; }

    public virtual Employee? Employee { get; set; }
}
