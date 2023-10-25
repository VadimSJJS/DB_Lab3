using System;
using System.Collections.Generic;

namespace Lab3.View;

public partial class OutgoingDetail
{
    public int OutgoingId { get; set; }

    public string MedicineName { get; set; } = null!;

    public string? ProducerName { get; set; }

    public DateTime ImplementationDate { get; set; }

    public int OutgoingCount { get; set; }

    public decimal SellingPrice { get; set; }
}
