﻿namespace MediQueue.Domain.DTOs.ServiceUsage;

public record ServiceUsageForCreateDto(
    int ServiceId,
    int AccountId,
    int QuestionnaireHistoryId
    );
