using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Cms.ViewModels
{
    public class NewsViewModel
    {
        [JsonProperty("news_id")]
        public string NewsId { get; set; }
        [JsonProperty("news_title")]
        public string Title { get; set; }
        [JsonProperty("news_desc")]
        public string Desc { get; set; }
        [JsonProperty("news_content")]
        public string Content { get; set; }
        [JsonProperty("news_ispublish")]
        public bool IsPublish { get; set; }
        [JsonProperty("news_publishdate")]
        public DateTime PublishedDate { get; set; }
    }
}
