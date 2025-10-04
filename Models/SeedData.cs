using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace RecruitmentSystem.Models
{
    public static class SeedData
    {
        public static async Task SeedAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Tạo Admin role nếu chưa có
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // Kiểm tra xem đã có tài khoản admin nào chưa
            var adminExists = await userManager.GetUsersInRoleAsync("Admin");
            if (!adminExists.Any())
            {
                // Tạo tài khoản admin mặc định
                var adminUser = new IdentityUser
                {
                    UserName = "admin@tuyendung.com",
                    Email = "admin@tuyendung.com",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    Console.WriteLine("✅ Đã tạo tài khoản admin: admin@tuyendung.com / Admin@123");
                }
            }
        }
    }
}
