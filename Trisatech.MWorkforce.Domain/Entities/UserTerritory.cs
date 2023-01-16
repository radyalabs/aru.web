using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Trisatech.MWorkforce.Domain.Entities
{
    public class UserTerritory
    {
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int UserTerritoryId { get; set; }
		[ForeignKey("User")]
		public string UserId { get; set; }
		[ForeignKey("Territory")]
		public string TerritoryId { get; set; }

		public virtual User User { get; set; }
		public virtual Territory Territory { get; set; }
	}
}
