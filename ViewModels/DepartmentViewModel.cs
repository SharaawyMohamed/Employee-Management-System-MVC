using DAL.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
namespace Demo.PL.ViewModels
{
	public class DepartmentViewModel
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "Name is required")]
		public string Name { get; set; }   // becuase we in .NET 5.0 the name can be nullable 
		[Required(ErrorMessage = "Code is required")]
		public string Code { get; set; }
		[Required(ErrorMessage = "Date of creation is required")]
		public DateTime DateOfCreation { get; set; }
		[InverseProperty("Department")]
		public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
	}
}
