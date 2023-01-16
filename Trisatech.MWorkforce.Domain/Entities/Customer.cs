using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Trisatech.MWorkforce.Domain.Entities
{
    public class Customer:BaseData
    {
        [Key]
        [StringLength(50, MinimumLength = 24)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string CustomerId { get; set; }
        [DataType(DataType.EmailAddress)]
        public string CustomerEmail { get; set; }
        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; }
        [StringLength(100)]
        public string CustomerCode { get; set; }
        [StringLength(255)]
        public string CustomerAddress { get; set; }
        [StringLength(15)]
        public string CustomerPhoneNumber { get; set; }
        [StringLength(100)]
        public string CustomerCity { get; set; }
        [StringLength(255)]
        public string CustomerPhoto { get; set; }
        public double? CustomerLatitude { get; set; }
        public double? CustomerLongitude { get; set; }
        [StringLength(255)]
        public string Desc { get; set; }
        [StringLength(255)]
        public string CustomerDistrict { get; set; }
        [StringLength(255)]
        public string CustomerVillage { get; set; }
        [StringLength(100)]
        public string CustomerPhotoId { get; set; }
        [StringLength(255)]
        public string CustomerPhotoNPWP { get; set; }
        [StringLength(100)]
        public string CustomerPhotoBlobId { get; set; }
        [StringLength(100)]
        public string CustomerPhotoIdBlobId { get; set; }
        [StringLength(100)]
        public string CustomerPhotoNPWPBlobId { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual CustomerDetail CustomerDetail { get; set; }
    }
}
