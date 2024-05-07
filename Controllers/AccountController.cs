using DAL.Entities;
using DAL.Models;
using Demo.Pl.ViewModels;
using Demo.PL.Utility;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
namespace Demo.PL.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<ApplicationUser> usermanager;
		private readonly SignInManager<ApplicationUser> signinmanager;
		private readonly IMailService mailservice;
		private readonly ISMS_Service sMSservice;

		public AccountController(UserManager<ApplicationUser> _usermanager,SignInManager<ApplicationUser> _signinmanager,IMailService _mailservice,ISMS_Service _SMSservice)
		{
			usermanager = _usermanager;
			signinmanager = _signinmanager;
			mailservice = _mailservice;
			sMSservice = _SMSservice;
		}

		#region Register
		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel registerviewmodel)
		{
			if (!ModelState.IsValid)
				return View(registerviewmodel);

			var user = new ApplicationUser()
			{
				FirstName = registerviewmodel.FirstName,
				LastName = registerviewmodel.LastName,
				Email = registerviewmodel.Email,
				IsAgree = registerviewmodel.IsAgree,
				UserName = registerviewmodel.Email.Split('@')[0]

			};
			var res = await usermanager.CreateAsync(user, registerviewmodel.Password);
			if (res.Succeeded)
			{
				//TempData["Message"] = $"Welcome, {user.FirstName + " " + user.LastName} .";
				return RedirectToAction(nameof(Login));
			}

			foreach (var error in res.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}
			return View(registerviewmodel);
		}
		#endregion

		#region Login
		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				// server side validation
				// we have three steps
				// 1. find user by email
				var user = await usermanager.FindByEmailAsync(model.Email);
				if (user is not null)
				{   //	 chek password	
					if (await usermanager.CheckPasswordAsync(user, model.Password))
					{
						// Login --> signinManager                             //     --+
						var res = await signinmanager                                //   V if user chice option keep { me sign in }
								.PasswordSignInAsync(user, model.Password, model.Remember, false);
						// if successful login operation
						if (res.Succeeded)
							return RedirectToAction("Index", "Home");	
					}
					else
						ModelState.AddModelError(string.Empty, "Incorrect Password");
				}
				else
					ModelState.AddModelError(string.Empty, "Email Not Found");
				
			}
			return View(model);
		}

		// Login With Google
		public IActionResult LoginWithGoogle()
		{
			var prop = new AuthenticationProperties()
			{
				RedirectUri = Url.Action("GoogleResponse")
			};
			return Challenge(prop,GoogleDefaults.AuthenticationScheme);
		}

		public async Task<IActionResult> GoogleResponse()
		{
			var Res =await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
			var claims = Res.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
			{
				claim.Issuer,
				claim.OriginalIssuer,
				claim.Type,
				claim.Value
			});

			return RedirectToAction("Index","Home");
		}
		#endregion

		#region Sing Out
		[HttpGet]
		public new async Task<IActionResult> SignOut()
		{
			await signinmanager.SignOutAsync();
			return RedirectToAction(nameof(Login));
		}
		#endregion

		#region Forget Password

		public IActionResult ForgetPassword()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> SendEmail(ForgetPasswordViewModel model)
		{
			if (!ModelState.IsValid) return View(nameof(ForgetPassword),model);// server side validation

			// 1. Get User By Email
			var user = await usermanager.FindByEmailAsync(model.Email);

			// 2. Reset Password
			if (user is not null)
			{
				// generate Token link => baseURL/Account/ForgetPassword?Email=EmailValue&Token=TokenValue
				var Token = await usermanager.GeneratePasswordResetTokenAsync(user);

				// generate Password reste link => baseURL/Account/ForgetPassword?Email=EmailValue&Token=TokenValue
				//                      Action          Controller
				var URL = Url.Action("ResetPassword", "Account", new { email = model.Email, token = Token }, Request.Scheme);
				var email = new Email
				{
					To = model.Email,
					Subject = "Reset Password",
					// body is the password rest Link
					Body = URL,
				};

				// 3. Send Email User have 
				EmailSettings.SendEmail(email);

				return RedirectToAction(nameof(CheckYourInBox));
			}
			ModelState.AddModelError(string.Empty, "Email Not Found");
			return View(nameof(ForgetPassword));
		}

		// Best and Fast Way:
		public async Task<IActionResult> SendEmailAdvanced(ForgetPasswordViewModel model)
		{
			if (!ModelState.IsValid) return View(nameof(ForgetPassword), model);// server side validation

			// 1. Get User By Email
			var user = await usermanager.FindByEmailAsync(model.Email);

			// 2. Reset Password
			if (user is not null)
			{
				// generate Token link => baseURL/Account/ForgetPassword?Email=EmailValue&Token=TokenValue
				var Token = await usermanager.GeneratePasswordResetTokenAsync(user);

				// generate Password reste link => baseURL/Account/ForgetPassword?Email=EmailValue&Token=TokenValue
				//                      Action          Controller
				var URL = Url.Action("ResetPassword", "Account", new { email = model.Email, token = Token }, Request.Scheme);
				var email = new Email
				{
					To = model.Email,
					Subject = "Reset Password",
					// body is the password rest Link
					Body = URL,
				};

				// 3. Send Email User have 
				//EmailSettings.SendEmail(email);
				mailservice.SendMail(email);

				return RedirectToAction(nameof(CheckYourInBox));
			}
			ModelState.AddModelError(string.Empty, "Email Not Found");
			return View(nameof(ForgetPassword));
		}
		public async Task<IActionResult> SendEmailSMS(ForgetPasswordViewModel model)
		{
			if (!ModelState.IsValid) return View(nameof(ForgetPassword), model);// server side validation

			// 1. Get User By Email
			var user = await usermanager.FindByEmailAsync(model.Email);

			// 2. Reset Password
			if (user is not null)
			{
				// generate Token link => baseURL/Account/ForgetPassword?Email=EmailValue&Token=TokenValue
				var Token = await usermanager.GeneratePasswordResetTokenAsync(user);

				// generate Password reste link => baseURL/Account/ForgetPassword?Email=EmailValue&Token=TokenValue
				//                      Action          Controller
				var URL = Url.Action("ResetPassword", "Account", new { email = model.Email, token = Token }, Request.Scheme);
				var email = new Email
				{
					To = model.Email,
					Subject = "Reset Password",
					// body is the password rest Link
					Body = URL,
				};

				// 3. Send Email User have 
				//EmailSettings.SendEmail(email);
				//mailservice.SendMail(email);
				var sms = new SMS()
				{
					Body=URL,
					PhoneNumber=user.PhoneNumber,
				};
				sMSservice.SendSMS(sms);

				return RedirectToAction(nameof(CheckYourInBox));
			}
			ModelState.AddModelError(string.Empty, "Email Not Found");
			return View(nameof(ForgetPassword));
		}

		[HttpGet]
		public IActionResult CheckYourInBox()
		{
			return View();
		}
		#endregion

		#region Reset Password
		[HttpGet]
		public IActionResult ResetPassword(string email, string token)
		{
			TempData["email"] = email;
			TempData["token"] = token;
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (!ModelState.IsValid) { return View(model); }

			var email = TempData["email"] as string;
			var token = TempData["token"] as string;

			var user = await usermanager.FindByEmailAsync(email);
			// rest password 
			if (user is not null)
			{
				var res = await usermanager.ResetPasswordAsync(user, token, model.Password);
				if (res.Succeeded)
				{
					return RedirectToAction(nameof(Login));
				}

				foreach (var item in res.Errors)
				{
					ModelState.AddModelError(string.Empty, item.Description);
				}

			}
			return View(model);
		}
		#endregion

		#region AccessDenied
		public IActionResult AccessDenied()
		{
			return View();
		}
		#endregion
	}
}
