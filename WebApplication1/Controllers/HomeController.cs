using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public HomeController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_userManager.Users.ToList());
        }

        public async Task<IActionResult> RefactorUser(string[] userIds, Func<User, Task> handler)
        {
            if (userIds != null)
            {
                var userExist = false;
                foreach (var userId in userIds)
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        await handler(user);
                        var refactoredUser = await _userManager.FindByIdAsync(user.Id);
                        if (User.Identity?.Name == user.UserName && (user.LockoutEnabled || refactoredUser == null))
                        {
                            userExist = true;
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "User not found!");
                    }

                    if (userExist)
                    {
                        await _signInManager.SignOutAsync();
                        return RedirectToAction("Login", "Account");
                    }
                }
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BlockUser(string[] userIds)
        {
            return await RefactorUser(userIds, async user =>
            {
                user.LockoutEnabled = true;
                user.LockoutEnd = DateTimeOffset.Now.AddMinutes(1);
                await _userManager.UpdateAsync(user);
                await _userManager.UpdateSecurityStampAsync(user);
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnblockUser(string[] userIds)
        {
            return await RefactorUser(userIds, async user =>
            {
                user.LockoutEnabled = false;
                user.LockoutEnd = DateTimeOffset.Now;
                await _userManager.UpdateAsync(user);
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string[] userIds)
        {
            return await RefactorUser(userIds, async user => { await _userManager.DeleteAsync(user); });
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}