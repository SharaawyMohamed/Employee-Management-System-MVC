using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
	public interface IUnitOfWork:IDisposable
	{
		public IEmployeeRepository EmployeeRepository { get; set; }
	    public IDepartmentRepository DepartmentRepository { get; set; }
		public Task<int> CompleteAsync();
		public void Dispose();
	}
}
