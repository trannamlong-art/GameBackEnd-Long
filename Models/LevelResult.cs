namespace WebApplication1.Models;
using System.Text.Json.Serialization;
using WebApplication1.Models; // để nhận ra class User

public class LevelResult
{
    public int LevelResultId { get; set; }
    public int UserId { get; set; }           // Khóa ngoại
    public User? User { get; set; }           // Navigation property

    public int GameLevelId { get; set; }      // Khóa ngoại
    public GameLevel? GameLevel { get; set; } // Navigation property

    public int Score { get; set; }
    public DateTime CompletionDate { get; set; }

    public LevelResult()
    {
    }

    public LevelResult(int levelResultId, int score, DateTime completedAt, int userId, int gameLevelId)
    {
        LevelResultId = levelResultId;
        Score = score;
        CompletionDate = completedAt;
        UserId = userId;
        GameLevelId = gameLevelId;
    }

  
}