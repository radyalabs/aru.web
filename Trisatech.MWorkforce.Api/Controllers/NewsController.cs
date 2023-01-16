using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Trisatech.MWorkforce.Api.Model;
using Trisatech.MWorkforce.Business.Entities;
using Trisatech.MWorkforce.Business.Interfaces;
using Trisatech.MWorkforce.Business.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Trisatech.MWorkforce.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/news")]
    public class NewsController : ControllerBase
    {
        private readonly IContentManagementService contentManagementService;
        private Trisatech.AspNet.Common.Models.JsonEntity json;
        public NewsController(IContentManagementService contentManagementService)
        {
            json = new Trisatech.AspNet.Common.Models.JsonEntity();
            this.contentManagementService = contentManagementService;
        }
        
        [HttpGet]
        public IActionResult Get(string keyword = "", int limit = 10, int offset = 0)
        {
            try
            {
                json.AddAlert((int)HttpStatusCode.OK, "success");
                json.AddData(contentManagementService.Get(keyword, limit, offset, "PublishedDate", "desc", true));
            }
            catch(Exception ex)
            {
                json.AddAlert((int)HttpStatusCode.InternalServerError, ex.ToString());
                json.AddData(null);
            }

            return Ok(json);
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            try
            {
                json.AddAlert((int)HttpStatusCode.OK, "success");
                json.AddData(contentManagementService.Get(id));
            }
            catch (Exception ex)
            {
                json.AddAlert((int)HttpStatusCode.InternalServerError, ex.ToString());
                json.AddData(null);
            }

            return Ok(json);
        }
    }
}