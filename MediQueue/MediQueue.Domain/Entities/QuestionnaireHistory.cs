﻿using MediQueue.Domain.Common;

namespace MediQueue.Domain.Entities;

public class QuestionnaireHistory : EntityBase
{
    public int Historyid { get; set; }
    public string? HistoryDiscription { get; set; }
    public DateTime? DateCreated { get; set; }
    public decimal? Balance { get; set; }
    public bool? IsPayed { get; set; } = false;
    public decimal? InitialDiscountPercentage { get; set; }
    public decimal? InitialBenefitPercentage { get; set; }

    public int? AccountId { get; set; }
    public virtual Account? Account { get; set; }
    public int? QuestionnaireId { get; set; }
    public virtual Questionnaire? Questionnaire { get; set; }
    public virtual ICollection<Discount>? Discounts { get; set; }
    public virtual ICollection<Benefit>? Benefits { get; set; }
    public virtual ICollection<ServiceUsage>? ServiceUsages { get; set; }
    public virtual ICollection<PaymentService>? PaymentServices { get; set; }
    public virtual ICollection<Conclusion>? Conclusions { get; set; }
    public virtual ICollection<AnalysisResult>? AnalysisResults { get; set; }
    public virtual ICollection<StationaryStayUsage>? StationaryStays { get; set; }
} 