using Trisatech.MWorkforce.Domain.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
namespace Trisatech.MWorkforce.Business.Entities
{
	public class UserTerritoryModel
    {
		public int UserTerritoryId { get; set; }
        public string TerritoryName { get; set; }
		public string UserId { get; set; }
		public string TerritoryId { get; set; }
		//public UserModel User { get; set; }
		public string AssignmentId { get; set; }
		public UserModel User { get; set; }
	}
}
