using Trisatech.MWorkforce.Api.Helpers;
using Trisatech.MWorkforce.Api.Model;
using Trisatech.MWorkforce.Api.ViewModels;
using Trisatech.MWorkforce.Business.Entities;
using Trisatech.MWorkforce.Business.Interfaces;
using Trisatech.MWorkforce.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Trisatech.AspNet.Common.Extensions;
using Trisatech.AspNet.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Api.Controllers
{
    public class CustomerController : AppBaseController
    {
        private readonly ICustomerService customerService;
        private readonly IAccountService accountService;
        private readonly IAzureStorageService azureStorageService;
        private Trisatech.AspNet.Common.Models.JsonEntity json;

        public CustomerController(ICustomerService customerService,
            IAccountService accountService,
            IAzureStorageService azureStorageService, 
            IOptions<ApplicationSetting> options):base(options)
        {
            this.customerService = customerService;
            this.azureStorageService = azureStorageService;
            this.accountService = accountService;

            json = new Trisatech.AspNet.Common.Models.JsonEntity();
        }

        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<CustomerModel>), 200)]
        [Route("/api/customer")]
        public IActionResult Post([FromHeader(Name = "X-Aru-Token")]string authKey, [FromBody]ContactSearchViewModel request, int limit = 20, int offset = 0)
        {
            try
            {
                SetUserAuth();

                if (UserAuth == null)
                    return Ok(UnAuthorizeResponse.UnauthorizeResponse());

                var result = customerService.Get(request?.Keyword, limit, offset);

                json.SetError(false);
                json.AddAlert((int)HttpStatusCode.OK, "success");
                json.AddData(result);
            }
            catch (ApplicationException appEx)
            {
                json.SetError(false);
                json.AddAlert((int)HttpStatusCode.NotAcceptable, appEx.Message);
                json.AddData(null);
            }
            catch (Exception ex)
            {
                json.SetError(true);
                json.AddAlert((int)HttpStatusCode.InternalServerError, ex.Message);
                json.AddData(null);
            }
            return Ok(json);
        }

        //[HttpPost]
        //[Route("/api/customer/add")]
        //public IActionResult Add([FromHeader(Name = "X-Aru-Token")]string authKey, [FromBody]CustomerViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            SetUserAuth();
        //            if (UserAuth == null)
        //                return Ok(UnAuthorizeResponse.UnauthorizeResponse());

        //            CustomerModel customer = new CustomerModel();
        //            CopyProperty.CopyPropertiesTo(model, customer);
        //            customer.CustomerId = Guid.NewGuid().ToString();
        //            customer.CustomerCode = (string.IsNullOrEmpty(model.CustomerName) ? "" : model.CustomerName.GetInitial());

        //            customerService.Add(customer, UserAuth.UserId, model.SaveAsContact);

        //            json.SetError(false);
        //            json.AddAlert((int)HttpStatusCode.OK, "success");
        //            json.AddData(null);
        //        }
        //        catch (ApplicationException appEx)
        //        {
        //            json.SetError(false);
        //            json.AddAlert((int)HttpStatusCode.NotAcceptable, appEx.Message);
        //            json.AddData(null);
        //        }
        //        catch (Exception ex)
        //        {
        //            json.SetError(true);
        //            json.AddAlert((int)HttpStatusCode.InternalServerError, ex.Message);
        //            json.AddData(null);
        //        }
        //    }
        //    else
        //    {
        //        json.AddAlert((int)HttpStatusCode.NotAcceptable, "failed");
        //        json.AddData(ErrorHelper.Error(ModelState));
        //    }

        //    return Ok(json);
        //}

        [HttpPost]
        [Route("/api/customer/add")]
        public async Task<IActionResult> Add([FromHeader(Name = "X-Aru-Token")]string authKey, [FromForm]CustomerFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    SetUserAuth();

                    if (UserAuth == null)
                        return Ok(UnAuthorizeResponse.UnauthorizeResponse());

                    CustomerDetailModel customerModel = new CustomerDetailModel();
                    string IdCardBlobId = Guid.NewGuid().ToString();
                    string NPWPBlobId = Guid.NewGuid().ToString();
                    string BrandingPhotoBlobId = Guid.NewGuid().ToString();
                    string BrandPhotoBlobId = Guid.NewGuid().ToString();
                    CopyProperty.CopyPropertiesTo(model, customerModel);

                    if (model.PhotoIdCard != null)
                    {
                        try
                        {
                            var uploadPhotoResult = await azureStorageService.UploadAsync(IdCardBlobId, model.PhotoIdCard.OpenReadStream(), model.PhotoIdCard.ContentType);
                            customerModel.PhotoIdCardUrl = uploadPhotoResult;
                            customerModel.PhotoIdCardBlobId = IdCardBlobId;
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }

                    if (model.PhotoNPWP != null)
                    {
                        try
                        {
                            var uploadPhotoResult = await azureStorageService.UploadAsync(NPWPBlobId, model.PhotoNPWP.OpenReadStream(), model.PhotoNPWP.ContentType);
                            customerModel.PhotoNPWPUrl = uploadPhotoResult;
                            customerModel.PhotoNPWPBlobId = NPWPBlobId;
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }

                    if (model.BrandingPhoto != null)
                    {
                        try
                        {
                            var uploadPhotoResult = await azureStorageService.UploadAsync(BrandingPhotoBlobId, model.BrandingPhoto.OpenReadStream(), model.BrandingPhoto.ContentType);
                            customerModel.BrandingPhotoUrl = uploadPhotoResult;
                            customerModel.BrandingPhotoBlobId = BrandingPhotoBlobId;
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }

                    if (model.StorePhoto != null)
                    {
                        try
                        {
                            var uploadPhotoResult = await azureStorageService.UploadAsync(BrandPhotoBlobId, model.StorePhoto.OpenReadStream(), model.StorePhoto.ContentType);
                            customerModel.StorePhotoUrl = uploadPhotoResult;
                            customerModel.StorePhotoBlobId = BrandPhotoBlobId;
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }

                    customerService.AddCustomer(customerModel, UserAuth.UserId);

                    json.SetError(false);
                    json.AddAlert((int)HttpStatusCode.OK, "success");
                    json.AddData(null);
                }
                catch (ApplicationException appEx)
                {
                    json.SetError(false);
                    json.AddAlert((int)HttpStatusCode.NotAcceptable, appEx.Message);
                    json.AddData(null);
                }
                catch (Exception ex)
                {
                    json.SetError(true);
                    json.AddAlert((int)HttpStatusCode.InternalServerError, ex.Message);
                    json.AddData(null);
                }
            }
            else
            {
                json.AddAlert((int)HttpStatusCode.NotAcceptable, "failed");
                json.AddData(ErrorHelper.Error(ModelState));
            }

            return Ok(json);
        }
        [HttpPost]
        [Route("/api/customer/{id}/update")]
        public IActionResult Update([FromHeader(Name = "X-Aru-Token")]string authKey, string id, [FromBody]CustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    SetUserAuth();

                    if (UserAuth == null)
                        return Ok(UnAuthorizeResponse.UnauthorizeResponse());

                    CustomerModel customer = new CustomerModel();
                    CopyProperty.CopyPropertiesTo(model, customer);
                    customer.CustomerId = id;
                    customer.CustomerCode = (string.IsNullOrEmpty(model.CustomerName) ? "" : model.CustomerName.GetInitial());

                    customerService.Edit(customer, UserAuth.UserId);

                    json.SetError(false);
                    json.AddAlert((int)HttpStatusCode.OK, "success");
                    json.AddData(null);
                }
                catch (ApplicationException appEx)
                {
                    json.SetError(false);
                    json.AddAlert((int)HttpStatusCode.NotAcceptable, appEx.Message);
                    json.AddData(null);
                }
                catch (Exception ex)
                {
                    json.SetError(true);
                    json.AddAlert((int)HttpStatusCode.InternalServerError, ex.Message);
                    json.AddData(null);
                }
            }
            else
            {
                json.AddAlert((int)HttpStatusCode.NotAcceptable, "failed");
                json.AddData(ErrorHelper.Error(ModelState));
            }

            return Ok(json);
        }

        [HttpPost]
        [Route("/api/customer/{id}/delete")]
        public IActionResult Delete([FromHeader(Name = "X-Aru-Token")]string authKey, string id)
        {
            if (ModelState.IsValid)
            {
                try
                {   
                    SetUserAuth();

                    if (UserAuth == null)
                        return Ok(UnAuthorizeResponse.UnauthorizeResponse());

                    customerService.Delete(id, UserAuth.UserId);

                    json.SetError(false);
                    json.AddAlert((int)HttpStatusCode.OK, "success");
                    json.AddData(null);
                }
                catch (ApplicationException appEx)
                {
                    json.SetError(false);
                    json.AddAlert((int)HttpStatusCode.NotAcceptable, appEx.Message);
                    json.AddData(null);
                }
                catch (Exception ex)
                {
                    json.SetError(true);
                    json.AddAlert((int)HttpStatusCode.InternalServerError, ex.Message);
                    json.AddData(null);
                }
            }
            else
            {
                json.AddAlert((int)HttpStatusCode.NotAcceptable, "failed");
                json.AddData(ErrorHelper.Error(ModelState));
            }

            return Ok(json);
        }

        #region Contact Endpoint
        [HttpPost]
        [Route("/api/contact")]
        [ProducesResponseType(typeof(IEnumerable<ContactModel>), 200)]
        public IActionResult Contact([FromHeader(Name = "X-Aru-Token")]string authKey, [FromBody]ContactSearchViewModel request, int limit = 20, int offset = 0)
        {
            try
            {   
                SetUserAuth();

                if (UserAuth == null)
                    return Ok(UnAuthorizeResponse.UnauthorizeResponse());

                List<string> users = new List<string> { UserAuth.UserId };
                
                if (UserAuth.RoleCode.Equals(Domain.AppConstant.Role.SUPERVISOR))
                {
                    GetSalesByTerritory(UserAuth.TerritoryId.Distinct().ToList(), users);
                }

                var result = customerService.GetContact(request.Keyword, limit, offset, "CreatedDt", "desc", users);

                json.SetError(false);
                json.AddAlert((int)HttpStatusCode.OK, "success");
                json.AddData(result);
            }
            catch (ApplicationException appEx)
            {
                json.SetError(false);
                json.AddAlert((int)HttpStatusCode.NotAcceptable, appEx.Message);
                json.AddData(null);
            }
            catch (Exception ex)
            {
                json.SetError(true);
                json.AddAlert((int)HttpStatusCode.InternalServerError, ex.Message);
                json.AddData(null);
            }
            return Ok(json);
        }

        private void GetSalesByTerritory(List<string> list, List<string> users)
        {
            var agent = accountService.GetUserAgent(Domain.AppConstant.Role.SALES, list);

            if (agent != null)
                users.AddRange(agent.Select(x => x.UserId).ToList());
        }

        [HttpPost]
        [Route("/api/contact/add")]
        public IActionResult AddContact([FromHeader(Name = "X-Aru-Token")]string authKey, [FromBody]ContactModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    SetUserAuth();

                    if (UserAuth == null)
                        return Ok(UnAuthorizeResponse.UnauthorizeResponse());

                    ContactModel contact = new ContactModel();
                    CopyProperty.CopyPropertiesTo(model, contact);
                    contact.ContactId = Guid.NewGuid().ToString();

                    customerService.AddContact(contact, UserAuth.UserId);

                    json.SetError(false);
                    json.AddAlert((int)HttpStatusCode.OK, "success");
                    json.AddData(null);
                }
                catch (ApplicationException appEx)
                {
                    json.SetError(false);
                    json.AddAlert((int)HttpStatusCode.NotAcceptable, appEx.Message);
                    json.AddData(null);
                }
                catch (Exception ex)
                {
                    json.SetError(true);
                    json.AddAlert((int)HttpStatusCode.InternalServerError, ex.Message);
                    json.AddData(null);
                }
            }
            else
            {
                json.AddAlert((int)HttpStatusCode.NotAcceptable, "failed");
                json.AddData(ErrorHelper.Error(ModelState));
            }

            return Ok(json);
        }

        [HttpPost]
        [Route("/api/contact/{id}/update")]
        public IActionResult UpdateContact([FromHeader(Name = "X-Aru-Token")]string authKey, string id, [FromBody]ContactModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    SetUserAuth();
                    if (UserAuth == null)
                        return Ok(UnAuthorizeResponse.UnauthorizeResponse());

                    ContactModel contact = new ContactModel();
                    CopyProperty.CopyPropertiesTo(model, contact);
                    contact.ContactId = id;

                    customerService.EditContact(contact, UserAuth.UserId);

                    json.SetError(false);
                    json.AddAlert((int)HttpStatusCode.OK, "success");
                    json.AddData(null);
                }
                catch (ApplicationException appEx)
                {
                    json.SetError(false);
                    json.AddAlert((int)HttpStatusCode.NotAcceptable, appEx.Message);
                    json.AddData(null);
                }
                catch (Exception ex)
                {
                    json.SetError(true);
                    json.AddAlert((int)HttpStatusCode.InternalServerError, ex.Message);
                    json.AddData(null);
                }
            }
            else
            {
                json.AddAlert((int)HttpStatusCode.NotAcceptable, "failed");
                json.AddData(ErrorHelper.Error(ModelState));
            }

            return Ok(json);
        }
        [HttpPost]
        [Route("/api/contact/{id}/delete")]
        public IActionResult DeleteContact([FromHeader(Name = "X-Aru-Token")]string authKey, string id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    SetUserAuth();
                    if (UserAuth == null)
                        return Ok(UnAuthorizeResponse.UnauthorizeResponse());
                    
                    customerService.DeleteContact(id, UserAuth.UserId);

                    json.SetError(false);
                    json.AddAlert((int)HttpStatusCode.OK, "success");
                    json.AddData(null);
                }
                catch (ApplicationException appEx)
                {
                    json.SetError(false);
                    json.AddAlert((int)HttpStatusCode.NotAcceptable, appEx.Message);
                    json.AddData(null);
                }
                catch (Exception ex)
                {
                    json.SetError(true);
                    json.AddAlert((int)HttpStatusCode.InternalServerError, ex.Message);
                    json.AddData(null);
                }
            }
            else
            {
                json.AddAlert((int)HttpStatusCode.NotAcceptable, "failed");
                json.AddData(ErrorHelper.Error(ModelState));
            }

            return Ok(json);
        }
        #endregion
    }
}