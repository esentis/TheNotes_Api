using System;
using System.Collections.Generic;
using System.Text;

namespace Esen.Notes.Persistence
{
	using System.Linq;

	using Kritikos.Configuration.Persistence.Abstractions;

	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.ChangeTracking;

	public static class DbContextHelpers
	{
		public static void StampEntities(this ChangeTracker tracker, string user)
		{
			if (tracker == null)
			{
				throw new ArgumentNullException(nameof(tracker));
			}

			var now = DateTimeOffset.UtcNow;
			var entries = tracker.Entries()
				.Where(x => x.State == EntityState.Added || x.State == EntityState.Modified)
				.ToList();

			foreach (var entry in entries.Where(x => x.Entity is IAuditable))
			{
				if (entry.State == EntityState.Added)
				{
					((IAuditable)entry.Entity).CreatedBy = user;
				}

				((IAuditable)entry.Entity).UpdatedBy = user;
			}

			foreach (var entry in entries.Where(x => x.Entity is ITimestamped))
			{
				if (entry.State == EntityState.Added)
				{
					((ITimestamped)entry.Entity).CreatedAt = now;
				}

				((ITimestamped)entry.Entity).UpdatedAt = now;
			}
		}
	}
}
