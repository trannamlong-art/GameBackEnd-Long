    namespace WebApplication1.Controllers;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using WebApplication1.Data;
    using WebApplication1.Models;
    using WebApplication1.DTO;
    using WebApplication1.ViewModels;
    using WebApplication1.Service;

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;
        protected ResponseApi _response;

        public UserController(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
            _response = new ResponseApi();
        }

         [HttpPost("SendTestEmail")]
    public async Task<IActionResult> SendTestEmail()
    {
        try
        {
            await _emailService.SendEmailAsync(
                "recipient@example.com",
                "Test Email from ASP.NET Core",
                "Xin chào! Đây là mail test từ WebApplication1."
            );

            return Ok(new { isSuccess = true, notification = "Mail đã gửi thành công" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { isSuccess = false, notification = "Gửi mail thất bại", data = ex.Message });
        }
    }
    
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
                if (user == null)
                {
                    _response.IsSuccess = false;
                    _response.Notification = "Người dùng không tồn tại";
                    return NotFound(_response);
                }

                if (user.otp == dto.OTP.ToString())
                {
                    user.Password = dto.NewPassword; // Giữ kiểu plain text hoặc hash nếu muốn
                    user.otp = null;
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.Notification = "Đặt lại mật khẩu thành công";
                    return Ok(_response);
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.Notification = "Mã OTP không đúng";
                    return BadRequest(_response);
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Notification = "Đặt lại mật khẩu thất bại";
                _response.Data = ex.Message;
                return BadRequest(_response);
            }
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO dto)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
                if (user == null)
                {
                    _response.IsSuccess = false;
                    _response.Notification = "Người dùng không tồn tại";
                    return NotFound(_response);
                }

                // Tạo OTP
                Random rand = new Random();
                string otp = rand.Next(100000, 999999).ToString();
                user.otp = otp;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                // Gửi email OTP
                string subject = "Mã OTP đặt lại mật khẩu";
                string message = $"Mã OTP của bạn là: {otp}";
                await _emailService.SendEmailAsync(user.Email, subject, message);

                _response.IsSuccess = true;
                _response.Notification = "Đã gửi mã OTP đến email";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Notification = "Yêu cầu quên mật khẩu thất bại";
                _response.Data = ex.Message;
                return BadRequest(_response);
            }
        }


        [HttpPost("CheckOTP")]
    public async Task<IActionResult> CheckOTP([FromBody] CheckOTPDTO dto)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
            {
                _response.IsSuccess = false;
                _response.Notification = "Người dùng không tồn tại";
                return NotFound(_response);
            }

            if (user.otp == dto.OTP.ToString())
            {
                _response.IsSuccess = true;
                _response.Notification = "Xác thực OTP thành công";
                return Ok(_response);
            }
            else
            {
                _response.IsSuccess = false;
                _response.Notification = "Mã OTP không đúng";
                return BadRequest(_response);
            }
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Notification = "Xác thực OTP thất bại";
            _response.Data = ex.Message;
            return BadRequest(_response);
        }
    }


            [HttpDelete("DeleteAccount/{userId}")]
