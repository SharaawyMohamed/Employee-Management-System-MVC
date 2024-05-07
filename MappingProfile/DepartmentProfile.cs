using AutoMapper;
using DAL.Entities;
using Demo.PL.ViewModels;
using System.Runtime.InteropServices;

namespace Demo.PL.MappingProfile
{
	public class DepartmentProfile:Profile
	{
		public DepartmentProfile() {
		CreateMap<Department,DepartmentViewModel>().ReverseMap(); 
		}
	}
}
