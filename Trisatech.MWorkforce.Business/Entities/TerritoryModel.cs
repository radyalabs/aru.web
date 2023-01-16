using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Business.Entities
{
	public class TerritoryModel
	{
		//[Display(Name = "Territory id")]
		public string TerritoryId { get; set; }
		//[Display(Name = "Name")]
		public string Name { get; set; }
		public string Desc { get; set; }

		public string UserId { get; set; }

		//[JsonProperty("UserTerritories")]
		public List<UserTerritoryModel> UserTerritories { get; set; }

		public List<UserModel> UserLis { get; set; }
		public UserModel User { get; set; }


		//public ICollection<UserModel> User { get; set; }

	}
}
