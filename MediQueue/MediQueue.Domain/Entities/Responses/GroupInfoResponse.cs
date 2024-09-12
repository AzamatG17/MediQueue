
namespace MediQueue.Domain.Entities.Responses
{
    public class GroupInfoResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public GroupInfoResponse(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
