using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MediQueue.Domain.Entities
{
    public class AuditEntry
    {
        private readonly EntityEntry _entry;

        public AuditEntry(EntityEntry entry)
        {
            _entry = entry;
            TemporaryProperties = new List<PropertyEntry>();
            KeyValues = new Dictionary<string, object>();
            OldValues = new Dictionary<string, object>();
            NewValues = new Dictionary<string, object>();
        }

        public string TableName { get; set; }
        public string Action { get; set; }
        public int UserId { get; set; }

        public Dictionary<string, object> KeyValues { get; }
        public Dictionary<string, object> OldValues { get; }
        public Dictionary<string, object> NewValues { get; }
        public List<PropertyEntry> TemporaryProperties { get; }
        public bool HasTemporaryProperties => TemporaryProperties.Any();

        public AuditLog ToAuditLog()
        {
            return new AuditLog
            {
                AccountId = UserId,
                Timestamp = DateTime.UtcNow,
                Action = Action,
                TableName = TableName,
                RecordId = string.Join(", ", KeyValues.Select(k => $"{k.Key}: {k.Value}")),
                Changes = SerializeChanges()
            };
        }

        private string SerializeChanges()
        {
            var changes = new
            {
                OldValues,
                NewValues
            };
            return System.Text.Json.JsonSerializer.Serialize(changes);
        }
    }
}
