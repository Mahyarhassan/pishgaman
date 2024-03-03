using EFCore.EfStructures.Entities;
using EFCore.Models;
using EFCore.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Service.valid;
using System.Diagnostics;
using System.Security.Claims;

namespace EFCore.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

        }

        //[Authorize]
        public IActionResult Index()
        {

            if (!string.IsNullOrEmpty(Request.Cookies["jwt"]))
            {
                var jwt = Request.Cookies["jwt"];
                if (jwt != null)
                {
                    var claimsPrincipal = JwtManager.ValidateToken(jwt);
                    if (claimsPrincipal.Item1 != false)
                    {
                        // Save JWT token in cookie
                        //var userNameClaim = claimsPrincipal.Item2.FindFirst(ClaimTypes.Name).Value;
                        ViewBag.curentUser = claimsPrincipal.Item2?.Identity?.Name;
                        ViewBag.curentClaim = claimsPrincipal.Item2?.FindFirst(ClaimTypes.Name)?.Value;
                        return View();
                    }
                    else
                    {
                        return Redirect("/Home/Login");
                    }
                }
                else
                {
                    return Redirect("/Home/Login");
                }

            }
            return Redirect("/Home/Login");

        }
        public IActionResult Login()
        {
            if (!string.IsNullOrEmpty(Request.Cookies["jwt"]))
            {
                var jwt = Request.Cookies["jwt"];
                if (jwt != null)
                {
                    var claimsPrincipal = JwtManager.ValidateToken(jwt);
                    if (claimsPrincipal.Item1 != false)
                    {
                        //return RedirectToAction("index");
                        ViewBag.curentUser = claimsPrincipal.Item2?.Identity?.Name;
                        ViewBag.curentClaim = claimsPrincipal.Item2?.FindFirst(ClaimTypes.Name)?.Value;
                        return Redirect("/Home/Index");
                    }
                    else
                    {
                        return View();
                    }
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        [ValidateAntiForgeryToken]
        public ActionResult UserManeger(string Username, string Password, string FirstName, string LastName, string Phone, string Email)
        {
            try
            {
                if (!MyValidation.IsValidNameAndFamily(FirstName, LastName))
                {
                    return Json("نام یا فامیل نامعتبر است. باید حداقل 3 تا حداکثر 20 حرف باشند");
                }
                if (!MyValidation.IsValidEmail(Email) && !string.IsNullOrEmpty(Email))
                {
                    return Json("آدرس ایمیل وارد شده معتبر نمی باشد");
                }
                if (!MyValidation.IsMobileNumberValid(Phone))
                {
                    return Json("شماره موبایل باید با 09 شروع شده و 11 رقم باشد");
                }


                using (UnitOfWork db = new())
                {
                    if (db.UserRepository.IsExUser(Username, Password))
                    {
                        return Json("شما قبلا ثبت نام کرده اید");
                    }
                    var newUser = new TblUser();
                    newUser.FirstName = FirstName;
                    newUser.LastName = LastName;
                    newUser.Phone = Phone;
                    newUser.Email = Email;
                    newUser.Username = Username;
                    newUser.Password = Password;
                    db.UserGR.insert(newUser);
                    db.Save();
                }

                string userName = Username;
                string jwtToken = JwtManager.GenerateToken(userName);
                Response.Cookies.Append("jwt", jwtToken, new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(300)
                });
                return Json("ok");
            }

            catch (Exception e)
            {
                return Json(e.Message);
            }






        }

        [ValidateAntiForgeryToken]

        public ActionResult LoginUser(string username, string password)
        {
            using (UnitOfWork db = new())
            {
                if (db.UserRepository.IsExUser(username, password))
                {
                    string jwtToken = JwtManager.GenerateToken(username);
                    Response.Cookies.Append("jwt", jwtToken, new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(300)
                    });
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier,username) ,
                        new Claim(ClaimTypes.Name,username) ,
                        new Claim("Access Token",jwtToken)
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    var property = new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        IsPersistent = true,
                    };

                    HttpContext.SignInAsync(principal, property);

                    return Json("ok");
                }
                else
                {
                    return Json("چنین کاربری یافت نشد");
                }
            }
        }
        public ActionResult GetUser()
        {
            using (UnitOfWork db = new())
            {
                var res = db.UserGR.Get();
                return Json(res);
            }
        }


        public IActionResult EditOrAddUser(int id, string EUserName, string EPassword, string EFName, string ELName, string EPhone, string EEmail, string EditOrAdd)
        {
            var state = "true";
            TblUser res;
            try
            {
                if (!MyValidation.IsValidNameAndFamily(EFName, ELName))
                {
                    state = "نام یا فامیل نامعتبر است. باید حداقل 3 تا حداکثر 20 حرف باشند";
                    return Json(new { state });
                }
                if (!MyValidation.IsValidEmail(EEmail) && !string.IsNullOrEmpty(EEmail))
                {
                    state = "آدرس ایمیل وارد شده معتبر نمی باشد";
                    return Json(new { state });
                }
                if (!MyValidation.IsMobileNumberValid(EPhone))
                {
                    state = "شماره موبایل باید با 09 شروع شده و 11 رقم باشد";
                    return Json(new { state });
                }
                if(!MyValidation.IsValidUserNameAndPassword(EUserName, EPassword))
                {
                    state = "برای نام کاربری و رمز عبور نباید از حروف فارسی استفاده شده باشد و نباید کتر از 4 حرف و بیشتر از 20 حرف باشند";
                    return Json(new { state });
                }

                TblUser user = new TblUser()
                {
                    FirstName = EFName,
                    LastName = ELName,
                    Email = EEmail,
                    Phone = EPhone,
                    Username = "test",
                    Password = "123"
                };
                using (UnitOfWork db = new())
                {
                    if (db.UserRepository.IsExUser(EUserName, EPassword))
                    {
                        state = "این نام کاربری از قبل وجود داشته است";
                        return Json(new { state });
                    }
                    if (EditOrAdd == "Edite")
                    {
                        user.UserId = id;
                        db.UserGR.Update(user);
                    }
                    else
                    {
                        db.UserGR.insert(user);
                    }
                    db.Save();
                    res = db.UserRepository.GetUserByPhone(EPhone);
                }
                return Json(new { res, state });
            }
            catch (Exception ex)
            {
                state = ex.Message;
                return Json(new { state });
            }
        }

        public IActionResult DeleteUser(int id)
        {
            try
            {
                using (UnitOfWork db = new())
                {
                    db.UserGR.DeleteById(id);
                    db.Save();
                    return Json(true);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }


        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
