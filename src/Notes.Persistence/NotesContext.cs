namespace Esen.Notes.Persistence
{
	using System;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;

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
		public override int SaveChanges()
			=> SaveChanges(true, ApiUser);

		/// <inheritdoc />
		public override int SaveChanges(bool acceptAllChangesOnSuccess)
			=> SaveChanges(acceptAllChangesOnSuccess, ApiUser);

		public int SaveChanges(string user)
			=> SaveChanges(true, user);

		public int SaveChanges(bool acceptAllChangesOnSuccess, string user)
		{
			ChangeTracker.StampEntities(user);
			return SaveChanges(acceptAllChangesOnSuccess);
		}

		/// <inheritdoc />
		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
			SaveChangesAsync(true, ApiUser, cancellationToken);

		/// <inheritdoc />
		public override Task<int> SaveChangesAsync(
			bool acceptAllChangesOnSuccess,
			CancellationToken cancellationToken = default)
			=> SaveChangesAsync(acceptAllChangesOnSuccess, ApiUser, cancellationToken);

		public Task<int> SaveChangesAsync(
			string user,
			CancellationToken cancellationToken = default)
		{
			ChangeTracker.StampEntities(user);
			return SaveChangesAsync(true, cancellationToken);
		}

		public Task<int> SaveChangesAsync(
			bool acceptAllChangesOnSuccess,
			string user,
			CancellationToken cancellationToken = default)
		{
			ChangeTracker.StampEntities(user);
			return SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
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
