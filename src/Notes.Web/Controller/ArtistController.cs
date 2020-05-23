namespace Esen.Notes.Web.Controller
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.Linq;

	using Esen.Notes.Persistence;
	using Esen.Notes.Web.Helpers;

	using Microsoft.AspNetCore.Mvc;
	using Microsoft.EntityFrameworkCore.Query;
	using Microsoft.Extensions.Logging;

	[Route("api/artist")]
	public class ArtistController : BaseController
	{
		public ArtistController(NotesContext ctx, ILogger<ArtistController> logger)
			: base(ctx)
		{
			Logger = logger;
		}

		private ILogger<ArtistController> Logger { get; }

		[HttpGet("{id}")]
		public ActionResult<Artist> RetrieveArtist(long id)
		{
			// ΛΑΘΟΣ, ΔΕ ΤΑ ΧΡΗΣΙΜΟΠΟΙΟΥΜΕ ΕΤΣΙ
			IQueryable<Artist> artists = (new List<Artist>()).AsQueryable();
			IQueryable<Album> albums = (new List<Album>()).AsQueryable();

			var a = artists.Where(x => x.Id == id).SingleOrDefault();
			var cds = albums.Where(x => x.Artist.Id == id).ToList();

			var result = new ArtistDto
			{
				Id = a.Id,
				Name = a.Name,
				Albums = cds.Select(x => new AlbumDto { Id = x.Id, Year = x.Year, ArtistName = a.Name, })
					.ToList(),
			};

			return Ok(result);
		}

		[HttpPost("")]
		public ActionResult<Artist> CreateArtist(Artist artist)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}

			return CreatedAtAction(nameof(RetrieveArtist), new { id = artist.Id }, artist);
		}

		[HttpDelete("{id}")]
		public ActionResult DeleteArtist(long id)
		{
			return NoContent();
		}
	}

	public class Artist
	{
		public long Id { get; set; }

		public string Name { get; set; }
	}

	public class Album
	{
		public long Id { get; set; }

		public int Year { get; set; }

		public Artist Artist { get; set; }
	}

	public class ArtistDto
	{
		public long Id { get; set; }

		[Required]
		public string Name { get; set; }

		public List<AlbumDto> Albums { get; set; }
	}

	public class AlbumDto
	{
		public long Id { get; set; }

		public int Year { get; set; }

		public string ArtistName { get; set; }
	}

	public class AlbumRetrieveDto
	{
		public long Id { get; set; }

		public int Year { get; set; }

		public ArtistDto Artist { get; set; }
	}
}
