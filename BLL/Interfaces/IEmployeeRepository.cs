using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
	public interface IEmployeeRepository:	IGenericRepository<Employee>
	{
		public Task<IEnumerable<Employee>> GetEmployeeByAddress(string address);// I would implement it
		public Task<IEnumerable<Employee>> SearchByName(string name);
	}
}
