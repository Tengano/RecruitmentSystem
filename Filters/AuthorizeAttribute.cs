using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace RecruitmentSystem.Filters
{
    public class AuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userId = context.HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                var controller = context.Controller as Controller;
                var tempData = controller?.TempData;

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