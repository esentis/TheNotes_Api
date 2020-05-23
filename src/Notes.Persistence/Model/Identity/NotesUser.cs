namespace Esen.Notes.Persistence.Model.Identity
{
	using System;

	using Kritikos.Configuration.Persistence.Abstractions;

	using Microsoft.AspNetCore.Identity;

	public class NotesUser : IdentityUser<Guid>, IEntity<Guid>, IAuditable, ITimestamped
	{
		/// <summary>
		/// Date of creation.
		/// </summary>
		public DateTimeOffset CreatedAt { get; set; }

		/// <summary>
		/// Date of last change.
		/// </summary>
		public DateTimeOffset UpdatedAt { get; set; }

		/// <summary>
		/// User that created this entity.
		/// </summary>
		public string CreatedBy { get; set; } = string.Empty;

		/// <summary>
		/// User that last changed this entity.
		/// </summary>
		public string UpdatedBy { get; set; } = string.Empty;
	}
}
