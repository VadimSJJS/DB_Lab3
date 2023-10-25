using System;
using System.Collections.Generic;

namespace Lab3;

public partial class MedicinesForDisease
{
    public int Id { get; set; }

    public int MidicinesId { get; set; }

    public int DiseasesId { get; set; }

    public virtual Disease Diseases { get; set; } = null!;

    public virtual Medicine Midicines { get; set; } = null!;
}
