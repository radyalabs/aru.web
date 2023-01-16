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
using Trisatech.MWorkforce.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Trisatech.MWorkforce.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Attendance")]
    public class AttendanceController : AppBaseController
    {
        private readonly IAssignmentService assignmentService;
        private readonly IAccountService accountService;
        private readonly IAzureStorageService azureStorageService;
        private Trisatech.AspNet.Common.Models.JsonEntity json;
        public AttendanceController(IAssignmentService assignmentService, IAccountService accountService, IAzureStorageService azureStorageService,IOptions<ApplicationSetting> options)
        :base(options){
            this.assignmentService = assignmentService;
            this.accountService = accountService;
            this.azureStorageService = azureStorageService;
            json = new Trisatech.AspNet.Common.Models.JsonEntity();
        }

        [HttpPost]
        [Route("check")]
        public IActionResult Check([FromHeader(Name = "X-Aru-Token")] string authKey)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    SetUserAuth();
                    if (UserAuth == null)
                        return Ok(UnAuthorizeResponse.UnauthorizeResponse());

                    short result = (short)accountService.Check(UserAuth.UserId, DateTime.UtcNow);

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
            }
            else
            {
                var messageError = ErrorHelper.Error(ModelState);

                json.SetError(false);
                json.AddAlert((int)HttpStatusCode.NotAcceptable, "");
                json.AddData(messageError);
            }
            return Ok(json);
        }

        [HttpPost]
        [Route("checkin")]
        public async Task<IActionResult> Checkin([FromHeader(Name = "X-Aru-Token")] string authKey, [FromForm] CheckinViewModel checkinViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    SetUserAuth();
                    if (UserAuth == null)
                        return Ok(UnAuthorizeResponse.UnauthorizeResponse());

                    DateTime utcNow = DateTime.UtcNow;
                    var result = accountService.Check(UserAuth.UserId, 
                        (checkinViewModel.Mode == CheckInMode.OFFLINE? checkinViewModel.StartTime.ToUniversalTime() : utcNow));
                    if(result == AttendanceStatus.CHECKIN)
                    {
                        json.SetError(false);
                        json.AddAlert((int)HttpStatusCode.Accepted, "already checked in.");
                        json.AddData(null);

                        return Ok(json);
                    }

                    if(result == AttendanceStatus.CHECKOUT) 
                    {
                        json.SetError(false);
                        json.AddAlert((int)HttpStatusCode.Accepted, "already checkout today. please checkin tomorrow");
                        json.AddData(null);

                        return Ok(json);
                    }

                    if (UserAuth.RoleCode == Domain.AppConstant.Role.DRIVER)
                    {
                        string attachmentId = string.Empty;
                        string attachmentUrl = string.Empty;

                        if (checkinViewModel.Attachment != null)
                        {
                            string attachmentContentType = checkinViewModel.Attachment.ContentType.ToLower();
                            if (attachmentContentType != "image/png" && attachmentContentType != "image/jpg" && attachmentContentType != "image/jpeg")
                            {
                                if (checkinViewModel.Attachment == null)
                                {
                                    json = new Trisatech.AspNet.Common.Models.JsonEntity()
                                    {
                                        Error = false,
                                        Data = false,
                                        Alerts = new Trisatech.AspNet.Common.Models.JsonAlert { Code = (int)HttpStatusCode.BadRequest, Message = "Cannot accept attachment file type." }
                                    };

                                    return Ok(json);
                                }
                            }

                            if (checkinViewModel.Attachment.Length > 10000000)
                            {

                                json = new Trisatech.AspNet.Common.Models.JsonEntity()
                                {
                                    Error = false,
                                    Data = false,
                                    Alerts = new Trisatech.AspNet.Common.Models.JsonAlert { Code = (int)HttpStatusCode.BadRequest, Message = "Attachment exceeds the maximum limit." }
                                };

                                return Ok(json);
                            }

                            try
                            {
                                attachmentId = Guid.NewGuid().ToString();
                                attachmentUrl = await azureStorageService.UploadAsync(attachmentId, checkinViewModel.Attachment.OpenReadStream(), checkinViewModel.Attachment.ContentType);
                            }
                            catch
                            {
                                throw new Exception("Gagal mengirimkan gambar");
                            }
                        }

                        accountService.CheckIn(UserAuth.UserId, 
                            checkinViewModel.Latitude, 
                            checkinViewModel.Longitude, 
                            (checkinViewModel.Mode == CheckInMode.OFFLINE ? checkinViewModel.StartTime.ToUniversalTime() : utcNow), 
                            checkinViewModel.Comment, "", 
                            checkinViewModel.Distance, 
                            attachmentId, 
                            attachmentUrl);
                    }
                    else if (UserAuth.RoleCode == Domain.AppConstant.Role.SUPERVISOR)
                    {
                        accountService.CheckIn(UserAuth.UserId,
                            checkinViewModel.Latitude,
                            checkinViewModel.Longitude,
                            (checkinViewModel.Mode == CheckInMode.OFFLINE ? checkinViewModel.StartTime.ToUniversalTime() : DateTime.UtcNow),
                            checkinViewModel.Comment,
                            checkinViewModel.UserId);

                        var taskCopyTask = assignmentService.CopyTask(checkinViewModel.UserId, UserAuth.UserId, utcNow);

                        taskCopyTask.Wait();
                    }
                    else
                        accountService.CheckIn(UserAuth.UserId, checkinViewModel.Latitude, checkinViewModel.Longitude, (checkinViewModel.Mode == CheckInMode.OFFLINE ? checkinViewModel.StartTime.ToUniversalTime() : DateTime.UtcNow), checkinViewModel.Comment);

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
                var messageError = ErrorHelper.Error(ModelState);

                json.SetError(false);
                json.AddAlert((int)HttpStatusCode.NotAcceptable, "");
                json.AddData(messageError);
            }
            return Ok(json);
        }

        [HttpPost]
        [Route("checkout")]
        public async Task<IActionResult> Checkout([FromHeader(Name = "X-Aru-Token")] string authKey, [FromForm] CheckoutViewModel checkoutViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    SetUserAuth();
                    if (UserAuth == null)
                        return Ok(UnAuthorizeResponse.UnauthorizeResponse());

                    var result = accountService.Check(UserAuth.UserId, (checkoutViewModel.Mode == CheckInMode.OFFLINE ? 
                        checkoutViewModel.EndTime.ToUniversalTime() : DateTime.UtcNow.AddHours(7)));
                    if (result == AttendanceStatus.CHECKOUT)
                    {
                        json.SetError(false);
                        json.AddAlert((int)HttpStatusCode.Accepted, "already checked out.");
                        json.AddData(null);
    
                        return Ok(json);
                    }
                    #region Attachment
                    string attachmentId = string.Empty;
                    string attachmentUrl = string.Empty;

                    if (checkoutViewModel.Attachment != null)
                    {
                        string attachmentContentType = checkoutViewModel.Attachment.ContentType.ToLower();
                        if (attachmentContentType != "image/png" && attachmentContentType != "image/jpg" && attachmentContentType != "image/jpeg")
                        {
                            if (checkoutViewModel.Attachment == null)
                            {
                                json = new Trisatech.AspNet.Common.Models.JsonEntity()
                                {
                                    Error = false,
                                    Data = false,
                                    Alerts = new Trisatech.AspNet.Common.Models.JsonAlert { Code = (int)HttpStatusCode.BadRequest, Message = "Cannot accept attachment file type." }
                                };

                                return Ok(json);
                            }
                        }

                        if (checkoutViewModel.Attachment.Length > 10000000)
                        {

                            json = new Trisatech.AspNet.Common.Models.JsonEntity()
                            {
                                Error = false,
                                Data = false,
                                Alerts = new Trisatech.AspNet.Common.Models.JsonAlert { Code = (int)HttpStatusCode.BadRequest, Message = "Attachment exceeds the maximum limit." }
                            };

                            return Ok(json);
                        }

                        try
                        {
                            attachmentId = Guid.NewGuid().ToString();
                            attachmentUrl = await azureStorageService.UploadAsync(attachmentId, checkoutViewModel.Attachment.OpenReadStream(), checkoutViewModel.Attachment.ContentType);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }

                    #endregion

                    accountService.CheckOut(UserAuth.UserId,
                        checkoutViewModel.Latitude,
                        checkoutViewModel.Longitude, 
                        (checkoutViewModel.Mode == CheckInMode.OFFLINE ? checkoutViewModel.EndTime.ToUniversalTime() : DateTime.UtcNow),
                        checkoutViewModel.Comment, "",
                        checkoutViewModel.Distance,
                        attachmentId,
                        attachmentUrl);

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
                var messageError = ErrorHelper.Error(ModelState);

                json.SetError(false);
                json.AddAlert((int)HttpStatusCode.NotAcceptable, "");
                json.AddData(messageError);
            }
            return Ok(json);
        }
    }
}