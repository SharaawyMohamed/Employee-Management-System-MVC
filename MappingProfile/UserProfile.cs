using AutoMapper;
using DAL.Models;
using Demo.PL.ViewModels;
namespace Demo.PL.MappingProfile
{
	public class UserProfile:Profile
	{
		public UserProfile()
		{                                                        //  Reverce :   
			CreateMap<ApplicationUser,UserViewModel>().ReverseMap();// convert two directions between (Employee, EmployeeViewModel)
		}
	}
}
