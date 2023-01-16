using Trisatech.MWorkforce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Trisatech.MWorkforce.Business.Entities
{
    public class CreateGasolineModel
    {
        //Assignment
        public string AssignmentId { get; set; }
        public string AssignmentCode { get; set; }
        public string AssignmentName { get; set; }
        public string AssignmentStatusCode { get; set; }
        public DateTime AssignmentDate { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public string Address { get; set; }
        public string Remarks { get; set; }
        public AssignmentType AssignmentType { get; set; }

        //Assignment detail
        public string AssignmentDetailId { get; set; }
        public string UserId { get; set; }
        public string UserCode { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Attachment { get; set; }
        public string Attachment1 { get; set; }
        public string Attachment2 { get; set; }
        public string AttachmentBlobId { get; set; }
        public string AttachmentBlobId1 { get; set; }
        public string AttachmentBlobId2 { get; set; }
        public string Note { get; set; }
    }
}
