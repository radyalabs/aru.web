using System;
using System.Collections.Generic;
using System.Text;

namespace Trisatech.MWorkforce.Business.Entities
{
	public class UserModel
	{
		//--Account 
		public string AccountId { get; set; }
		public string Password { get; set; }
		public string RoleId { get; set; }
		public string RoleCode { get; set; }
		public string RoleName { get; set; }

		//--User
		public string UserId { get; set; }
		public string UserCode { get; set; }
		public string UserName { get; set; }
		public string Name { get; set; }
		public string UserEmail { get; set; }
		public string UserPhone { get; set; }

		public string Territoryid { get; set; }

		public ICollection<TerritoryModel> Territories { get; set; }
		public ICollection<UserTerritoryModel> UserTerritories { get; set; }
		//public List<UserTerritoryModel> UserTerritories1 { get; set; }
		public List<TerritoryModel> tempat { get; set; }
	}
}