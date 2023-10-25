using System;
using System.Collections.Generic;

namespace Lab3.View;

public partial class IncomingDetail
{
    public int IncomingId { get; set; }

    public string MedicineName { get; set; } = null!;

    public string? ProducerName { get; set; }

    public DateTime ArrivalDate { get; set; }

    public int IncomingCount { get; set; }

    public string Provider { get; set; } = null!;

    public decimal Price { get; set; }
}
