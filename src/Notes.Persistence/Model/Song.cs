using System;
using System.Collections.Generic;
using System.Text;

namespace Esen.Notes.Persistence.Model
{
	using Esen.Notes.Persistence.Model.Identity;

	using Kritikos.Configuration.Persistence.Abstractions;

	public class Song : IEntity<long>
	{
		public long Id { get; set; }

		public Artist Artist { get; set; }

		public string Name { get; set; }

		public string Genre { get; set; }

		public string Lyrics { get; set; }

		// Nullable, the track may not belong to any official album.
		public Album? Album { get; set; }

		// The number is in seconds.
		public long Duration { get; set; }

		public int WordCount { get; set; }
	}
}
