﻿using MediQueue.Domain.Entities.Enums;
using MediQueue.Domain.Entities.Responses;

namespace MediQueue.Domain.DTOs.Questionnaire
{
    public record QuestionnaireForCreateDto(
        decimal? Balance,
        Gender? Gender, 
        string? PassportSeria,
        string? PassportPinfl,
        string? PhoneNumber,
        string? FirstName,
        string? LastName,
        string? SurName,
        DateTime? DateIssue,
        DateTime? DateBefore,
        string? Region,
        string? District, 
        string? Posolos,
        string? Address, 
        DateTime? Bithdate,
        string? SocialSattus,
        string? AdvertisingChannel,
        string? PhotoBase64,
        string? HistoryDiscription,
        int? AccountId,
        List<ServiceAndAccountResponse>? ServiceAndAccountIds,
        List<int>? DiscountIds,
        List<int>? BenefitIds);
}
