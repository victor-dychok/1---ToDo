
using Common.Domain;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;

namespace ToDoDomain
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string? Label { get; set; } = default!;
        public bool IsDone { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int OwnerId { get; set; }
        public virtual AppUser User { get; set; }

        public TodoItem() { }
        public TodoItem(int id, string label, bool isDone, DateTime createdDate, DateTime updatedDate, int ownerId)
        {
            Id = id;
            Label = label;
            IsDone = isDone;
            CreatedDate = createdDate;
            UpdatedDate = updatedDate;
            OwnerId = ownerId;
        }
    }
}
