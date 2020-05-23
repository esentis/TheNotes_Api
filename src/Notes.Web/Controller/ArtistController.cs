namespace Esen.Notes.Web.Controller
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.Linq;

	using Esen.Notes.Persistence;
	using Esen.Notes.Web.Helpers;

	using Microsoft.AspNetCore.Mvc;
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
			var artist = new Artist{Name = "babis"};
			return Ok(artist);
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

		[Required]
		public string Name { get; set; }
	}
}
