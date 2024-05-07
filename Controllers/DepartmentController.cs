using AutoMapper;
using BLL.Interfaces;
using DAL.Entities;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
	[Authorize]
	public class DepartmentController : Controller
	{
		private readonly IUnitOfWork unitofwork;
		private readonly IMapper mapper;

		// Q) way we take object of interface not of class? (A) because in stage testing 
		public DepartmentController(IUnitOfWork _unitofwork, IMapper _mapper)// ask CLR for creating object from class implement interface IDepartment
		{
			unitofwork = _unitofwork;
			mapper = _mapper;
		}

		public async Task<IActionResult> Index()
		{
			var Departments = await unitofwork.DepartmentRepository.GetAllAsync();
			var MapedDepartments = mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(Departments);
			return View(MapedDepartments);
		}
		[HttpGet]// this is a default for any Action
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]// use HttpPost because the data which would submit as (Post)
		public async Task<IActionResult> Create(DepartmentViewModel department)
		{
			if (ModelState.IsValid)// server side validation
			{
				try
				{

					await unitofwork.DepartmentRepository.AddAsync(mapper.Map<DepartmentViewModel, Department>(department));
					await unitofwork.CompleteAsync();
					TempData["Message"] = "Department Creaded Successfully";
				}
				catch (Exception e)
				{
					ModelState.AddModelError(string.Empty, e.Message);
				}
				return RedirectToAction(nameof(Index));// 
			}
			return View(department);// if data endered by non correct way show data error for user
		}
		[HttpGet]
		public async Task<IActionResult> ReturnDepartmentView(int? id, string viewname)
		{
			if (id is null)
				return BadRequest();

			var department = await unitofwork.DepartmentRepository.GetByIdAsync(id.Value);

			if (department is null)
				return NotFound();

			return View(viewname, mapper.Map<Department, DepartmentViewModel>(department));
		}
		public async Task<IActionResult> Details(int? id) => await ReturnDepartmentView(id, nameof(Details));

		[HttpGet]
		public async Task<IActionResult> Edit(int? id) => await ReturnDepartmentView(id, nameof(Edit));

		[HttpPost]
		[ValidateAntiForgeryToken]// for security bcause no any tool can reach for my Action
		public async Task<IActionResult> Edit(DepartmentViewModel department, [FromRoute] int? id)
		{
			if (department.Id != id || id is null)
				return BadRequest();

			if (ModelState.IsValid)
			{
				try
				{
					unitofwork.DepartmentRepository.Update(mapper.Map<DepartmentViewModel, Department>(department));
					await unitofwork.CompleteAsync();
					TempData["Message"] = "Department Updated Successfully";
					return RedirectToAction(nameof(Index));
				}
				catch (Exception e)
				{
					// 1. log Exception: that mean show anything then recorde exception and send exception to support team
					// 2. show exception in my page this way
					//                             key    ,  error message
					ModelState.AddModelError(string.Empty, e.Message);
				}
			}
			return View(department);
		}

		[HttpGet]
		public async Task<IActionResult> Delete(int? id) => await ReturnDepartmentView(id, nameof(Delete));

		[HttpPost]
		[ValidateAntiForgeryToken]// for security bcause no any tool can reach for my Action
		public async Task<IActionResult> Delete(DepartmentViewModel department, [FromRoute] int? id)
		{
			if (id is null || department.Id != id)
				return BadRequest();

			if (ModelState.IsValid)
			{
				try
				{
					var mappeddepartment = mapper.Map<DepartmentViewModel, Department>(department);
					unitofwork.DepartmentRepository.Delete(mappeddepartment);
					await unitofwork.CompleteAsync();
					TempData["Message"] = "Department Deleted Successfully";
				}
				catch
				{
					TempData["Message"] = "You Can't Delete This Department Because It Hase Employees ";
					//ModelState.AddModelError(string.Empty, e.Message);
				}
				return RedirectToAction(nameof(Index));

			}
			return View();
		}
	}
}