public async Task<IActionResult> DeleteAccount(int userId)
{
    try
    {
        // Dùng IgnoreQueryFilters để vẫn lấy được user đã bị soft delete
        var user = await _context.Users
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.userId == userId);

        if (user == null)
        {
            _response.IsSuccess = false;
            _response.Notification = "Người dùng không tồn tại";
            return NotFound(_response);
        }

        if (user.IsDeleted)
        {
            _response.IsSuccess = false;
            _response.Notification = "Tài khoản đã bị xóa trước đó";
            return BadRequest(_response);
        }

        // Soft delete
        user.IsDeleted = true;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        _response.IsSuccess = true;
        _response.Notification = "Xóa tài khoản thành công";
        return Ok(_response);
    }
    catch (Exception ex)
    {
        _response.IsSuccess = false;
        _response.Notification = "Xóa tài khoản thất bại";
        _response.Data = ex.Message;
        return BadRequest(_response);
    }
}


        [HttpPost("UpdateUserInformation")]
    public async Task<IActionResult> UpdateUserInformation([FromForm] UserInformationDTO dto)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.userId == dto.UserId);
            if (user == null)
            {
                _response.IsSuccess = false;
                _response.Notification = "Người dùng không tồn tại";
                return NotFound(_response);
            }

            user.username = dto.Name;
            user.regionId = dto.RegionId;
            if (dto.Avatar != null && dto.Avatar.Length > 0)
        {
            var filePath = Path.Combine("wwwroot/avatars", dto.Avatar.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.Avatar.CopyToAsync(stream);
            }
            user.linkAvatar = "/avatars/" + dto.Avatar.FileName; // gán đường dẫn string
        }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            _response.IsSuccess = true;
            _response.Notification = "Cập nhật thông tin thành công";
            _response.Data = user;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Notification = "Cập nhật thông tin thất bại";
            _response.Data = ex.Message;
            return BadRequest(_response);
        }
    }


        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO dto)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.userId == dto.UserId);
                if (user == null)
                {
                    _response.IsSuccess = false;
                    _response.Notification = "Người dùng không tồn tại";
                    return NotFound(_response);
                }

                if (user.Password != dto.OldPassword)
                {
                    _response.IsSuccess = false;
                    _response.Notification = "Mật khẩu cũ không đúng";
                    return BadRequest(_response);
                }

                user.Password = dto.NewPassword;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Notification = "Đổi mật khẩu thành công";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Notification = "Đổi mật khẩu thất bại";
                _response.Data = ex.Message;
                return BadRequest(_response);
            }
        }

        [HttpGet("GetUserInformation/{userId}")]
    public async Task<IActionResult> GetUserInformation(int userId)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.userId == userId);
            if (user == null)
            {
                _response.IsSuccess = false;
                _response.Notification = "Người dùng không tồn tại";
                return NotFound(_response);
            }

            var region = await _context.Regions.FirstOrDefaultAsync(r => r.regionId == user.regionId);
            var userInfo = new UserInformationVM
            {
                Name = user.username,
                Email = user.Email,
                Avatar = user.linkAvatar,
                Region = region != null ? region.Name : "Unknown"
            };

            _response.IsSuccess = true;
            _response.Notification = "Lấy thông tin người dùng thành công";
            _response.Data = userInfo;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Notification = "Lấy thông tin người dùng thất bại";
            _response.Data = ex.Message;
            return BadRequest(_response);
        }
    }


        [HttpPost("SaveResult")]
        public async Task<IActionResult> SaveResult([FromBody] LevelResultDTO dto)
        {  
            try
            {
            var levelResult = new LevelResult
            {
                UserId = dto.UserId,
                GameLevelId = dto.GameLevelId,
                Score = dto.Score,
                CompletionDate = DateTime.Now
            };

            await _context.LevelResults.AddAsync(levelResult);
            await _context.SaveChangesAsync();

            _response.Notification = "Lưu kết quả thành công";
            _response.Data = levelResult;
            _response.IsSuccess = true;

            return Ok(_response);
            }
        
            catch (Exception ex)
            {
            _response.Notification = "Lưu kết quả thất bại";
            _response.IsSuccess = false;
            _response.Data = ex.Message;

            return BadRequest(_response);
            }
        }


    [HttpGet("Rating/{idRegion}")]
    public async Task<IActionResult> Rating(int idRegion)
        {
            try
            {
            RatingVM vm = new();
            vm.UserResultSums = new();

                if (idRegion > 0)
                {
                    var region = await _context.Regions.FirstOrDefaultAsync(r => r.regionId == idRegion);
                    if (region == null)
                        return NotFound("Không tìm thấy vùng");

                    var users = await _context.Users.Where(u => u.regionId == idRegion).ToListAsync();
                    var results = await _context.LevelResults
                        .Where(r => users.Select(u => u.userId).Contains(r.UserId))
                        .ToListAsync();

                    vm.NameRegion = region.Name;

                    foreach (var user in users)
                    {
                        var sumScore = results.Where(r => r.UserId == user.userId).Sum(r => r.Score);
                        var sumLevel = results.Count(r => r.UserId == user.userId);

                        vm.UserResultSums.Add(new UserResultSum
                        {
                            NameUser = user.username,
                            SumScore = sumScore,
                            SumLevel = sumLevel
                        });
                    }
                }
                else
                {
                    var users = await _context.Users.ToListAsync();
                    var results = await _context.LevelResults.ToListAsync();

                    vm.NameRegion = "All Regions";

                    foreach (var user in users)
                    {
                        var sumScore = results.Where(r => r.UserId == user.userId).Sum(r => r.Score);
                        var sumLevel = results.Count(r => r.UserId == user.userId);

                        vm.UserResultSums.Add(new UserResultSum
                        {
                            NameUser = user.username,
                            SumScore = sumScore,
                            SumLevel = sumLevel
                        });
                    } 
                }

                _response.Data = vm;
                _response.IsSuccess = true;
                _response.Notification = "Lấy dữ liệu thành công";
                return Ok(_response);
            }
        
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Notification = "Lấy dữ liệu thất bại";
                _response.Data = ex.Message;
                return BadRequest(_response);
            }
        }

        [HttpGet("GetAllQuestionGameByLevel/{levelId}")]
        public async Task<IActionResult> GetAllQuestionGameByLevel(int levelId)
        {
            try
            {
            var questions = await _context.Questions
                .Where(q => q.GameLevelId == levelId)
                .ToListAsync();
            _response.Notification = "Lấy dữ liệu thành công";
            _response.Data = questions;
            _response.IsSuccess = true;
            return Ok(_response);
            }
            catch (Exception ex)
            {
            _response.Notification = "Lấy dữ liệu thất bại";
            _response.IsSuccess = false;
            _response.Data = ex.Message;
            return BadRequest(_response);
            }
        }

        [HttpPost("Register")]
