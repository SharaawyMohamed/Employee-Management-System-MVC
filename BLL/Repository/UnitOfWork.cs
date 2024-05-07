using BLL.Interfaces;
using DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repository
{
	public class UnitOfWork : IUnitOfWork ,IDisposable
	{
		private readonly MVC2DbContext context;
		public IEmployeeRepository EmployeeRepository { get; set; }
		public IDepartmentRepository DepartmentRepository { get; set; }
		public UnitOfWork(MVC2DbContext _context)// ask CLR For Object from DBcontext
		{
			EmployeeRepository = new EmployeeRepository(_context);
			DepartmentRepository = new DepartmentRepository(_context);
			context = _context;
		}

		public async Task<int> CompleteAsync()
		{
			return await context.SaveChangesAsync();
		}
		public void Dispose()// close connection with database
		{
			context.Dispose();
		}
	}
}
