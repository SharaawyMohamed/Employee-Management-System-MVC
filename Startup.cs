using BLL.Interfaces;
using BLL.Repository;
using DAL.Context;
using DAL.Models;
using Demo.PL.MappingProfile;
using Demo.PL.Settings;
using Demo.PL.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
namespace Demo.PL
{
	public class Startup
	{
		public Startup(IConfiguration configuration)//dependancy injection
		{
			Configuration = configuration;// object refere at Appsettings File
		}

		public IConfiguration Configuration { get; }// read only object

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllersWithViews();
			// to make CLR open connection with database directly
			services.AddDbContext<MVC2DbContext>(Options =>
			{
				Options.UseSqlServer(Configuration.GetConnectionString("DefalutConnection"));// Allow Dependency injection in Class IDepartmentRepository
																							 //             life time for service
			}, ServiceLifetime.Scoped);
			// service life time

			#region replace servecies
			// we replace them by unit of work;
			// add services for unit of work
			services.AddScoped<IUnitOfWork, UnitOfWork>();

			services.AddScoped<IDepartmentRepository, DepartmentRepository>();// allow Dependency injection to class Department repository(in dempartment controller)
																			  //services.AddScoped<IEmployeeRepository, EmployeeRepository>();// allow Dependency injection to class Department repository(in employee controller)
			#endregion


			services.AddAutoMapper(config => config.AddProfile(new EmployeeProfile()));// for mapping
			services.AddAutoMapper(config => config.AddProfile(new DepartmentProfile()));// for mapping between Department,DepartmentViewModel
			services.AddAutoMapper(config => config.AddProfile(new UserProfile()));// for mapping between Department,DepartmentViewModel
			services.AddAutoMapper(config => config.AddProfile(new RoleProfile()));// for mapping between Department,DepartmentViewModel
			services.AddAutoMapper(Assembly.GetExecutingAssembly());
			//services.AddScoped<UserManager<ApplicationUser>>();
			// to add servicies to use segnatures wich we need it in User 
			services.AddIdentity<ApplicationUser, IdentityRole>(option =>
			{
				option.Password.RequireNonAlphanumeric = true;
				option.Password.RequireDigit = true;
				option.Password.RequireLowercase = true;
				option.Password.RequireUppercase = true;
			}
			)
			   .AddEntityFrameworkStores<MVC2DbContext>()
			   .AddDefaultTokenProviders();
			// add Servicies to UserManager
			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(option =>
				{
					option.LoginPath = "Account/Login";
					option.AccessDeniedPath = "Home/Error";
				});

			services.AddAuthentication(option =>
			{
				option.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
				option.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
			}).AddGoogle(option =>
				{
					IConfiguration GoogleAuth = Configuration.GetSection("Authentication:LoginWithGoogle");
					option.ClientId = GoogleAuth["ClientId"];
					option.ClientSecret = GoogleAuth["ClientSecret"];
				});
			services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
			services.AddScoped<IMailService, MailService>();
			
			services.Configure<TwilioSettings>(Configuration.GetSection("Twilio"));
			services.AddScoped<ISMS_Service, SMS_Service>();
				
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Account}/{action=Login}/{id?}");
			});
		}
	}
}
