namespace Esen.Notes.Persistence
{
	using System;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;

	using Esen.Notes.Persistence.Model;
	using Esen.Notes.Persistence.Model.Identity;

	using Kritikos.Configuration.Persistence.Abstractions;

	using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Logging;

	public class NotesContext : IdentityDbContext<NotesUser, NotesRole, Guid>
	{
		private const string ApiUser = "API";

		public NotesContext(DbContextOptions<NotesContext> options)
			: base(options)
		{
		}

		public NotesContext(DbContextOptions<NotesContext> options, ILoggerFactory loggerFactory)
			: base(options)
			=> LoggerFactory = loggerFactory;

		private ILoggerFactory? LoggerFactory { get; }

		/// <inheritdoc />
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (optionsBuilder == null)
			{
				throw new ArgumentNullException(nameof(optionsBuilder));
			}

			if (LoggerFactory != null)
			{
				optionsBuilder.UseLoggerFactory(LoggerFactory);
			}

			base.OnConfiguring(optionsBuilder);
		}

		public DbSet<Artist> Artists { get; set; }

		public DbSet<Album> Albums { get; set; }

		public DbSet<Song> Songs { get; set; }

		/// <inheritdoc />
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.Entity<Artist>(e => e.UseXminAsConcurrencyToken());
			builder.Entity<Album>(e => e.UseXminAsConcurrencyToken());
			builder.Entity<Song>(e => e.UseXminAsConcurrencyToken());
		}
	}
}
