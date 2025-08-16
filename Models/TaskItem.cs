using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace ToDoApi.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Status { get; set; } = "pending";
        public string Priority { get; set; } = "low";

    }
}
