namespace Esen.Notes.Persistence.Model.Identity
{
	using System;
	using System.Collections.Generic;
	using System.Text;

	using Kritikos.Configuration.Persistence.Abstractions;

	public class Album : IEntity<long>
	{
		public long Id { get; set; }

		public string Name { get; set; } = string.Empty;

		public string Genre { get; set; } = string.Empty;

		public Artist Artist { get; set; }

		public DateTime ReleaseDate { get; set; }
	}
}
