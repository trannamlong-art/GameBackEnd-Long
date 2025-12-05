using System.Text.Json.Serialization;

namespace WebApplication1.Models;

public class User
{
    public int userId { get; set; }
    public string username { get; set; } = string.Empty;
    public string? linkAvatar { get; set; }
    public string? otp { get; set; }

    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }

    // Foreign keys
    public int regionId { get; set; }
    public int roleId { get; set; }

    // Navigation properties
    public Region? region { get; set; }
    public Role? role { get; set; }

    [JsonIgnore]
    public ICollection<LevelResult>? LevelResults { get; set; }
}
