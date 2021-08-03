using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MIS.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MIS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Context _context;

        public HomeController(ILogger<HomeController> logger,Context context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index(string ReturnUrl)
        {
            ViewBag.url = ReturnUrl;
            return View();
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Index(Login login, System.Uri ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = _context.User.FirstOrDefault(w => w.username == login.username);
                if (user==null)
                {
                    ModelState.AddModelError("username", "کاربری یافت نشد.");
                    return View(login);
                }
                if (user.password==login.password)
                {
                    var accesses = _context.Access.Where(w => w.userid == user.id).ToList();
                    List<Action> actions = new List<Action>();

                    foreach (var item in accesses)
                    {
                        actions.Add(_context.Action.Find(item.actionid));
                    }

                    const string Issuer = "https://localhost:44366";
                    var claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Name, login.username.ToLower(), ClaimValueTypes.String, Issuer));
                    foreach (var item in actions)
                    {
                        claims.Add(new Claim(item.url, "ok"));
                    }


                    var userIdentity = new ClaimsIdentity(login.username);
                    userIdentity.AddClaims(claims);
                    var userPrincipal = new ClaimsPrincipal(userIdentity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);


                    if (ReturnUrl != null)
                    {
                        return Redirect(ReturnUrl.ToString());
                    }
                    return Redirect("/Panel");
                }
                ModelState.AddModelError("password", "رمز عبور اشتباه است.");
                return View(login);
                
            }
            return View(login);
        }

        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string ReturnUrl)
        {
            if (ReturnUrl.Contains("panel")|| ReturnUrl.Contains("Panel"))
            {
                return Redirect("/panel");
            }
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
