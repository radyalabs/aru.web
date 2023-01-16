using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Trisatech.MWorkforce.Domain.Entities
{
    public class CustomerDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerDetailId { get; set; }
        [ForeignKey("Customer")]
        public string CustomerId { get; set; }
        [StringLength(255)]
        public string BrandName { get; set; }
        [StringLength(255)]
        public string BrandAddress { get; set; }
        [StringLength(100)]
        public string BrandCity { get; set; }
        [StringLength(255)]
        public string BrandDistrict { get; set; }
        [StringLength(255)]
        public string BrandVillage { get; set; }
        [StringLength(100)]
        public string BrandStatus { get; set; }
        [StringLength(10)]
        public string BrandAge { get; set; }
        [StringLength(100)]
        public string BrandType { get; set; }
        [StringLength(255)]
        public string BrandingName { get; set; }
        [StringLength(255)]
        public string Desc { get; set; }
        [StringLength(255)]
        public string BrandPhotoUrl { get; set; }
        [StringLength(255)]
        public string BrandPhotoUrl1 { get; set; }
        public double? BrandLatitude { get; set; }
        public double? BrandLongitude { get; set; }
        public string Reserved { get; set; }
        [StringLength(255)]
        public string BrandPhotoBlobId { get; set; }
        [StringLength(255)]
        public string brandPhoto1BlobId { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
