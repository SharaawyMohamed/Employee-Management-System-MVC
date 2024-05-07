using BLL.Interfaces;
using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly MVC2DbContext context;// dependency injection to open connection with database
        public GenericRepository(MVC2DbContext _context)
        {
            context = _context;
        }
        public async Task AddAsync(T model)
        {
            await  context.AddAsync(model);
        }

        public void Delete(T model)
        {
            context.Remove(model);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Employee))
             return   (IEnumerable<T>) await context.Employees.Include(E => E.Department).ToListAsync();

            return await context.Set<T>().ToListAsync();
        }
        public async Task<T> GetByIdAsync(int id) =>await context.Set<T>().FindAsync(id);


        public void Update(T model)
        {
            context.Update(model);
        }
    }
}
