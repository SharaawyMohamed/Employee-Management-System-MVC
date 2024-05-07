using DAL.Entities;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Context
{
	public class MVC2DbContext : IdentityDbContext<ApplicationUser>
	{
		public MVC2DbContext(DbContextOptions<MVC2DbContext> option):base(option) { }

		//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)// becasue 
		//{                                                                              // if we need execute more than one query in one erquest
		//	optionsBuilder.UseSqlServer("server=.; database=MVC2; trusted_connection=true; MultipleActiveResultSets=true");
		//	base.OnConfiguring(optionsBuilder);
		//}
		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<Employee>()
				.Property(p => p.Salary)
				.HasColumnType("decimal(18,2)");
			base.OnModelCreating(builder);
		}
		public DbSet<Department> Departments { get; set; }
		public DbSet<Employee> Employees { get; set; }
		
	}	
}
