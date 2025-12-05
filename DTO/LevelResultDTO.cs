namespace WebApplication1.DTO
{
    public class LevelResultDTO
    {
        public int UserId { get; set; }
        public int GameLevelId { get; set; }   // <- thêm cái này
        public int Score { get; set; }
    }
}
