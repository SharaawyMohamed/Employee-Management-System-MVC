using AutoMapper;
using BLL.Interfaces;
using DAL.Entities;
using Demo.PL.Utility;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
	[Authorize]
	public class EmployeeController : Controller
	{
		private readonly IUnitOfWork unitofwork;
		private readonly IMapper mapper;

		public EmployeeController(IUnitOfWork _unitofwork, IMapper _mapper)
		{
			unitofwork = _unitofwork;
			mapper = _mapper;
		}

		public async Task<IActionResult> Index(string? searchvalue)
		{
			if (string.IsNullOrEmpty(searchvalue))
			{
				var employees = await unitofwork.EmployeeRepository.GetAllAsync();
				if (employees is null) return NotFound();

				var mapedemp = mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
				
				return View(mapedemp);
			}
			var res = await unitofwork.EmployeeRepository.SearchByName(searchvalue);
			if (res is null)
				return NotFound();

			var mapedemployees = mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(res);
			//ViewBag.SearchValue = mapedemployees;// I'm not remember way i put this line here
			return View(mapedemployees);
		}
		[HttpGet]
		public async Task<IActionResult> Create()
		{
			ViewBag.Departments = await unitofwork.DepartmentRepository.GetAllAsync();
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(EmployeeViewModel employee)
		{
			if (ModelState.IsValid)
			{
				try
				{
					employee.ImageName = DocumentSetting.UploadFile(employee.Image, "Images");
					var mapedemployee = mapper.Map<EmployeeViewModel, Employee>(employee);

					await unitofwork.EmployeeRepository.AddAsync(mapedemployee);
					await unitofwork.CompleteAsync();
					TempData["Message"] = "Employee Created Successfully";
				}
				catch (Exception e)
				{
					ModelState.AddModelError(string.Empty, e.Message);
				}
				return RedirectToAction(nameof(Index));
			}

			return View(employee);
		}
		[HttpGet]
		public async Task<IActionResult> Details(int? id)
		{
			if (id is not null)
			{
				var employee = await unitofwork.EmployeeRepository.GetByIdAsync(id.Value);
				if (employee is not null)
				{
					return View(mapper.Map<Employee, EmployeeViewModel>(employee));
				}
				return NotFound();
			}
			return BadRequest();
		}
		[HttpGet]
		public async Task<IActionResult> Edit(int? id)
		{
			if (id is null) return BadRequest();

			var employee = await unitofwork.EmployeeRepository.GetByIdAsync(id.Value);
			if (employee is null) { return NotFound(); }
			TempData["ImageName"] = employee.ImageName;
			var departments = await unitofwork.DepartmentRepository.GetAllAsync();
			ViewBag.Departments = mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(departments);
			return View(mapper.Map<Employee, EmployeeViewModel>(employee));
		}
		[HttpPost]
		public async Task<IActionResult> Edit(EmployeeViewModel employee, [FromRoute] int? id)
		{
			if (id is null || id != employee.Id) { return BadRequest(); }
			try
			{
				string ImageName = TempData["ImageName"] as string;
				if(ImageName is not null)
				{
					DocumentSetting.DeleteFile("Images", ImageName);
				}
				employee.ImageName = DocumentSetting.UploadFile(employee.Image, "Images");
				var mapedemployee = mapper.Map<EmployeeViewModel, Employee>(employee);
				unitofwork.EmployeeRepository.Update(mapedemployee);
				await unitofwork.CompleteAsync();
				TempData["Message"] = "Employee Updated Successfully";
			}
			catch (Exception e)
			{
				ModelState.AddModelError(string.Empty, e.Message);
			}
			return RedirectToAction(nameof(Index));
		}
		[HttpGet]
		public async Task<IActionResult> Delete(int? id)
		{
			if (id is null) return BadRequest();
			var employee = await unitofwork.EmployeeRepository.GetByIdAsync(id.Value);
			if (employee is null) { return NotFound(); }
			TempData["ImageName"]=employee.ImageName;
			return View(mapper.Map<Employee, EmployeeViewModel>(employee));
		}
		[HttpPost]
		public async Task<IActionResult> Delete(EmployeeViewModel employee)
		{
			if (!ModelState.IsValid) { return BadRequest(); }
			try
			{
				var mapedemployee = mapper.Map<EmployeeViewModel, Employee>(employee);
				unitofwork.EmployeeRepository.Delete(mapedemployee);
				await unitofwork.CompleteAsync();
				DocumentSetting.DeleteFile("Images", TempData["ImageName"]as string);
				TempData["Message"] = "Employee Deleted Successfully";
				return RedirectToAction(nameof(Index));

			}
			catch (Exception e)
			{
				ModelState.AddModelError(string.Empty, e.Message);
			}
			return RedirectToAction(nameof(Index));
		}
	}
}
