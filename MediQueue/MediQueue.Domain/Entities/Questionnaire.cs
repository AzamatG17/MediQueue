using MediQueue.Domain.Common;

namespace MediQueue.Domain.Entities
{
    public class Questionnaire : EntityBase
    {
        public int QuestionnaireId { get; set; }
        public decimal Balance { get; set; }
        public Gender Gender { get; set; }
        
        public int AccountId { get; set; }
        public Account Account { get; set; }
    }
}
