using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
	public class Department
	{
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }// becuase we in .NET 5.0 the name can be nullable 
		[Required]
		public string Code {  get; set; }
		[Required]
		public DateTime DateOfCreation { get; set; }
		[InverseProperty("Department")]
		public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();


	}
}
