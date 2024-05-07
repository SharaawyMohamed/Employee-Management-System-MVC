using AutoMapper;
using DAL.Entities;
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
	[Authorize(Roles = "Admin")]
	//[Authorize(Email ="sharawym275@gmail.com")]
	public class RolesController : Controller
	{
		private readonly RoleManager<IdentityRole> rolemanager;
		private readonly IMapper mapper;
		private readonly UserManager<ApplicationUser> usermanager;
		public RolesController(RoleManager<IdentityRole> _rolmanager, IMapper _mapper,UserManager<ApplicationUser> _usermanager)
		{
			rolemanager = _rolmanager;
			mapper = _mapper;
			usermanager = _usermanager;
		}

		public async Task<IActionResult> Index(string email)
		{
			if (string.IsNullOrEmpty(email))
			{
				// convert all usere ---> userviewmodel

				var roles = await rolemanager.Roles.ToListAsync();
				var mapedrole = mapper.Map<IEnumerable<IdentityRole>,IEnumerable<RoleViewModel>>(roles);
				return View(mapedrole);
			}
			var role = (await rolemanager.FindByNameAsync(email));
			if (role is null) return View(Enumerable.Empty<RoleViewModel>());

			var maped = mapper.Map<IdentityRole,RoleViewModel>(role);
			
			return View(new List<RoleViewModel>() { maped });
		}
		//                                                          default value
		public async Task<IActionResult> Details(string id, string viewname = "Details")
		{
			if (string.IsNullOrEmpty(id)) { return BadRequest(); }

			var role = await rolemanager.FindByIdAsync(id);
			if (role is null) return NotFound();

			var maped = mapper.Map<IdentityRole, RoleViewModel>(role);
			return View(viewname, maped);
		}
		[HttpGet]
		public async Task<IActionResult> Edit(string id)
		{
			return await Details(id, nameof(Edit));
		}

		[HttpPost]
		public async Task<IActionResult> Edit(RoleViewModel model, string id)
		{
			if (model.Id != id || id is null) return BadRequest();
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			try
			{
				var role = await rolemanager.FindByIdAsync(id);
				if (role is null) return NotFound();
				role.Name = model.Name;

				await rolemanager.UpdateAsync(role);
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
		public async Task<IActionResult> ConfermDelete(RoleViewModel model)
		{
			if (!ModelState.IsValid) return BadRequest();
			try
			{
				var role = await rolemanager.FindByIdAsync(model.Id);
				if (role is null) return NotFound();
				await rolemanager.DeleteAsync(role);
				return RedirectToAction(nameof(Index));
			}
			catch (Exception e)
			{
				ModelState.AddModelError(string.Empty, e.Message);
			}
			return View();
		}
		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Create(RoleViewModel model)
		{
			if (ModelState.IsValid)
			{
				var maped = mapper.Map<RoleViewModel,IdentityRole>(model);

				var res = await rolemanager.CreateAsync(maped);
				if (res.Succeeded) return RedirectToAction(nameof(Index));

				foreach (var i in res.Errors)
					ModelState.AddModelError(string.Empty, i.Description);
			}
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> AddOrRemoveUser(string roleId)
		{
			var role = await rolemanager.FindByIdAsync(roleId);
			if (role is null) return NotFound();

			var usersinrole = new List<UsersInRoleViewModel>();
			var users = await usermanager.Users.ToListAsync();

			foreach (var user in users)
			{
				usersinrole.Add(new UsersInRoleViewModel
				{
					Id = user.Id,
					Name = user.UserName,
					IsInRole = await usermanager.IsInRoleAsync(user, role.Name),// have bool value true or false
				}) ;
			}
			ViewBag.RoleId = roleId;
			return View(usersinrole);
		}

		[HttpPost]
		public async Task<IActionResult> AddOrRemoveUser(string roleId,List<UsersInRoleViewModel> users)
		{
			var role = await rolemanager.FindByIdAsync(roleId);
			if (role is null) return NotFound();

			if(ModelState.IsValid)
			{
				foreach (var user in users)
				{
					var userbeforeidt = await usermanager.FindByIdAsync(user.Id);
					if(ModelState.IsValid)
					{

						bool RoleIsFound = await usermanager.IsInRoleAsync(userbeforeidt, role.Name);
						if (user.IsInRole == true && !RoleIsFound)
						{
							await usermanager.AddToRoleAsync(userbeforeidt, role.Name);
						}
						else if (user.IsInRole == false && RoleIsFound)
						{
							await usermanager.RemoveFromRoleAsync(userbeforeidt, role.Name);
						}

					}
				}

				return RedirectToAction(nameof(Edit),new {Id= roleId });

			}
			return View(users);
		}
	}
}

