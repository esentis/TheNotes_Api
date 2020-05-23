namespace Esen.Notes.Web
{
	using System;
	using System.IO;
	using System.Reflection;

	using Microsoft.AspNetCore.Hosting;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;

	using Serilog;
	using Serilog.Core;
	using Serilog.Events;
	using Serilog.Sinks.SystemConsole.Themes;

	public static class Program
	{
		private static readonly LoggingLevelSwitch LevelSwitch = new LoggingLevelSwitch();

		public static void Main(string[] args)
		{
			Log.Logger = new LoggerConfiguration()
				.ApplyStartingConfiguration()
				.CreateLogger();

			IHost? host = null;
			try
			{
				host = CreateHostBuilder(args).Build();
			}
			catch (Exception e)
			{
				Log.Fatal(e, "Unhandled exception: {Message}", e.Message);
				throw;
			}

			var configuration = host.Services.GetRequiredService<IConfiguration>();
			var environment = host.Services.GetRequiredService<IWebHostEnvironment>();

			Log.Logger = new LoggerConfiguration()
				.ApplyActualLoggerConfiguration(configuration, environment)
				.CreateLogger();

			try
			{
				host?.Run();
			}
			finally
			{
				Log.CloseAndFlush();
			}
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				})
				.ConfigureAppConfiguration((context, config) => config
					.SetBasePath(context.HostingEnvironment.ContentRootPath)
					.AddJsonFile("appsettings.json", false, true)
					.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true, true)
					.AddJsonFile("connectionStrings.json", true, true)
					.AddJsonFile("appsettings.Local.json", true, true)
					.AddEnvironmentVariables()
					.AddCommandLine(args))
				.UseSerilog();

		private static LoggerConfiguration ApplyStartingConfiguration(this LoggerConfiguration logger)
			=> logger
				.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
				.MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Information)
				.MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
				.MinimumLevel.Override("System", LogEventLevel.Error)
				.Enrich.WithProperty("Application", Assembly.GetAssembly(typeof(Program))?.GetName().Name)
				.Enrich.FromLogContext()
				.WriteTo.Console(theme: AnsiConsoleTheme.Code, standardErrorFromLevel: LogEventLevel.Verbose)
				.WriteTo.Debug();

		private static LoggerConfiguration ApplyActualLoggerConfiguration(
			this LoggerConfiguration logger,
			IConfiguration configuration,
			IHostEnvironment environment)
			=> logger
				.ApplyStartingConfiguration()
				.Enrich.WithProperty("Application", environment.ApplicationName)
				.Enrich.WithProperty("Environment", environment.EnvironmentName)
				.WriteTo.Logger(log => log
					.MinimumLevel.ControlledBy(LevelSwitch)
					.Filter.ByExcluding(configuration["Serilog:Seq:Ignored"] ?? "false")
					.WriteTo.File(
						Path.Combine(Directory.GetCurrentDirectory(), "Logs", $"{environment.ApplicationName}-.log"),
						fileSizeLimitBytes: 31457280,
						retainedFileCountLimit: 10,
						rollingInterval: RollingInterval.Day,
						rollOnFileSizeLimit: true,
						shared: true)
					.WriteTo.Seq(
						configuration["Seq:Uri"] ?? "about:local",
						apiKey: configuration["Seq:ApiKey"] ?? string.Empty));
	}
}
