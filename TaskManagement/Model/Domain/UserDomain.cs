using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Model.Domain
{
    public class UserDomain
    {
        [Key]
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }  // Store hashed password

        //// Foreign key to Team
        public Guid TeamId { get; set; }

        public TeamDomain Team { get; set; }


        // Navigation property to Tasks
       //public ICollection<Task> Tasks { get; set; }
    }
}
