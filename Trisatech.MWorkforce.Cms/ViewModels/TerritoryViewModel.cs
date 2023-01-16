using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Cms.ViewModels
{
    public class TeritoryViewModel
    {
		public string TerritoryId { get; set; }

		[Display(Name = "Name")]
		public string Name { get; set; }
		[Required]
		[Display(Name = "Deskripsi")]
		public string Desc { get; set; }

		[Display(Name = "UserId")]
		public string UserId { get; set; }

		//[JsonProperty("User")]
		//[Display(Name = "User")]
		//public string User { get; set; }

	}
}