public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
{
    try
    {
        // Kiểm tra email đã tồn tại
        var existUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (existUser != null)
        {
            _response.IsSuccess = false;
            _response.Notification = "Email đã được đăng ký";
            return BadRequest(_response);
        }

        // Optional: kiểm tra regionId hợp lệ
        var regionExist = await _context.Regions.AnyAsync(r => r.regionId == dto.RegionId);
        if (!regionExist)
        {
            _response.IsSuccess = false;
            _response.Notification = "RegionId không hợp lệ";
            return BadRequest(_response);
        }

        var user = new User
        {
            username = dto.Name,
            Email = dto.Email,
            Password = dto.Password, // nếu muốn hash thì hash ở đây
            regionId = dto.RegionId,
            roleId = 2, // Mặc định là User
            linkAvatar = dto.LinkAvatar
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        _response.IsSuccess = true;
        _response.Notification = "Đăng ký thành công";
        _response.Data = user;
        return Ok(_response);
    }
    catch (DbUpdateException dbEx)
    {
        // Lấy lỗi thật sự từ database
        _response.IsSuccess = false;
        _response.Notification = "Đăng ký thất bại";
        _response.Data = dbEx.InnerException?.Message ?? dbEx.Message;
        return BadRequest(_response);
    }
    catch (Exception ex)
    {
        _response.IsSuccess = false;
        _response.Notification = "Đăng ký thất bại";
        _response.Data = ex.Message;
        return BadRequest(_response);
    }
}



        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest dto)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email && u.Password == dto.Password);
                if (user != null)
                {
                    _response.IsSuccess = true;
                    _response.Notification = "Đăng nhập thành công";
                    _response.Data = user;
                    return Ok(_response);
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.Notification = "Đăng nhập thất bại";
                    return BadRequest(_response);
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Notification = "Đăng nhập thất bại";
                _response.Data = ex.Message;
                return BadRequest(_response);
            }
        }
        
        [HttpGet("GetAllGameLevel")]
        public async Task<IActionResult> GetAllGameLevel()
        {
            try
            {
            var gameLevels = await _context.GameLevels.ToListAsync();
            _response.Notification = "Lấy dữ liệu thành công";
            _response.Data = gameLevels;
            _response.IsSuccess = true;
            return Ok(_response);
            }
            catch (Exception ex)
            {
            _response.Notification = "Lấy dữ liệu thất bại";
            _response.IsSuccess = false;
            _response.Data = ex.Message;
            return BadRequest(_response);
            }
        }
        [HttpGet("GetAllQuestionGame")]
        public async Task<IActionResult> GetAllQuestionGame()
        {
            try
            {
            var questions = await _context.Questions.ToListAsync();
            _response.Notification = "Lấy dữ liệu thành công";
            _response.Data = questions;
            _response.IsSuccess = true;
            return Ok(_response);
            }
            catch (Exception ex)
            {
            _response.Notification = "Lấy dữ liệu thất bại";
            _response.IsSuccess = false;
            _response.Data = ex.Message;
            return BadRequest(_response);
            }
        }
        [HttpGet("GetAllRegion")]
        public async Task<IActionResult> GetAllRegion()
        {
            try
            {
            var regions = await _context.Regions.ToListAsync();
            _response.Notification = "Lấy dữ liệu thành công";
            _response.Data = regions;
            _response.IsSuccess = true;
            return Ok(_response);
            }
            catch (Exception ex)
            {
            _response.Notification = "Lấy dữ liệu thất bại";
            _response.IsSuccess = false;
            _response.Data = ex.Message;
            return BadRequest(_response);
            }
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users
                .Include(u => u.role)
                .Include(u => u.region)
                .ToListAsync();
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.Include(u => u.role).Include(u => u.region).FirstOrDefaultAsync(u => u.userId == id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: api/User
       // POST: api/User/CreateUser
[HttpPost("CreateUser")]
public async Task<ActionResult<User>> PostUser(User user)
{
    // Add new user to the database
    _context.Users.Add(user);
    await _context.SaveChangesAsync();

    return CreatedAtAction("GetUser", new { id = user.userId}, user);
}

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.userId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return NoContent();
        }
        
    }