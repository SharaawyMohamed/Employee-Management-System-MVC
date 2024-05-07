using DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;

namespace Demo.PL.ViewModels
{
	public class EmployeeViewModel
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Name is Required")]
		public string Name { get; set; }
		public IFormFile Image { get; set; }
		public string ImageName { get; set; }

		[Required(ErrorMessage = "Age is Required")]
		public int Age { get; set; }

		[Required(ErrorMessage = "Address is Required")]
		public string Address { get; set; }

		[DataType(DataType.Currency)]
		[Required(ErrorMessage = "Salary is Required")]
		public decimal Salary { get; set; }
		[EmailAddress]
		[Required(ErrorMessage = "Email is Required")]
		public string Email { get; set; }
		[Phone]
		public string Phone { get; set; }

		public DateTime HireDate { get; set; }
		public bool IsActive { get; set; }

		public Department Department { get; set; }
		[ForeignKey("Department")]
		public int? DepartmentId { get; set; }
	}
}
