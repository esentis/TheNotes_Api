namespace Esen.Notes.Web.Controller
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.Linq;
	using System.Security.Cryptography.X509Certificates;
	using System.Threading.Tasks;

	using Esen.Notes.Persistence;
	using Esen.Notes.Persistence.Model;
	using Esen.Notes.Web.Helpers;

	using Microsoft.AspNetCore.Mvc;
	using Microsoft.EntityFrameworkCore.Query;
	using Microsoft.Extensions.Logging;

	[Route("api/artist")]
	public class ArtistController : BaseController<ArtistController>
	{
		public ArtistController(NotesContext ctx, ILogger<ArtistController> logger)
			: base(ctx, logger)
		{
		}
		[HttpGet("")]
		public ActionResult<Artist> GetAllArtists()
		{
			

			var artists = Ctx.Artists.Where(x =>true).ToList();
			if (artists == null)
			{
				return NotFound("Artist was not found");
			}


			return Ok(artists);
		}
		[HttpGet("{id}")]
		public ActionResult<Artist> RetrieveArtist(long id)
		{
			/*// ΛΑΘΟΣ, ΔΕ ΤΑ ΧΡΗΣΙΜΟΠΟΙΟΥΜΕ ΕΤΣΙ
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
			};*/

			var artist = Ctx.Artists.Where(x => x.Id == id).SingleOrDefault();
			if (artist == null)
			{
				return NotFound("Artist was not found");
			}

			return Ok(artist);
		}
		[HttpPost("addTwo")]
		public async Task<ActionResult<Artist>> TestAddTwo(string artistName, int artistAge)
		{

			var artist = new Artist() { Name = artistName, Age = artistAge };
			Ctx.Artists.Add(artist);
			await Ctx.SaveChangesAsync();

			Logger.LogDebug("I'm inside the /add");
			/*var a = artists.Where(x => x.Id == id).SingleOrDefault();
			var cds = albums.Where(x => x.Artist.Id == id).ToList();

			var result = new ArtistDto
			{
				Id = a.Id,
				Name = a.Name,
				Albums = cds.Select(x => new AlbumDto { Id = x.Id, Year = x.Year, ArtistName = a.Name, })
					.ToList(),
			};*/

			return Ok();
		}
		[HttpPost("add")]
		public async Task<ActionResult<Artist>> TestAdd(Artist artist)
		{

			
			Ctx.Artists.Add(artist);
			await Ctx.SaveChangesAsync();

			Logger.LogDebug("I'm inside the /add");
			/*var a = artists.Where(x => x.Id == id).SingleOrDefault();
			var cds = albums.Where(x => x.Artist.Id == id).ToList();

			var result = new ArtistDto
			{
				Id = a.Id,
				Name = a.Name,
				Albums = cds.Select(x => new AlbumDto { Id = x.Id, Year = x.Year, ArtistName = a.Name, })
					.ToList(),
			};*/

			return Ok();
		}
		/*
				[HttpPost("")]
				public ActionResult<Artist> CreateArtist(Artist artist)
				{
					if (!ModelState.IsValid)
					{
						return BadRequest();
					}
					Logger.LogDebug("LOGDEbug{esen}");
					return CreatedAtAction(nameof(RetrieveArtist), new { id = artist.Id }, artist);
				}

				[HttpDelete("{id}")]
				public ActionResult DeleteArtist(long id)
				{
					return NoContent();
				}*/
	}


}
