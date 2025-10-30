using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace RecruitmentSystem.Filters
{
    public class AdminAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userId = context.HttpContext.Session.GetInt32("UserId");
            var userRole = context.HttpContext.Session.GetString("UserRole");

            var controller = context.Controller as Controller;
            var tempData = controller?.TempData;

            if (userId == null)
            {
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