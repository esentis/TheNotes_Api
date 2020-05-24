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
	using Esen.Notes.Persistence.Model.Identity;
	using Esen.Notes.Web.Helpers;

	using Microsoft.AspNetCore.Mvc;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Query;
	using Microsoft.Extensions.Logging;

	[Route("api/album")]
	public class AlbumController : BaseController<ArtistController>
	{
		public AlbumController(NotesContext ctx, ILogger<ArtistController> logger)
			: base(ctx, logger)
		{
		}

		[HttpGet("")]
		public ActionResult<Album> GetAllAlbums()
		{

			var albums = Ctx.Albums.Include(x => x.Artist).Where(x => true).ToList();
			if (albums == null)
			{
				return NotFound("Album was not found");
			}

			return Ok(albums);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Album>> RetrieveAlbum(long id)
		{

			var album = await Ctx.Albums.Include(x => x.Artist).Where(x => x.Id == id).SingleOrDefaultAsync();
			if (album == null)
			{
				return NotFound("Album was not found");
			}

			var songs = await Ctx.Songs.Where(x => x.Album == album).Select(x => x.Name).ToListAsync();

			return Ok(songs);
		}

		[HttpPost("add")]
		public async Task<ActionResult<Album>> AddAlbum(Album album)
		{

			Ctx.Albums.Add(album);
			await Ctx.SaveChangesAsync();

			return Ok();
		}

	}

}
