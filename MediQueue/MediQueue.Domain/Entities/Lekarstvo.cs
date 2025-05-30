﻿using MediQueue.Domain.Common;
using MediQueue.Domain.Entities.Enums;

namespace MediQueue.Domain.Entities;

public class Lekarstvo : EntityBase
{
    public string? Name { get; set; }
    public string? PhotoBase64 { get; set; }
    public MeasurementUnit? MeasurementUnit { get; set; }

    public int? CategoryLekarstvoId { get; set; }
    public CategoryLekarstvo? CategoryLekarstvo { get; set; }
    public virtual ICollection<Partiya> Partiyas { get; set; }
}