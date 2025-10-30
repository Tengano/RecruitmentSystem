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
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> GeneratePasswordResetTokenAsync(string email);
        Task<bool> ResetPasswordAsync(string token, string newPassword);
        Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword);
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
            return Task.CompletedTask;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.HoatDong);
        }

        public async Task<bool> GeneratePasswordResetTokenAsync(string email)
        {
            try
            {
                var user = await GetUserByEmailAsync(email);
                if (user == null)
                    return false;

                user.MaDatLaiMatKhau = Guid.NewGuid().ToString();
                user.ThoiGianHetHanMa = DateTime.Now.AddHours(24);

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ResetPasswordAsync(string token, string newPassword)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.MaDatLaiMatKhau == token 
                                           && u.ThoiGianHetHanMa > DateTime.Now 
                                           && u.HoatDong);
                
                if (user == null)
                    return false;

                user.MatKhau = newPassword;
                user.MaDatLaiMatKhau = null;
                user.ThoiGianHetHanMa = null;

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            try
            {
                var user = await GetUserByIdAsync(userId);
                if (user == null)
                    return false;

                var trimmedOldPassword = oldPassword?.Trim() ?? "";
                var trimmedDbPassword = user.MatKhau?.Trim() ?? "";

                if (trimmedDbPassword != trimmedOldPassword)
                    return false;

                user.MatKhau = newPassword.Trim();
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in ChangePasswordAsync: {ex.Message}");
                return false;
            }
        }
    }
}
