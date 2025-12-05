namespace WebApplication1.DTO
{
   public class ResetPasswordDTO
   {
       public string Email { get; set; }
       public string NewPassword { get; set; }
       public int OTP { get; set; }
   }
}