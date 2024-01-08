using Company.DAL.Models;
using Company.PL.Helpers;
using Company.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Threading.Tasks;

namespace Company.PL.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUser> userManager;
		private readonly SignInManager<ApplicationUser> signInManager;

		public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
			this.userManager = userManager;
			this.signInManager = signInManager;
		}
		#region Actions Register
		public IActionResult Register()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
                var use = await userManager.FindByEmailAsync(model.Email);
				if(use.Email.ToLower()==model.Email.ToLower())
				{
                    ModelState.AddModelError(string.Empty, "Email is already Exist");
					return View(model);
                }

                var user = new ApplicationUser
				{
					UserName = model.Email.Split('@')[0],
					Email = model.Email,
					FName = model.FName,
					LName = model.LName,
					IsAgree = model.IsAgree
				};
              
                var result = await userManager.CreateAsync(user, model.Password);
				if (result.Succeeded)
				{
					return RedirectToAction("Login");
				}
				else
					foreach (var error in result.Errors)
						ModelState.AddModelError(string.Empty, error.Description);
			}
			return View(model);
		}
		#endregion

		#region Actions Login
		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await userManager.FindByEmailAsync(model.Email);
				if (user is not null)
				{
					var flag = await userManager.CheckPasswordAsync(user, model.Password);
					if (flag)
					{
						var result = await signInManager.PasswordSignInAsync(user, model.Password, model.RemenberMe, false);
						if (result.Succeeded)
						{
							return RedirectToAction("Index", "Home");
						}
					}
					else
					{
						ModelState.AddModelError(string.Empty, "Password is InCorrect");
					}

				}
				else
				{
					ModelState.AddModelError(string.Empty, "Email Not Exist");
				}

			}
			return View(model);
		}
		#endregion

		#region Action Logout
		public new async Task<IActionResult> SignOut()
		{
			await signInManager.SignOutAsync();
			return RedirectToAction(nameof(Login));
		} 
		#endregion

		public IActionResult ForgetPassword()
		{
			return View();
		}

		public async Task<IActionResult> SendEmail(ForgetPasswordViewModel model)
		{   
			if(ModelState.IsValid)
			{var User =await userManager.FindByEmailAsync(model.Email);
				if (User is not null)
				{
					var token = await userManager.GeneratePasswordResetTokenAsync(User);
					var ReseltPasswordLink = Url.Action("ResetPassword", "Account", new { Email = model.Email, Token = token }, Request.Scheme);
					var email = new Email()
					{
						Subject = "ResetYourPassword",
						To = model.Email,
						Body = ReseltPasswordLink,
					};
					EmailSetting.SendEmail(email);
					return RedirectToAction(nameof(CheckYourInbox));
				}
				else
					ModelState.AddModelError(string.Empty, "Email is Not Exist");
			}
			return View("ForgetPassword", model);
        }

        public IActionResult CheckYourInbox()
		{
			return View();
		}
        public IActionResult ResetPassword(string Email,string Token)
		{
			TempData["email"]=Email;
			TempData["token"]=Token;
            return View();
        }
		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if(ModelState.IsValid)
			{
				string email = TempData["email"]as string;
				string token = TempData["token"]as string;	
				var user=await userManager.FindByEmailAsync(email);
				var result= await userManager.ResetPasswordAsync(user, token,model.NewPassword);
				if(result.Succeeded)
				{
					return RedirectToAction(nameof(Login));
				}
				else
				{
					foreach(var error in result.Errors)
					{
						ModelState.AddModelError(string.Empty, error.Description);
					}
				}
			}
            return View(model);

        }
    }
}
