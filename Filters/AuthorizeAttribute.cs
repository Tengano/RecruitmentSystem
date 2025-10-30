using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace RecruitmentSystem.Filters
{
    /// <summary>
    /// Filter kiểm tra đăng nhập - yêu cầu người dùng phải đăng nhập
    /// </summary>
    public class AuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userId = context.HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                // Lấy TempData từ controller
                var controller = context.Controller as Controller;
                var tempData = controller?.TempData;

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

            base.OnActionExecuting(context);
        }
    }
}

