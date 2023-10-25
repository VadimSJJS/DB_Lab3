using System;
using System.Collections.Generic;

namespace Lab3;

public partial class Incoming
{
    public int Id { get; set; }

    public int MedicineNameId { get; set; }

    public DateTime ArrivalDate { get; set; }

    public int Count { get; set; }

    public string Provider { get; set; } = null!;

    public decimal Price { get; set; }

    public virtual Medicine MedicineName { get; set; } = null!;
}
