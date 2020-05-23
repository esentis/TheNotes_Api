namespace Esen.Notes.Web
{
	using System;
	using System.IO;
	using System.Linq;

	using Esen.Notes.Persistence;
	using Esen.Notes.Persistence.Model.Identity;

	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.ResponseCompression;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Diagnostics;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using Microsoft.OpenApi.Models;

	using Serilog;

	using Swashbuckle.AspNetCore.SwaggerUI;

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
			services.AddMvc();

			services.AddRazorPages();

			services.AddResponseCompression(opts =>
			{
				opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
					new[] { "application/octet-stream" });
			});

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

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Notes Api", Version = "v1" });

				var filePath = Path.Combine(AppContext.BaseDirectory, $"{typeof(Startup).Assembly.GetName().Name}.xml");
				c.IncludeXmlComments(filePath);

				c.EnableAnnotations();
				c.DescribeAllParametersInCamelCase();
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

				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseResponseCompression();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.DocumentTitle = "Notes Api";
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Esen - Notes Api v1");

				c.DocExpansion(DocExpansion.None);
				c.EnableDeepLinking();
				c.EnableFilter();
				c.EnableValidator();

				if (!Environment.IsDevelopment())
				{
					return;
				}

				c.DisplayOperationId();
				c.DisplayRequestDuration();
			});

			app.UseAuthorization();
			app.UseSerilogRequestLogging(options => options.EnrichDiagnosticContext =
				(diagnosticContext, httpContext) =>
				{
					diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
					diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
				});

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.MapRazorPages();
			});
		}
	}
}
