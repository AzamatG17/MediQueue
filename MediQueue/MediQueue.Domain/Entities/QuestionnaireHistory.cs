using MediQueue.Domain.Common;

namespace MediQueue.Domain.Entities
{
    public class QuestionnaireHistory : EntityBase
    {
        public string HistoryDiscription { get; set; }

        public int? AccountId { get; set; }
        public Account? Account { get; set; }
        public int QuestionnaireId { get; set; }
        public Questionnaire Questionnaire { get; set; }
    }
}