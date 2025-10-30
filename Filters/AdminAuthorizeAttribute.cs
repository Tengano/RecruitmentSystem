using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace RecruitmentSystem.Filters
{
    /// <summary>
    /// Filter kiểm tra quyền Admin - chỉ cho phép Admin truy cập
    /// </summary>
    public class AdminAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userId = context.HttpContext.Session.GetInt32("UserId");
            var userRole = context.HttpContext.Session.GetString("UserRole");

            // Lấy TempData từ controller
            var controller = context.Controller as Controller;
            var tempData = controller?.TempData;

            if (userId == null)
            {
                // Người dùng chưa đăng nhập
                if (tempData != null)
                {
                    tempData["ErrorMessage"] = "Vui lòng đăng nhập để tiếp tục!";
                }
                context.Result = new RedirectToActionResult("Login", "Account", new 
                { 
                    returnUrl = context.HttpContext.Request.Path 
                });
                return;
            }

            if (userRole != "Admin")
            {
                // Người dùng không có quyền Admin
                if (tempData != null)
                {
                    tempData["ErrorMessage"] = "Bạn không có quyền truy cập trang này!";
                }
                context.Result = new RedirectToActionResult("Index", "Home", null);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}

