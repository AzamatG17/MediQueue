﻿using MediQueue.Domain.Common;

namespace MediQueue.Domain.Entities;

public class Service : EntityBase
{
    public string Name { get; set; }
    public decimal Amount { get; set; }

    public int? CategoryId { get; set; }
    public virtual Category? Category { get; set; }

    public virtual ICollection<ServiceUsage> ServiceUsages { get; set; }
    public virtual ICollection<Account> Accounts { get; set; }
}
