using BLL.Interfaces;
using DAL.Context;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repository
{
	public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
	{
		private readonly MVC2DbContext context;

		public EmployeeRepository(MVC2DbContext _context) : base(_context)// Chan on base class 
		{
			context = _context;
		}

		public async Task<IEnumerable<Employee>> GetEmployeeByAddress(string address)
			 => context.Employees.Where(e => e.Address == address);

		public async Task<IEnumerable<Employee>> SearchByName(string name)
		=> context.Employees.Where(i => i.Name.Contains(name));

	}
}
