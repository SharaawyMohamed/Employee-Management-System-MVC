using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
	public class Employee
	{
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public int Age { get; set; }
		
		[Required]
		public string Address { get; set; }

		[Required]
		[DataType(DataType.Currency)]
		public decimal Salary { get; set; }
		[Required]
		public string Email { get; set; }
		public string Phone { get; set; }
		public string ImageName { get; set; }
		public DateTime HireDate { get;set; }
		public DateTime CreationDate { get; set; } = DateTime.Now;
		public bool IsActive { get; set; }

		public Department Department { get; set; }
		[ForeignKey("Department")]
		public int? DepartmentId { get; set; }
	}
}
