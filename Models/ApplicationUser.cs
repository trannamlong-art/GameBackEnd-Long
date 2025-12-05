using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace WebApplication1.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Avatar { get; set; }
        public int RegionId { get; set; }
        public bool IsDeleted { get; set; } = false;

        [JsonIgnore]
        public string OTP { get; set; }
             = DateTimeOffset.Now.ToUnixTimeSeconds().ToString() + "none";
    }
}
