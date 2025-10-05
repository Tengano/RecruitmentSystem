using RecruitmentSystem.Data;
using RecruitmentSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace RecruitmentSystem.Services
{
    public interface IAuthenticationService
    {
        Task<User?> LoginAsync(string tenDangNhap, string matKhau);
        Task<bool> RegisterAsync(User user);
        Task<User?> GetUserByIdAsync(int id);
        Task LogoutAsync();
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApplicationDbContext _context;

        public AuthenticationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> LoginAsync(string tenDangNhap, string matKhau)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.TenDangNhap == tenDangNhap 
                                       && u.MatKhau == matKhau 
                                       && u.HoatDong);
        }

        public async Task<bool> RegisterAsync(User user)
        {
            try
            {
                // Kiểm tra tên đăng nhập và email đã tồn tại chưa
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.TenDangNhap == user.TenDangNhap || u.Email == user.Email);
                
                if (existingUser != null)
                    return false;

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public Task LogoutAsync()
        {
            // Đơn giản chỉ cần xóa session
            return Task.CompletedTask;
        }
    }
}
