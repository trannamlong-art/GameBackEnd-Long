// ViewModels/RatingVM.cs
namespace WebApplication1.ViewModels
{
    public class RatingVM
    {
        public string NameRegion { get; set; } = string.Empty;
        public List<UserResultSum> UserResultSums { get; set; } = new();
    }
}
