using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Trisatech.MWorkforce.Domain.Entities
{
    public class News:BaseData
    {
        [Key]
        [StringLength(50, MinimumLength = 24)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string NewsId { get; set; }
        [StringLength(225)]
        public string Title { get; set; }
        [StringLength(255)]
        public string Desc { get; set; }
        [DataType(DataType.Html)]
        public string Content { get; set; }
        public bool IsPublish { get; set; }
        public DateTime PublishedDate { get; set; }
    }
}
