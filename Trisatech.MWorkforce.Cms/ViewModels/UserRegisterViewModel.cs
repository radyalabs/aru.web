using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Cms.ViewModels
{
    public class UserRegisterViewModel
    {
        //--Account
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Confirm Password")]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        [Required]
        [Display(Name = "Role")]
        public string RoleId { get; set; }

        //--User
        [Display(Name = "Sales Code")]
        public string UserCode { get; set; }
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [Required]
        public string Name { get; set; }
        [Display(Name = "Email")]
        public string UserEmail { get; set; }
        [Display(Name = "Phone number")]
        public string UserPhone { get; set; }

		[Display(Name = "Territory")]
		public string Territoryid { get; set; }

		public List<UserTerritoryViewModel> UserTerritory { get; set; }

		public List<TeritoryViewModel> tempat { get; set; }
	}
}
