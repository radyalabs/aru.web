using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Trisatech.MWorkforce.Domain.Entities
{
	public class User : BaseData
	{
		[Key]
		[StringLength(50, MinimumLength = 24)]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public string UserId { get; set; }

		[ForeignKey("Account")]
		[StringLength(50, MinimumLength = 24)]
		public string AccountId { get; set; }

		[StringLength(20, MinimumLength = 2)]
		public string UserCode { get; set; }

		[StringLength(20, MinimumLength = 2)]
		public string UserName { get; set; }
		[StringLength(100)]
		public string FullName { get; set; }

		[StringLength(50, MinimumLength = 6)]
		[DataType(DataType.EmailAddress)]
		public string UserEmail { get; set; }

		[StringLength(15, MinimumLength = 3)]
		public string UserPhone { get; set; }

		public virtual Account Account { get; set; }

        [NotMapped]
        public virtual ICollection<Territory> Territory { get; set; }

		public virtual ICollection<UserTerritory> UserTerritory { get; set; }
	}
}
