using System;
using System.Collections.Generic;
using System.Text;

namespace Trisatech.MWorkforce.Business.Entities
{
    public class NewsModel
    {
        public string NewsId { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string Content { get; set; }
        public bool IsPublish { get; set; }
        public DateTime PublishedDate { get; set; }
    }
}
