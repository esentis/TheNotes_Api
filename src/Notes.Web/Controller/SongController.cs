namespace Esen.Notes.Web.Controller
{
	using System;
	using System.Linq;
	using System.Threading.Tasks;

	using Esen.Notes.Persistence;
	using Esen.Notes.Persistence.Model;
	using Esen.Notes.Persistence.Model.Identity;
	using Esen.Notes.Web.Helpers;
	using Esen.Notes.WebModels;

	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Routing;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Logging;

	[Route("api/song")]
	public class SongController : BaseController<ArtistController>
	{
		public SongController(NotesContext ctx, ILogger<ArtistController> logger)
			: base(ctx, logger)
		{
		}

		[HttpGet("")]
		public ActionResult<Song> GetAllAlbums()
		{

			var songs = Ctx.Songs.Include(x => x.Album).Include(x => x.Artist).Where(x => true).ToList();
			if (songs == null)
			{
				return NotFound("No songs were found");
			}

			return Ok(songs);
		}

		[HttpGet("{id}")]
		public ActionResult<Song> RetrieveSong(long id)
		{

			var song = Ctx.Songs.Where(x => x.Id == id).SingleOrDefault();
			if (song == null)
			{
				return NotFound("Song was not found");
			}

			return Ok(song);
		}

		[HttpPost("add")]
		public async Task<ActionResult<Song>> AddSong([FromForm] CreateSongDto songDto)
		{
			var artist = await Ctx.Artists.Where(x => x.Id == songDto.ArtistId).SingleOrDefaultAsync();


			if (artist == null)
			{
				Logger.LogError("Requested {Entity} with Id {Id} was not found!", typeof(Artist), songDto.ArtistId);
				return BadRequest("No artist found");

			}

			var album = await Ctx.Albums.Where(x => x.Id == songDto.AlbumId).SingleOrDefaultAsync();
			if (album == null)
			{
				Logger.LogError("Requested {Entity} with Id {Id} was not found!", typeof(Album), songDto.AlbumId);
				return BadRequest("No album found");
			}
			var delimiter = new char[] { ' ', '\n', '\r' };



			var song = new Song
			{
				Artist = artist,
				Album = album,
				Name = songDto.Name,
				Duration = songDto.Duration,
				Genre = songDto.Genre,
				Lyrics = songDto.Lyrics,
				WordCount = songDto.Lyrics.Split(delimiter, StringSplitOptions.RemoveEmptyEntries).Length,
			};
			Ctx.Songs.Add(song);

			try
			{
				await Ctx.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException exception)
			{
				Logger.LogError(exception, "Concurrency issues");
				return Conflict();
			}

			return Ok();
		}

	}



}
