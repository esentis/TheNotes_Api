using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Esen.Notes.WebModels
{
	public class CreateSongDto
	{

		public long ArtistId { get; set; }

		public long? AlbumId { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public long Duration { get; set; }

		[Required]
		public string Lyrics { get; set; }

		[Required]
		public int WordCount { get; set; }

		[Required]
		public string Genre { get; set; }
	}
}
