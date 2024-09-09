using MediQueue.Domain.Common;

namespace MediQueue.Domain.Entities
{
    public class Questionnaire : EntityBase
    {
        public int QuestionnaireId { get; set; }
        public decimal Balance { get; set; }
        public Gender Gender { get; set; }
        public string Passport { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SurName { get; set; }
        public string Region { get; set; }
        public DateTime Bithdate { get; set; }

        public ICollection<QuestionnaireHistory> QuestionnaireHistories { get; set; }
    }
}
