using BookingMeet.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.SpaServices.AngularCli;

namespace BookingMeet
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}
		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));

			services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();

			services.AddAuthentication()
			.AddCookie(options =>
			{
				options.Cookie.SameSite = SameSiteMode.None;
			})
			.AddGoogle("Google", options =>
			{
				options.ClientId = "652368834323-kchmgulul1qok09kuadov9to3mhe3qqa.apps.googleusercontent.com";
				options.ClientSecret = "7qI6F9X71opU03iWrfifqFGC";
				options.Events = new OAuthEvents
				{
					OnRemoteFailure = (RemoteFailureContext context) =>
					{
						context.Response.Redirect("/home/denied");
						context.HandleResponse();
						return Task.CompletedTask;
					}
				};
			});

			services.AddControllersWithViews();
			services.AddSpaStaticFiles(configuration =>
			{
				configuration.RootPath = ".../.../wwwroot";
			});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseDefaultFiles();
			app.UseCookiePolicy(new CookiePolicyOptions
			{
				MinimumSameSitePolicy = SameSiteMode.None
			});
			app.UseAuthentication();
			app.UseRouting();

			if (!env.IsDevelopment())
			{
				app.UseSpaStaticFiles();
			}

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller}/{action=Index}/{id?}");
			});

			app.UseSpa(spa =>
			{
				spa.Options.SourcePath = "ClientApp";
				if (env.IsDevelopment())
				{
					spa.UseAngularCliServer(npmScript: "start");
					//spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
				}
			});
		}
	}
}
