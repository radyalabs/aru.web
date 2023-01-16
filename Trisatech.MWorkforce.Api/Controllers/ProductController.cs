using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Trisatech.MWorkforce.Api.Helpers;
using Trisatech.MWorkforce.Api.Model;
using Trisatech.MWorkforce.Api.ViewModels;
using Trisatech.MWorkforce.Business.Entities;
using Trisatech.MWorkforce.Business.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Trisatech.MWorkforce.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/product")]
    public class ProductController : AppBaseController
    {
        private readonly IProductService productService;
        private readonly IAccountService accountService;
        private Trisatech.AspNet.Common.Models.JsonEntity json;
        public ProductController(IAccountService accountService, IProductService productService, IOptions<ApplicationSetting> options):base(options)
        {
            this.productService = productService;
            this.accountService = accountService;
            json = new Trisatech.AspNet.Common.Models.JsonEntity();
        }
        
        [HttpGet]
        public ActionResult Get([FromHeader(Name = "X-Aru-Token")]string authKey)
        {
            try
            {
                SetUserAuth();

                if (UserAuth == null)
                    return Ok(UnAuthorizeResponse.UnauthorizeResponse());

                int totalRow = productService.TotalRow();
                var result = productService.Get("", totalRow, 0);

                json.SetError(false);
                json.AddAlert((int)HttpStatusCode.OK, "Success");
                json.AddData(result);
            }
            catch (Exception ex)
            {
                json.SetError(true);
                json.AddAlert((int)HttpStatusCode.InternalServerError, ex.Message);
            }
            return Ok(json);
        }

        [HttpPost]
        [Route("order")]
        public async Task<ActionResult> Order([FromHeader(Name = "X-Aru-Token")]string authKey, [FromBody]OrderViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                SetUserAuth();

                if (UserAuth == null)
                    return Ok(UnAuthorizeResponse.UnauthorizeResponse());

                if (viewModel.Products != null && viewModel.Products.Count > 0)
                {
                    try
                    {
                        List<OrderModel> orderModels = new List<OrderModel>();
                        foreach (var item in viewModel.Products)
                        {
                            orderModels.Add(new OrderModel
                            {
                                AssignmentId = viewModel.AssignmentId,
                                CustomerId = viewModel.CustomerId,
                                OrderId = Guid.NewGuid().ToString(),
                                ProductCode = item.ProductCode,
                                Discount = item.Discount,
                                ProductAmount = item.ProductAmount,
                                ProductName = item.ProductName,
                                Quantity = item.Quantity
                            });
                        }

                        await productService.Order(orderModels, UserAuth.UserId);

                        json.SetError(false);
                        json.AddAlert((int)HttpStatusCode.OK, "Success");
                        json.AddData(null);
                    }
                    catch(Exception ex)
                    {
                        json.SetError(false);
                        json.AddAlert((int)HttpStatusCode.InternalServerError, ex.Message);
                        json.AddData(ex);
                    }
                }
                else
                {
                    json.SetError(false);
                    json.AddAlert((int)HttpStatusCode.NotAcceptable, "product is empty");
                    json.AddData(ModelState);
                }
            }
            else
            {
                json.SetError(false);
                json.AddAlert((int)HttpStatusCode.NotAcceptable, "Not acceptable");
                json.AddData(ModelState);
            }

            return Ok(json);
        }
    }
}