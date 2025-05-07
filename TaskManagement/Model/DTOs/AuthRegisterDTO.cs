using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Model.DTOs
{
    public class AuthRegisterDTO
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Username { get; set; }

        public string PhoneNumber { get; set; }

        public string Role { get; set; }
    }
}
