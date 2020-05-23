namespace Esen.Notes.Persistence
{
	using System;
	using System.Linq;

	using Esen.Notes.Persistence.Model.Identity;

	using Kritikos.Configuration.Persistence.Abstractions;

	using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Logging;

	public class NotesContext : IdentityDbContext<NotesUser, NotesRole, Guid>
	{
		public NotesContext(DbContextOptions<NotesContext> options)
			: base(options)
		{
		}

		public NotesContext(DbContextOptions<NotesContext> options, ILoggerFactory loggerFactory)
			: base(options)
			=> LoggerFactory = loggerFactory;

		private ILoggerFactory? LoggerFactory { get; }

		/// <inheritdoc />
		public override int SaveChanges() => SaveChanges(true, "SYSTEM");

		/// <inheritdoc />
		public override int SaveChanges(bool acceptAllChangesOnSuccess) =>
			SaveChanges(acceptAllChangesOnSuccess, "SYSTEM");

		public int SaveChanges(bool acceptAllChangesOnSuccess, string user)
		{
			var now = DateTimeOffset.UtcNow;
			var entries = ChangeTracker.Entries()
				.Where(x => x.State == EntityState.Added || x.State == EntityState.Modified)
				.ToList();

			foreach (var entry in entries.Where(x => x is IAuditable))
			{
				if (entry.State == EntityState.Added)
				{
					((IAuditable)entry.Entity).CreatedBy = user;
				}

				((IAuditable)entry.Entity).UpdatedBy = user;
			}

			foreach (var entry in entries.Where(x => x is ITimestamped))
			{
				if (entry.State == EntityState.Added)
				{
					((ITimestamped)entry.Entity).CreatedAt = now;
				}

				((ITimestamped)entry.Entity).UpdatedAt = now;
			}

			return base.SaveChanges(acceptAllChangesOnSuccess);
		}

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

		/// <inheritdoc />
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
		}
	}
}
