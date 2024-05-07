using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel;

namespace Demo.PL.ViewModels
{
	public class RoleViewModel
	{
		
		[ValidateNever]
		public string Id { get; set; }

		[DisplayName("Role Name")]
		public string Name { get; set; }

		public RoleViewModel()
		{
			Id=Guid.NewGuid().ToString(); 
		}
	}
}
