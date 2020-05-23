namespace Esen.Notes.Web.Helpers
{
	using Esen.Notes.Persistence;

	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.Logging;

	[ApiController]
	public abstract class BaseController<T> : ControllerBase
		where T : BaseController<T>
	{
		// ReSharper disable once ContextualLoggerProblem [Hack for missing Self keyword]
		protected BaseController(NotesContext ctx, ILogger<T> logger)
		{
			Ctx = ctx;
			Logger = logger;
		}

		protected NotesContext Ctx { get; }

		protected ILogger<T> Logger { get; }
	}
}
