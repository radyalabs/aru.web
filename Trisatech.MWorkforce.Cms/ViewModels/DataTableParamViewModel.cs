using Trisatech.MWorkforce.Cms.Helpers;
using Newtonsoft.Json;
using Trisatech.AspNet.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Cms.ViewModels
{
    public class AssignmentDataTableViewModel
    {
        [JsonProperty("assignment_id")]
        public string AssignmentId { get; set; }
        [JsonProperty("assignment_code")]
        public string AssignmentCode { get; set; }
        [JsonProperty("assignment_name")]
        public string AssignmentName { get; set; }
        [JsonProperty("assignment_date")]
        public DateTime AssignmentDate { get; set; }
        [JsonProperty("assignment_status_code")]
        public string AssignmentStatusCode { get; set; }
        [JsonProperty("assignment_address")]
        public string AssignmentAddress { get; set; }
        [JsonProperty("assignment_status")]
        public string Status { get; set; }
        [JsonProperty("agent_code")]
        public string AgentCode { get; set; }
        [JsonProperty("agent_name")]
        public string AgentName { get; set; }
        [JsonProperty("contact_name")]
        public string ContactName { get; set; }
        [JsonProperty("contact_number")]
        public string ContactNumber { get; set; }
        public string BaseUrl { get; set; }
        [JsonProperty("url_detail")]
        public string UrlDetail
        {
            get
            {
                return BaseUrl+"/taskmanagement/details/"+AssignmentId;
            }
        }
        [JsonProperty("url_edit")]
        public string UrlEdit
        {
            get
            {
                return BaseUrl + "/assignmentmanagement/edit/" + AssignmentId;
            }
        }
        [JsonProperty("url_delete")]
        public string UrlDelete
        {
            get
            {
                return BaseUrl + "/assignmentmanagement/delete/" + AssignmentId;
            }
        }
        public string Date
        {
            get
            {
                return AssignmentDate.ToUtcID().ToString("dd/MM/yyyy HH:mm:ss");
            }
        }
    }

    public class UserDataTableViewModel
    {
        //--Account 
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        //--User    
        public string UserId { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
        public string BaseUrl { get; set; }
        public string UrlDetail
        {
            get
            {
                return BaseUrl + "/usermanagement/details/" + UserId;
            }
        }
    }

    public class ReportDataTableViewModel
    {  
        public string UserId { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndedTime { get; set; }

        public double TotalWorkTime {
            get
            {
                double result = 0;

                if (StartTime != null && EndedTime != null)
                {
                    result = Math.Round((EndedTime - StartTime).Value.TotalHours, 2);
                }
                
                return result;
            }
        }

        public string CheckinTime { get
            {
                return (StartTime == null ? "No Checkin" : StartTime.Value.ToUtcID().ToString("dd/MM/yyyy HH:mm:ss"));
            }
        }

        public string CheckOutTime
        {
            get
            {
                return (EndedTime == null ? "No Checkout" : EndedTime.Value.ToUtcID().ToString("dd/MM/yyyy HH:mm:ss"));
            }
        }

        public int TotalLostTime { get; set; }

        public double TotalLostTimeInMinute
        {
            get
            {
                return (TotalLostTime / 60);
            }
        }

        public int TotalTaskCompleted { get; set; }
        public int TotalTaskInProgress { get; set; }
        public int TotalTaskFailed { get; set; }
        public int TotalTask { get; set; }
        public string BaseUrl { get; set; }

        public string UrlDetail
        {
            get
            {
                return BaseUrl + "/report/details/" + UserId + "?date="+StartTime.Value.ToUtcID().ToString("MM-dd-yyyy");
            }
        }

    }
    public class CustomerDataTableViewModel
    {
        [JsonProperty("customer_id")]
        public string CustomerId { get; set; }
        [JsonProperty("customer_email")]
        public string CustomerEmail { get; set; }
        [JsonProperty("customer_code")]
        public string CustomerCode { get; set; }
        [JsonProperty("customer_name")]
        public string CustomerName { get; set; }
        [JsonProperty("customer_address")]
        public string CustomerAddress { get; set; }
        [JsonProperty("customer_phone_number")]
        public string CustomerPhoneNumber { get; set; }
        [JsonProperty("customer_photo")]
        public string CustomerPhoto { get; set; }
        public string BaseUrl { get; set; }
        public string UrlDetail
        {
            get
            {
                return BaseUrl + "/customer/details/" + CustomerId;
            }
        }
        public string UrlDetailOutlet
        {
            get
            {
                return BaseUrl + "/outlet/details/" + CustomerId;
            }
        }
        public string Date
        {
            get
            {
                return "";
            }
        }
    }
    public class ContactDataTableViewModel
    {
        [JsonProperty("contact_name")]
        public string ContactName { get; set; }
        [JsonProperty("contact_id")]
        public string ContactId { get; set; }
        [JsonProperty("position")]
        public string Position { get; set; }
        [JsonProperty("contact_number")]
        public string ContactNumber { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("secondary_email")]
        public string SecondaryEmail { get; set; }
        [JsonProperty("photo")]
        public string ContactPhoto { get; set; }
        public string BaseUrl { get; set; }
        public string urlDetail
        {
            get
            {
                return BaseUrl + "/contact/details/" + ContactId;
            }
        }
    }
    public class SurveyDataTableViewModel
    {
        [JsonProperty("survey_id")]
        public string SurveyId { get; set; }
        [JsonProperty("name")]
        public string SurveyName { get; set; }
        [JsonProperty("link")]
        public string SurveyLink { get; set; }
        [JsonProperty("start_date")]
        public DateTime StartDate { get; set; }
        [JsonProperty("end_date")]
        public DateTime EndDate { get; set; }
        public string BaseUrl { get; set; }
        public string UrlEdit
        {
            get
            {
                return BaseUrl + "/survey/edit/" + SurveyId;
            }
        }
        public string UrlDetail
        {
            get
            {
                return BaseUrl + "/survey/details/" + SurveyId;
            }
        }
    }
    public class ProductDataTableViewModel
    {

        [JsonProperty("product_id")]
        public string ProductId { get; set; }
        [JsonProperty("product_code")]
        public string ProductCode { get; set; }
        [JsonProperty("product_name")]
        public string ProductName { get; set; }
        [JsonProperty("product_model")]
        public string ProductType { get; set; }
        [JsonProperty("product_image")]
        public string ProductImage { get; set; }
        [JsonProperty("product_price")]
        public decimal? ProductPrice { get; set; }

        public string BaseUrl { get; set; }
        public string UrlEdit
        {
            get
            {
                return BaseUrl + "/product/edit/" + ProductId;
            }
        }

        public string UrlDetail
        {
            get
            {
                return BaseUrl + "/product/details/" + ProductId;
            }
        }
    }

    public class NewsDataTableViewModel
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

        public string BaseUrl { get; set; }
        public string urlDetail
        {
            get
            {
                return "/news/details/" + NewsId;
            }
        }
    }

	public class TerritoryDataTableViewModel
	{
		[JsonProperty("territory_id")]
		public string TerritoryId { get; set; }
		[JsonProperty("Name")]
		public string Name { get; set; }
		[JsonProperty("Desc")]
		public string Desc { get; set; }

		public string BaseUrl { get; set; }
		public string urlDetail
		{
			get
			{
				return  "/territory/details/" + TerritoryId;
			}
		}
	}

	public class DataTableParamViewModel
    {
        public string target { get; set; }
        public string search { get; set; }
        public int length { get; set; }
        public int start { get; set; }
        public int orderCol { get; set; }
        public string orderType { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public string status { get; set; }
    }
}
