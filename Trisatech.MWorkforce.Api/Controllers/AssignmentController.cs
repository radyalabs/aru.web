using Microsoft.AspNetCore.Mvc;
using Trisatech.MWorkforce.Business.Interfaces;
using Trisatech.MWorkforce.Api.ViewModels;
using System.Net;
using Trisatech.MWorkforce.Api.Helpers;
using Trisatech.MWorkforce.Business.Entities;
using Trisatech.MWorkforce.Api.Model;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using System.IO;
using Trisatech.MWorkforce.Infrastructure.Interfaces;
using Trisatech.MWorkforce.Api.Interfaces;
using Trisatech.MWorkforce.Infrastructure.GoogleMapsAPI;
using Trisatech.MWorkforce.Api.Services;
using Trisatech.MWorkforce.Domain;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Trisatech.MWorkforce.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AssignmentController : AppBaseController
    {
        private readonly IAssignmentService assignmentService;
        private readonly IAccountService accountService;
        private readonly IAzureStorageService azureStorageService;
        private readonly ISurveyService surveyService;
        private readonly IRefService refService;
        private readonly ISequencerNumberService sequencerNumberService;
        private Trisatech.AspNet.Common.Models.JsonEntity json;
        private readonly GoogleMapsAPI googleMapsAPIService;

        public AssignmentController(IAssignmentService assignmentService,
            IAccountService accountService,
            IAzureStorageService azureStorageService,
            ISurveyService surveyService,
            IRefService refService,
            ISequencerNumberService sequencerNumberService,
            GoogleMapsAPI googleMapsAPI,
            IOptions<ApplicationSetting> options):base(options)
        {
            this.assignmentService = assignmentService;
            this.accountService = accountService;
            this.azureStorageService = azureStorageService;
            this.surveyService = surveyService;
            this.refService = refService;
            this.sequencerNumberService = sequencerNumberService;
            this.googleMapsAPIService = googleMapsAPI;
            json = new Trisatech.AspNet.Common.Models.JsonEntity();
        }

        #region List & Detail Assignment
        // GET: api/<controller>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Trisatech.MWorkforce.Business.Entities.AssignmentModel>), 200)]
        public IActionResult Get([FromHeader(Name = "X-Aru-Token")] string authKey, DateTime? from, DateTime? to, AssignmentProgressStatus status = AssignmentProgressStatus.INPROGRESS, int limit = 10, int offset = 0)
        {
            try
            {
                SetUserAuth();

                if (UserAuth == null)
                    return Ok(UnAuthorizeResponse.UnauthorizeResponse());

                var assignments = assignmentService.GetAssignments(UserAuth.UserId, (int)status, limit, offset, "AssignmentDate", "desc", from, to);
                
                json = new Trisatech.AspNet.Common.Models.JsonEntity()
                {
                    Error = false,
                    Data = assignments,
                    Alerts = new Trisatech.AspNet.Common.Models.JsonAlert { Code = 200, Message = "Success" }
                };
            }
            catch(Exception ex)
            {
                json = new Trisatech.AspNet.Common.Models.JsonEntity()
                {
                    Error = true,
                    Data = null,
                    Alerts = new Trisatech.AspNet.Common.Models.JsonAlert { Code = (int)HttpStatusCode.InternalServerError, Message = ex.Message }
                };
            }

            return Ok(json);
        }

        // GET: api/<controller>
        [HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(IEnumerable<Trisatech.MWorkforce.Business.Entities.AssignmentModel>), 200)]
        public IActionResult Get([FromHeader(Name = "X-Aru-Token")] string authKey, DateTime? from, DateTime? to, AssignmentProgressStatus status = AssignmentProgressStatus.INPROGRESS)
        {
            try
            {
                SetUserAuth();

                if (UserAuth == null)
                    return Ok(UnAuthorizeResponse.UnauthorizeResponse());
                
                long? totalAssignments = assignmentService.GetAssignmentsCount(UserAuth.UserId, (int)status, 0, 0, "AssignmentDate", "desc", from, to);
                
                var assignments = assignmentService.GetAssignmentDetail(UserAuth.UserId, (int)status, totalAssignments == null? 100 : (int)totalAssignments, 0, "AssignmentDate", "desc", from, to);

                json = new Trisatech.AspNet.Common.Models.JsonEntity()
                {
                    Error = false,
                    Data = assignments,
                    Alerts = new Trisatech.AspNet.Common.Models.JsonAlert { Code = 200, Message = "Success" }
                };
            }
            catch (Exception ex)
            {
                json = new Trisatech.AspNet.Common.Models.JsonEntity()
                {
                    Error = true,
                    Data = null,
                    Alerts = new Trisatech.AspNet.Common.Models.JsonAlert { Code = (int)HttpStatusCode.InternalServerError, Message = ex.Message }
                };
            }

            return Ok(json);
        }

        // GET : api/<controlller>
        [HttpGet]
        [Route("calendar/{from}/{to}")]
        [ProducesResponseType(typeof(IEnumerable<Trisatech.MWorkforce.Business.Entities.AssignmentModel>), 200)]
        public IActionResult Calender([FromHeader(Name = "X-Aru-Token")] string authKey, DateTime from, DateTime? to, AssignmentProgressStatus status = AssignmentProgressStatus.INPROGRESS, int limit = 10, int offset = 0)
        {
            try
            {
                SetUserAuth();
                if (UserAuth == null)
                    return Ok(UnAuthorizeResponse.UnauthorizeResponse());

                var assignments = assignmentService.GetAssignmentsByDate(UserAuth.UserId, from, to, (short)status, limit, offset);
                
                json = new Trisatech.AspNet.Common.Models.JsonEntity()
                {
                    Error = false,
                    Data = assignments,
                    Alerts = new Trisatech.AspNet.Common.Models.JsonAlert { Code = 200, Message = "Success" }
                };
            }
            catch(Exception ex)
            {
                json = new Trisatech.AspNet.Common.Models.JsonEntity()
                {
                    Error = true,
                    Data = null,
                    Alerts = new Trisatech.AspNet.Common.Models.JsonAlert { Code = (int)HttpStatusCode.InternalServerError, Message = ex.Message }
                };
            }

            return Ok(json);
        }

        // POST api/<controller>/search
        [HttpPost]
        [Route("search")]
        [ProducesResponseType(typeof(IEnumerable<Trisatech.MWorkforce.Business.Entities.AssignmentModel>), 200)]
        public IActionResult Search([FromHeader(Name = "X-Aru-Token")] string authKey, [FromBody]AssignmentSearchViewModel viewModel, AssignmentProgressStatus status = AssignmentProgressStatus.INPROGRESS, int limit = 10, int offset = 0)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    SetUserAuth();
                    if (UserAuth == null)
                        return Ok(UnAuthorizeResponse.UnauthorizeResponse());

                    var assignments = assignmentService.Search(UserAuth.UserId, viewModel.Keyword, (short)status, viewModel.Date, limit, offset);
                    
                    json = new Trisatech.AspNet.Common.Models.JsonEntity()
                    {
                        Error = false,
                        Data = assignments,
                        Alerts = new Trisatech.AspNet.Common.Models.JsonAlert { Code = 200, Message = "Success" }
                    };
                }
                catch(Exception ex)
                {
                    json = new Trisatech.AspNet.Common.Models.JsonEntity()
                    {
                        Error = true,
                        Data = null,
                        Alerts = new Trisatech.AspNet.Common.Models.JsonAlert { Code = (int)HttpStatusCode.InternalServerError, Message = ex.Message }
                    };
                }
            }
            else
            {
                json.SetError(false);
                json.AddData(null);
                json.AddAlert((int)System.Net.HttpStatusCode.NotAcceptable, "Not acceptable");
            }

            return Ok(json);
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Trisatech.MWorkforce.Business.Entities.AssignmentModel), 200)]
        public IActionResult Get([FromHeader(Name = "X-Aru-Token")] string authKey, string id)
        {
            try
            {
                DateTime utcNow = DateTime.UtcNow;

                SetUserAuth();
                if (UserAuth == null)
                    return Ok(UnAuthorizeResponse.UnauthorizeResponse());

                if (string.IsNullOrEmpty(id))
                {
                    json = new Trisatech.AspNet.Common.Models.JsonEntity()
                    {
                        Error = false,
                        Data = id,
                        Alerts = new Trisatech.AspNet.Common.Models.JsonAlert { Code = (int)HttpStatusCode.BadRequest, Message = "Bad request" }
                    };

                    return Ok(json);
                }
                
                AssignmentModel assignment = null;
                assignment = assignmentService.Detail(UserAuth.UserId, id);

                if (assignment == null)
                {
                    json = new Trisatech.AspNet.Common.Models.JsonEntity()
                    {
                        Error = false,
                        Data = id,
                        Alerts = new Trisatech.AspNet.Common.Models.JsonAlert { Code = (int)HttpStatusCode.BadRequest, Message = "Bad request" }
                    };

                    return Ok(json);
                }
                
                //get all survey
                var survey = surveyService.Get(utcNow);
                assignment.Survey = survey;

                json = new Trisatech.AspNet.Common.Models.JsonEntity()
                {
                    Error = false,
                    Data = assignment,
                    Alerts = new Trisatech.AspNet.Common.Models.JsonAlert { Code = 200, Message = "Success" }
                };
            }
            catch(Exception ex)
            {
                json = new Trisatech.AspNet.Common.Models.JsonEntity()
                {
                    Error = true,
                    Data = null,
                    Alerts = new Trisatech.AspNet.Common.Models.JsonAlert { Code = (int)HttpStatusCode.InternalServerError, Message = ex.Message }
                };
            }

            return Ok(json);
        }

        #endregion
        
        [HttpPost("{assignment_id}/start")]
        public async Task<IActionResult> Start([FromHeader(Name = "X-Aru-Token")]string authKey, string assignment_id,
            [FromBody] AssignmentStartViewModel assignmentStartViewModel,
            [FromQuery(Name = "is_online")]bool isOnline = true)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    SetUserAuth();
                    if (UserAuth == null)
                        return Ok(UnAuthorizeResponse.UnauthorizeResponse());
                    
                    if(!CheckAllowToStart(UserAuth.UserId, DateTime.UtcNow))
                    {
                        json.SetError(false);
                        json.AddAlert((int)HttpStatusCode.NotAcceptable, "Tidak dapat memulai tugas ini, karena ada tugas yang belum diselesaikan.");
                        json.AddData(null);

                        return Ok(json);
                    }

                    DateTime utcNow = DateTime.UtcNow;
                    int duration = 0;
                    int salesTime = 0;
                    int googleTime = 0;
                    DateTime startTime = isOnline || (!isOnline && assignmentStartViewModel.StartDate == null) ? DateTime.UtcNow : assignmentStartViewModel.StartDate.Value.AddHours(-7);
                    (salesTime, duration, googleTime) =  await GetTravelTime(assignmentStartViewModel, utcNow);
                    
                    assignmentService.Start(UserAuth.UserId, 
                        assignment_id, 
                        assignmentStartViewModel.Latitude,
                        assignmentStartViewModel.Longitude, 
                        startTime,
                        "AGENT_STARTED", 
                        (duration < 0 ? 0 : duration),
                        salesTime,
                        googleTime
                        );
                    
                    json.SetError(false);
                    json.AddAlert((int)HttpStatusCode.OK, "success");
                    json.AddData(null);
                }
                catch(ApplicationException appEx)
                {
                    json.SetError(false);
                    json.AddAlert((int)HttpStatusCode.NotAcceptable, appEx.Message);
                    json.AddData(null);
                }
                catch(Exception ex)
                {
                    json.SetError(true);
                    json.AddAlert((int)HttpStatusCode.BadRequest, ex.Message);
                    json.AddData(null);
                }
            }
            else
            {
                var errMessages = ErrorHelper.Error(ModelState);
                string errorMessage = string.Join(", ", errMessages.ToArray());
                json.SetError(false);
                json.AddAlert((int)HttpStatusCode.BadRequest, errorMessage);
                json.AddData(null);
            }

            return Ok(json);
        }

        private async Task<(int, int, int)> GetTravelTime(AssignmentStartViewModel assignmentStartViewModel, DateTime utcNow)
        {
            int salesTime = 0;
            int duration = 0;
            int googleTime = 0;
            
            try
            {
                if (assignmentStartViewModel.Latitude != 0 && assignmentStartViewModel.Longitude != 0)
                {
                    var assignmentGroup = accountService.Get(UserAuth.UserId, utcNow);
                    var userActivity = accountService.GetUserActivity(UserAuth.UserId, utcNow);

                    if (userActivity != null && (userActivity.Latitude != 0 && userActivity.Longitude != 0))
                    {
                        googleTime = await googleMapsAPIService.GetTravelDuration(
                            new LatLng(userActivity.Latitude, userActivity.Longitude), 
                            new LatLng(assignmentStartViewModel.Latitude, assignmentStartViewModel.Longitude)
                            );

                        salesTime = (int)utcNow.Subtract(userActivity.CreatedDt).TotalSeconds;

                        if (googleTime <= 0)
                            googleTime = salesTime;

                        duration = salesTime - googleTime;
                    }
                    else
                    {
                        if (assignmentGroup != null && (assignmentGroup.StartLatitude != null && assignmentGroup.StartLongitude != null))
                        {
                            googleTime = await googleMapsAPIService.GetTravelDuration(new LatLng(assignmentGroup.StartLatitude ?? 0, assignmentGroup.StartLongitude ?? 0), new LatLng(assignmentStartViewModel.Latitude, assignmentStartViewModel.Longitude));
                            salesTime = (int)utcNow.Subtract(assignmentGroup.StartTime).TotalSeconds;
                            if (googleTime <= 0)
                                googleTime = salesTime;

                            duration = salesTime - googleTime;
                        }
                    }

                    if (assignmentGroup != null)
                        accountService.UpdateAssignmentGroup(assignmentGroup.AssignmentGroupId, (duration < 0 ? 0 : duration));
                }
            }
            catch
            {
                //do nothing
            }

            return (salesTime, duration, googleTime);
        }

        private bool CheckAllowToStart(string userId, DateTime utcNow)
        {
            if (string.IsNullOrEmpty(userId))
                return false;

            return assignmentService.AllowToStartAssignment(userId, utcNow);
        }

        [HttpPost("{assignment_id}/complete")]
        public async Task<IActionResult> Complete([FromHeader(Name = "X-Aru-Token")]string authKey, string assignment_id, 
            [FromForm]AssignmentCompleteViewModel completeViewModel, [FromQuery(Name = "is_online")]bool isOnline = true)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    SetUserAuth();
                    if (UserAuth == null)
                        return Ok(UnAuthorizeResponse.UnauthorizeResponse());

                    string attachmentUrl = "";
                    string attachBlobId = Guid.NewGuid().ToString();

                    if(completeViewModel.file != null)
                    {
                        string attachmentContentType = completeViewModel.file.ContentType.ToLower();
                        if (attachmentContentType != "image/png" && attachmentContentType != "image/jpg" && attachmentContentType != "image/jpeg")
                        {
                            if (completeViewModel.file== null)
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

                        if (completeViewModel.file.Length > 10000000)
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
                            attachmentUrl = await azureStorageService.UploadAsync(attachBlobId, completeViewModel.file.OpenReadStream(), completeViewModel.file.ContentType);
                        }catch(Exception ex)
                        {
                            throw ex;
                        }
                    }

                    DateTime processedTime = isOnline || (!isOnline
                        && completeViewModel.ProcessedTime == null) ? DateTime.UtcNow : completeViewModel.ProcessedTime.Value.ToUniversalTime();
                    assignmentService.Complete(UserAuth.UserId, 
                        assignment_id, 
                        completeViewModel.Latitude, 
                        completeViewModel.Longitude, 
                        completeViewModel.Remarks, 
                        processedTime, 
                        completeViewModel.AssignmentStatus, 
                        completeViewModel.ReasonCode, 
                        attachmentUrl, 
                        (string.IsNullOrEmpty(attachmentUrl)? "":attachBlobId));

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
                    json.AddAlert((int)HttpStatusCode.BadRequest, ex.Message);
                    json.AddData(null);
                }
            }
            else
            {
                var errMessages = ErrorHelper.Error(ModelState);
                string errorMessage = string.Join(", ", errMessages.ToArray());

                json.SetError(false);
                json.AddAlert((int)HttpStatusCode.BadRequest, errorMessage);
                json.AddData(null);
            }

            return Ok(json);
        }
            
        [HttpGet]
        [Route("status")]
        [ProducesResponseType(typeof(IEnumerable<AssignmentStatusModel>), 200)]
        public IActionResult GetAssignmentStatus([FromHeader(Name = "X-Aru-Token")]string authKey)
        {
            try
            {
                SetUserAuth();
                if (UserAuth == null)
                    return Ok(UnAuthorizeResponse.UnauthorizeResponse());

                var result = assignmentService.GetAssignmentStatus();

                json.SetError(false);
                json.AddAlert((int)HttpStatusCode.OK, "success");
                json.AddData(result);
            }
            catch (Exception ex)
            {
                json.SetError(true);
                json.AddAlert((int)HttpStatusCode.InternalServerError, ex.Message);
                json.AddData(null);
            }

            return Ok(json);
        }

        [HttpGet]
        [Route("reason")]
        [ProducesResponseType(typeof(IEnumerable<ReasonViewModel>), 200)]
        public IActionResult GetRefReasonAssignment([FromHeader(Name = "X-Aru-Token")]string authKey)
        {
            try
            {
                SetUserAuth();
                if (UserAuth == null)
                    return Ok(UnAuthorizeResponse.UnauthorizeResponse());

                var result = refService.GetReasons();

                json.SetError(false);
                json.AddAlert((int)HttpStatusCode.OK, "success");
                json.AddData(result);
            }
            catch (Exception ex)
            {
                json.SetError(true);
                json.AddAlert((int)HttpStatusCode.InternalServerError, ex.Message);
                json.AddData(null);
            }

            return Ok(json);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromHeader(Name = "X-Aru-Token")]string authKey, [FromForm]GasolineViewModel viewModel, [FromQuery] Domain.Entities.AssignmentType type = Domain.Entities.AssignmentType.GASOLINE)
        {
            if (ModelState.IsValid)
            {
                string attachmentUrl = "";
                string attchmentUrl1 = "";
                string attachmentUrl2 = "";
                string attachBlobId = Guid.NewGuid().ToString();
                string attachBlobId1 = Guid.NewGuid().ToString();
                string attachBlobId2 = Guid.NewGuid().ToString();

                try
                {
                    SetUserAuth();
                    if (UserAuth == null)
                        return Ok(UnAuthorizeResponse.UnauthorizeResponse());

                    #region Attachment
                    if (viewModel.Attachment != null)
                    {
                        string attachmentContentType = viewModel.Attachment.ContentType.ToLower();
                        if (attachmentContentType != "image/png" && attachmentContentType != "image/jpg" && attachmentContentType != "image/jpeg")
                        {
                            if (viewModel.Attachment == null)
                            {
                                json = new Trisatech.AspNet.Common.Models.JsonEntity()
                                {
                                    Error = false,
                                    Data = false,
                                    Alerts = new Trisatech.AspNet.Common.Models.JsonAlert { Code = (int)HttpStatusCode.BadRequest, Message = "Cannot attachment file type." }
                                };

                                return Ok(json);
                            }
                        }

                        if (viewModel.Attachment.Length > 10000000)
                        {

                            json = new Trisatech.AspNet.Common.Models.JsonEntity()
                            {
                                Error = false,
                                Data = false,
                                Alerts = new Trisatech.AspNet.Common.Models.JsonAlert { Code = (int)HttpStatusCode.BadRequest, Message = "Attachment photo file exceeds the maximum limit." }
                            };

                            return Ok(json);
                        }

                        try
                        {
                            attachmentUrl = await azureStorageService.UploadAsync(attachBlobId, viewModel.Attachment.OpenReadStream(), viewModel.Attachment.ContentType);
                        }
                        catch (Exception ex)
                        {
                            throw new ApplicationException(ex.Message);
                        }
                    }
                    #endregion

                    #region Attachment 1
                    if (viewModel.Attachment1 != null)
                    {
                        string attachmentContentType = viewModel.Attachment1.ContentType.ToLower();
                        if (attachmentContentType != "image/png" && attachmentContentType != "image/jpg" && attachmentContentType != "image/jpeg")
                        {
                            if (viewModel.Attachment1 == null)
                            {
                                json = new Trisatech.AspNet.Common.Models.JsonEntity()
                                {
                                    Error = false,
                                    Data = false,
                                    Alerts = new Trisatech.AspNet.Common.Models.JsonAlert { Code = (int)HttpStatusCode.BadRequest, Message = "Cannot attachment 1 file type." }
                                };

                                return Ok(json);
                            }
                        }

                        if (viewModel.Attachment1.Length > 10000000)
                        {

                            json = new Trisatech.AspNet.Common.Models.JsonEntity()
                            {
                                Error = false,
                                Data = false,
                                Alerts = new Trisatech.AspNet.Common.Models.JsonAlert { Code = (int)HttpStatusCode.BadRequest, Message = "Attachment 1 file exceeds the maximum limit." }
                            };

                            return Ok(json);
                        }

                        try
                        {
                            attachmentUrl = await azureStorageService.UploadAsync(attachBlobId, viewModel.Attachment1.OpenReadStream(), viewModel.Attachment1.ContentType);
                        }
                        catch (Exception ex)
                        {
                            //do nothing
                            //throw new ApplicationException(ex.Message);
                        }
                    }
                    #endregion

                    #region Attachment 2
                    if (viewModel.Attachment2 != null)
                    {
                        string attachmentContentType = viewModel.Attachment2.ContentType.ToLower();
                        if (attachmentContentType != "image/png" && attachmentContentType != "image/jpg" && attachmentContentType != "image/jpeg")
                        {
                            if (viewModel.Attachment2 == null)
                            {
                                json = new Trisatech.AspNet.Common.Models.JsonEntity()
                                {
                                    Error = false,
                                    Data = false,
                                    Alerts = new Trisatech.AspNet.Common.Models.JsonAlert { Code = (int)HttpStatusCode.BadRequest, Message = "Cannot attachment 2 file type." }
                                };

                                return Ok(json);
                            }
                        }

                        if (viewModel.Attachment2.Length > 10000000)
                        {

                            json = new Trisatech.AspNet.Common.Models.JsonEntity()
                            {
                                Error = false,
                                Data = false,
                                Alerts = new Trisatech.AspNet.Common.Models.JsonAlert { Code = (int)HttpStatusCode.BadRequest, Message = "Attachment 2 file exceeds the maximum limit." }
                            };

                            return Ok(json);
                        }

                        try
                        {
                            attachmentUrl = await azureStorageService.UploadAsync(attachBlobId, viewModel.Attachment2.OpenReadStream(), viewModel.Attachment2.ContentType);
                        }
                        catch (Exception ex)
                        {
                            //do nothing
                            //throw new ApplicationException(ex.Message);
                        }
                    }
                    #endregion

                    CreateGasolineModel createGasoline = new CreateGasolineModel();

                    createGasoline.AssignmentId = Guid.NewGuid().ToString();
                    createGasoline.AssignmentCode = $"G{DateTime.UtcNow:yyyyMMdd}{sequencerNumberService.SequenceNumber}";
                    createGasoline.AssignmentName = "Isi bahan bakar";
                    createGasoline.AssignmentStatusCode = AppConstant.AssignmentStatus.TASK_COMPLETED;
                    createGasoline.AssignmentDate = viewModel.EventDate.ToUniversalTime();
                    createGasoline.Latitude = viewModel.Latitude;
                    createGasoline.Longitude = viewModel.Longitude;
                    createGasoline.Address = viewModel.Address;
                    createGasoline.Note = viewModel.Remarks;
                    createGasoline.AssignmentType = Trisatech.MWorkforce.Domain.Entities.AssignmentType.GASOLINE;
                    createGasoline.Remarks = "Pengisian bensin dilakukan oleh pengendara (driver)";

                    createGasoline.AssignmentDetailId = Guid.NewGuid().ToString();
                    createGasoline.StartTime = viewModel.EventDate.ToUniversalTime();
                    createGasoline.EndTime = viewModel.EventDate.ToUniversalTime();
                    createGasoline.UserId = UserAuth.UserId;
                    createGasoline.UserCode = UserAuth.UserCode;
                    createGasoline.Attachment = attachmentUrl;
                    createGasoline.Attachment1 = attchmentUrl1;
                    createGasoline.Attachment2 = attachmentUrl2;
                    createGasoline.AttachmentBlobId = attachBlobId;
                    createGasoline.AttachmentBlobId1 = attachBlobId1;
                    createGasoline.AttachmentBlobId2 = attachBlobId2;

                    assignmentService.CreateAssignment(createGasoline, UserAuth.UserId);

                    json.SetError(false);
                    json.AddAlert((int)HttpStatusCode.OK, "success");
                    json.AddData(null);
                }
                catch (ApplicationException appEx)
                {
                    await azureStorageService.DeleteAsync(attachBlobId);
                    json.SetError(false);
                    json.AddAlert((int)HttpStatusCode.NotAcceptable, appEx.Message);
                    json.AddData(null);
                }
                catch (Exception ex)
                {
                    await azureStorageService.DeleteAsync(attachBlobId);
                    json.SetError(true);
                    json.AddAlert((int)HttpStatusCode.BadRequest, ex.Message);
                    json.AddData(null);
                }
            }
            else
            {
                var errMessages = ErrorHelper.Error(ModelState);
                string errorMessage = string.Join(", ", errMessages.ToArray());

                json.SetError(false);
                json.AddAlert((int)HttpStatusCode.BadRequest, errorMessage);
                json.AddData(null);
            }

            return Ok(json);
        }
    }
}
