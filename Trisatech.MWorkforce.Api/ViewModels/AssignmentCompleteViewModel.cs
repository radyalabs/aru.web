using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Api.ViewModels
{
    public class AssignmentCompleteViewModel
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Remarks { get; set; }
        public string AssignmentStatus { get; set; }
        public string ReasonCode { get; set; }
        public DateTime? ProcessedTime { get; set; }
        public IFormFile file { get; set; }
    }
}
