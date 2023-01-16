using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Api.ViewModels
{
    public class CustomerFormViewModel
    {
        //TOKO
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
        public string OwnerName { get; set; }
        public string OwnerAddress { get; set; }
        public string OwnerCity { get; set; }
        public string OwnerDistrict { get; set; }
        public string OwnerVillage { get; set; }
        public string OwnerPhoneNumber { get; set; }

        //PIC
        public string PICName { get; set; }
        public string PICPhoneNumber { get; set; }

        public IFormFile PhotoIdCard { get; set; }
        public IFormFile PhotoNPWP { get; set; }
        public IFormFile BrandingPhoto { get; set; }
        public IFormFile StorePhoto { get; set; }
    }
}
