namespace Esen.Notes.Web.Helpers
{
	using Esen.Notes.Persistence;

	using Microsoft.AspNetCore.Mvc;

	[ApiController]
	public abstract class BaseController : ControllerBase
	{
		protected BaseController(NotesContext ctx)
		{
			Ctx = ctx;
		}

		protected NotesContext Ctx { get; }
	}
}
