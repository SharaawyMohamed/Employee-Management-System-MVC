using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
	public interface IGenericRepository<T>
	{
		public Task<IEnumerable<T>> GetAllAsync();
		public Task<T> GetByIdAsync(int id);
		Task  AddAsync(T model);
		void  Update(T model);
		void  Delete(T model);
	}
}
