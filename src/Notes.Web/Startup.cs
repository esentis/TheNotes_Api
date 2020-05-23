namespace Esen.Notes.Web
{
	using Esen.Notes.Persistence;
	using Esen.Notes.Persistence.Model.Identity;

	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Diagnostics;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;

	public sealed class Startup
	{
		public Startup(IConfiguration configuration, IWebHostEnvironment environment)
		{
			Configuration = configuration;
			Environment = environment;
		}

		public IConfiguration Configuration { get; }

		private IWebHostEnvironment Environment { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddRazorPages();

			services.AddMvc();

			services.AddDbContext<NotesContext>(options => options
				.UseNpgsql(
					Configuration.GetConnectionString("NotesDb"),
					npgsql => npgsql.EnableRetryOnFailure())
				.EnableDetailedErrors(Environment.IsDevelopment())
				.EnableSensitiveDataLogging(Environment.IsDevelopment())
				.ConfigureWarnings(warn => warn
					.Ignore(CoreEventId.SensitiveDataLoggingEnabledWarning)
					.Log(RelationalEventId.QueryPossibleUnintendedUseOfEqualsWarning)));

			services.AddIdentity<NotesUser, NotesRole>()
				.AddEntityFrameworkStores<NotesContext>()
				.AddDefaultTokenProviders()
				.AddSignInManager();
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

				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapRazorPages();
			});
		}
	}
}
