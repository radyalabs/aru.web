using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Cms.ViewModels
{
    public class OutletViewModel
    {
        public int CustomerDetailId { get; set; }
        [Display(Name = "Store Name")]
        public string StoreName { get; set; }
        [Display(Name = "Store Address")]
        public string StoreAddress { get; set; }
        [Display(Name = "Store City")]
        public string StoreCity { get; set; }
        [Display(Name = "Store District")]
        public string StoreDistrict { get; set; }
        [Display(Name = "Store Village")]
        public string StoreVillage { get; set; }
        [Display(Name = "Store Status")]
        public string StoreStatus { get; set; }
        [Display(Name = "Store Age")]
        public string StoreAge { get; set; }
        [Display(Name = "Store Type")]
        public string StoreType { get; set; }
        [Display(Name = "Road Width")]
        public string WidthRoad { get; set; }
        [Display(Name = "Branding Name")]
        public string BrandingName { get; set; }
        public double? StoreLatitude { get; set; }
        public double? StoreLongitude { get; set; }
        [Display(Name = "Notes")]
        public string Note { get; set; }

        //Pemilik
        public string CustomerId { get; set; }
        public string OwnerName { get; set; }
        public string OwnerAddress { get; set; }
        public string OwnerCity { get; set; }
        public string OwnerDistrict { get; set; }
        public string OwnerVillage { get; set; }
        public string OwnerPhoneNumber { get; set; }

        //PIC
        public string PICName { get; set; }
        public string PICPhoneNumber { get; set; }

        public string PhotoIdCardBlobId { get; set; }
        public string PhotoNPWPBlobId { get; set; }
        public string BrandingPhotoBlobId { get; set; }
        public string StorePhotoBlobId { get; set; }

        [Display(Name = "Id Card Url")]
        public string PhotoIdCardUrl { get; set; }
        [Display(Name = "NPWP Photo Url")]
        public string PhotoNPWPUrl { get; set; }
        public string BrandingPhotoUrl { get; set; }
        [Display(Name = "Store Photo Url")]
        public string StorePhotoUrl { get; set; }
    }
}
