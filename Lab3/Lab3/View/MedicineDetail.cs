using System;
using System.Collections.Generic;

namespace Lab3.View;

public partial class MedicineDetail
{
    public int MedicineId { get; set; }

    public string MedicineName { get; set; } = null!;

    public string MedicineDescription { get; set; } = null!;

    public string ActiveSubstance { get; set; } = null!;

    public string UnitOfMeasurement { get; set; } = null!;

    public int MedicineCount { get; set; }

    public string StorageLocation { get; set; } = null!;

    public int? ProducerId { get; set; }

    public string? ProducerName { get; set; }

    public string? ProducerCountry { get; set; }

    public int? DiseaseId { get; set; }

    public string? DiseaseName { get; set; }
}
