using System;
using System.Collections.Generic;
using System.Text;

namespace Esen.Notes.Persistence.Model
{
	using Kritikos.Configuration.Persistence.Abstractions;
	using Kritikos.Configuration.Persistence.Base;

	public class Artist : IEntity<long>
	{
		public string Name { get; set; } = string.Empty;

		public int Age { get; set; }

		public long Id { get; set; }
	}
}
