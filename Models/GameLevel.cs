using System.Text.Json.Serialization;

namespace WebApplication1.Models
{
    public class GameLevel
    {
        public int GameLevelId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        public GameLevel() { }

        public GameLevel(int gameLevelId, string title, string? description = null)
        {
            GameLevelId = gameLevelId;
            Title = title;
            Description = description;
        }

        [JsonIgnore]
        public ICollection<User>? Users { get; set; }

        [JsonIgnore]
        public ICollection<Question>? Questions { get; set; }

        [JsonIgnore]
        public ICollection<LevelResult>? LevelResults { get; set; }
    }
}
