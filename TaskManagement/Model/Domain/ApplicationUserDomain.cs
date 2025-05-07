using Microsoft.AspNetCore.Identity;

namespace TaskManagement.Model.Domain
{
    public class ApplicationUserDomain:IdentityUser
    {
        public Boolean IsApproved { get; set; } = false;
        public Boolean IsRejected { get; set; } = true;
        public Boolean IsTrackingEnabled { get; set; } = false;

    }
}
