using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Demo.PL.ViewModels;
namespace Demo.PL.MappingProfile
{
	public class RoleProfile:Profile
	{
		public RoleProfile()
		{                                                        //  Reverce :   
			CreateMap<IdentityRole, RoleViewModel>().ReverseMap();// convert two directions between (Employee, EmployeeViewModel)
		}
	}
}
