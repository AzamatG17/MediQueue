using MediQueue.Domain.DTOs.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediQueue.Domain.DTOs.QuestionnaireHistory
{
    public record QuestionnaireHistoryForCreateDto(
        string? HistoryDiscription, DateTime? DateCreated, decimal? Balance, bool? IsPayed, int? AccountId, int? QuestionnaireId, List<int>? ServiceIds);
}