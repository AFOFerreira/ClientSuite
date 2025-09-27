using SQLite;

namespace ClientSuite.Model.Entities.Base
{
    public abstract class BaseEntity
    {
        [PrimaryKey]
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
