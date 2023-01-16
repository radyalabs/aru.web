using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Api.ViewModels
{
    public class AssignmentSearchViewModel
    {
        [JsonProperty("keyword")]
        public string Keyword { get; set; }
        [JsonProperty("date")]
        public DateTime? Date { get; set; }
    }
}
