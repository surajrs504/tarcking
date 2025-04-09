using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Model.Domain
{
    public class TeamDomain
    {
        [Key]
        public Guid Id { get; set; }
        public string teamName { get; set; }
        public string teamDescription { get; set; }
        public string teamAdmin { get; set; }

        public ICollection<UserDomain> Users { get; set; }
    }
}
