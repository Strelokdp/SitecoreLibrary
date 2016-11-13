using System.Web.Mvc;
using System.Web.Security;
using SitecoreLibrary.Models;

namespace SitecoreLibrary.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LogOn model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    var x = FormsAuthentication.GetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        var y = x;
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("GetAllBooks", "Book");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Wrong login or password");
                }
            }
            return View(model);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Register model)
        {
            if (ModelState.IsValid)
            {
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, passwordQuestion: null, passwordAnswer: null, isApproved: true, providerUserKey: null, status: out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false);
                    return RedirectToAction("GetAllBooks", "Book");
                }
                else
                {
                    ModelState.AddModelError("", "Registration error");
                }
            }

            return View(model);
        }
    }
}