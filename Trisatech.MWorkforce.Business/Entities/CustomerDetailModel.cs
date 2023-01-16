using System;
using System.Collections.Generic;
using System.Text;

namespace Trisatech.MWorkforce.Business.Entities
{
    public class CustomerDetailModel
    {
        //TOKO
        public int CustomerDetailId { get; set; }
        public string StoreName { get; set; }
        public string StoreAddress { get; set; }
        public string StoreCity { get; set; }
        public string StoreDistrict { get; set; }
        public string StoreVillage { get; set; }
        public string StoreStatus { get; set; }
        public string StoreAge { get; set; }
        public string StoreType { get; set; }
        public string WidthRoad { get; set; }
        public string BrandingName { get; set; }
        public double? StoreLatitude { get; set; }
        public double? StoreLongitude { get; set; }
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

        public string PhotoIdCardUrl { get; set; }
        public string PhotoNPWPUrl { get; set; }
        public string BrandingPhotoUrl { get; set; }
        public string StorePhotoUrl { get; set; }
    }
}
