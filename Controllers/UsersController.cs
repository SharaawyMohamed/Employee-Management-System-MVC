using AutoMapper;
using DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Demo.PL.Controllers
{
	[Authorize]
	public class UsersController : Controller
	{
		private readonly RoleManager<IdentityRole> rolmanager;
		private readonly UserManager<ApplicationUser> usermanager;
		private readonly IMapper mapper;
		public UsersController(UserManager<ApplicationUser> _usermanager, IMapper _mapper, RoleManager<IdentityRole> _rolmanager)
		{
			usermanager = _usermanager;
			mapper = _mapper;
			rolmanager = _rolmanager;
		}
		public async Task<IActionResult> Index(string email)
		{
			if (string.IsNullOrEmpty(email))
			{
				// convert all usere ---> userviewmodel

				var users = await usermanager.Users.Select(u => new UserViewModel
				{
					Id = u.Id,
					FirstName = u.FirstName,
					LastName = u.LastName,
					Email = u.Email,
					PhoneNumber = u.PhoneNumber,
					Roles = usermanager.GetRolesAsync(u).Result
				}).ToListAsync();

				#region Add-Admin Role
				//for(int i=0;i<users.Count();i++)
				//{
				//	if (users[i].Email== "sharawym275@gmail.com")
				//	{
				//		var RolesList=users[i].Roles.ToList();
				//		if (!RolesList.Contains("Admin"))
				//		{
				//			RolesList.Add("Admin");
				//			users[i].Roles=RolesList;
				//		}
				//	}
				//} 
				#endregion

				return View(users);
			}
			var user = await usermanager.FindByEmailAsync(email);

			if (user is null) return View(Enumerable.Empty<UserViewModel>());

			var maped = new UserViewModel
			{
				Id = user.Id,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email,
				PhoneNumber = user.PhoneNumber,
				Roles = usermanager.GetRolesAsync(user).Result

			};
			return View(new List<UserViewModel> { maped });
		}
		//                                                          default value
		[HttpGet]
		public async Task<IActionResult> Details(string? id, string viewname = "Details")
		{
			if (string.IsNullOrEmpty(id)) { return BadRequest(); }

			var user = await usermanager.FindByIdAsync(id);
			if (user is null) return NotFound();

			var maped = mapper.Map<ApplicationUser, UserViewModel>(user);
			maped.Roles = await usermanager.GetRolesAsync(user);
			return View(viewname, maped);
		}
		[HttpGet]
		public async Task<IActionResult> Edit(string id)
		{
			return await Details(id, nameof(Edit));
		}

		[HttpPost]
		public async Task<IActionResult> Edit(UserViewModel model,[FromRoute] string id)
		{
			if ( id is null ||model.Id != id) return BadRequest();
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			try
			{
				var user = await usermanager.FindByIdAsync(id);
				if (user is null) return NotFound();
				user.FirstName = model.FirstName;
				user.LastName = model.LastName;
				user.PhoneNumber = model.PhoneNumber;

				await usermanager.UpdateAsync(user);
				return RedirectToAction(nameof(Index));
			}
			catch (Exception e)
			{
				ModelState.AddModelError(string.Empty, e.Message);
			}
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Delete(string id)
		{
			return await Details(id, nameof(Delete));
		}
		[HttpPost]
		public async Task<IActionResult> Delete(UserViewModel model,[FromRoute]string id)
		{
			if(model.Id != id || id is null) { return BadRequest(); }
			if (!ModelState.IsValid)
			{
				return View(model);
			}
				var user = await usermanager.FindByIdAsync(id);
			if (user is null) return NotFound();

			try
			{
				await usermanager.DeleteAsync(user);
				return RedirectToAction(nameof(Index));
			}catch(Exception e)
			{
				ModelState.AddModelError(string.Empty,e.Message);
				return RedirectToAction("Error", nameof(HomeController));
			}
		}
		
	}
}
