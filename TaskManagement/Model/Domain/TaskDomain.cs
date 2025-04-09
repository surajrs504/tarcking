using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Model.Domain
{
    public class TaskDomain
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public string Priority { get; set; }
        public string Assignee { get; set; }
        public string Status { get; set; }

        public Guid? TeamId { get; set; }
        public Guid? UserId { get; set; }

        ////navigation properties
        public TeamDomain? Team { get; set; }
        public UserDomain? User { get; set; }
    }
}
