using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Trisatech.MWorkforce.Domain.Entities
{
	public class Territory : BaseData
	{
		public Territory()
		{
		}

		[Key]
		[StringLength(50, MinimumLength = 24)]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public string TerritoryId { get; set; }
		[StringLength(220)]
		public string Name { get; set; }
		[StringLength(255)]
		public string Desc { get; set; }

		public virtual ICollection<UserTerritory> UserTerritory { get; set; }
	}
}
