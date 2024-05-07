using AutoMapper;
using DAL.Entities;
using Demo.PL.ViewModels;

namespace Demo.PL.MappingProfile
{
	public class EmployeeProfile : Profile
	{
		public EmployeeProfile()
		{
			#region Simple Mappint
             CreateMap<EmployeeViewModel,Employee>().ReverseMap();
			#endregion
			#region Mapping if two objects have deferent names
			//   CreateMap<EmployeeViewModel, Employee>()
			//.ForMember(destination => destination.Id, option => option.MapFrom(source => source.Id))
			//.ForMember(destination => destination.Name, option => option.MapFrom(source => source.Name))
			//.ForMember(destination => destination.Age, option => option.MapFrom(source => source.Age))
			//.ForMember(destination => destination.Address, option => option.MapFrom(source => source.Address))
			//.ForMember(destination => destination.Salary, option => option.MapFrom(source => source.Salary))
			//.ForMember(destination => destination.Email, option => option.MapFrom(source => source.Email))
			//.ForMember(destination => destination.Phone, option => option.MapFrom(source => source.Phone))
			//.ForMember(destination => destination.HireDate, option => option.MapFrom(source => source.HireDate))
			//.ForMember(destination => destination.IsActive, option => option.MapFrom(source => source.IsActive))
			//.ForMember(destination => destination.Department, option => option.MapFrom(source => source.Department))
			//.ForMember(destination => destination.DepartmentId, option => option.MapFrom(source => source.DepartmentId));
			#endregion
		}

	}
}
