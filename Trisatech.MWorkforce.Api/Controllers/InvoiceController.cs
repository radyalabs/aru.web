using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Trisatech.MWorkforce.Api.Helpers;
using Trisatech.MWorkforce.Api.Model;
using Trisatech.MWorkforce.Api.ViewModels;
using Trisatech.MWorkforce.Business.Entities;
using Trisatech.MWorkforce.Business.Interfaces;
using Trisatech.MWorkforce.Domain.Entities;
using Trisatech.MWorkforce.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Trisatech.AspNet.Common.Helpers;

namespace Trisatech.MWorkforce.Api.Controllers
{
    [AruAuthorize]
    [Produces("application/json")]
    [Route("api/invoice")]
    public class InvoiceController : AppBaseController
    {
        private readonly IAssignmentService assignmentService;
        private readonly IInvoiceService invoiceService;
        private readonly IAzureStorageService azureStorageService;
        private readonly IAuthenticationService authenticationService;
        private readonly TwilioSetting twilioSetting;
        private Trisatech.AspNet.Common.Models.JsonEntity json;
        public InvoiceController(IAssignmentService assignmentService, 
            IInvoiceService invoiceService, IAuthenticationService authentication, 
            IAzureStorageService azureStorageService, IOptions<ApplicationSetting> options, IOptions<TwilioSetting> optionsTwilio):base(options)
        {
            this.invoiceService = invoiceService; 
            this.assignmentService = assignmentService;
            this.azureStorageService = azureStorageService;
            this.authenticationService = authentication;
            twilioSetting = optionsTwilio.Value;
        }

        [Route("{assignment_id}/otp")]
        [HttpPost]
        public IActionResult RequestOtp([FromHeader(Name = "X-Aru-Token")] string authKey, string assignment_id, [FromBody]RequestOtpViewModel viewModel)
        {
            try
            {
                string otpMessage = "Yth cust. {0} terima kasih atas pembayaran nota senilai {1} dalam bentuk {2}. Kode verifikasi transaksi ini {3}";

                SetUserAuth();

                if (UserAuth == null)
                    return Ok(UnAuthorizeResponse.UnauthorizeResponse());

                string rndValue = TextHelper.GetRandNumeric(6);

                var assignment = assignmentService.Detail(UserAuth.UserId, assignment_id);
                if(assignment == null)
                {
                    json = new Trisatech.AspNet.Common.Models.JsonEntity
                    {
                        Error = false,
                        Data = null,
                        Alerts = new Trisatech.AspNet.Common.Models.JsonAlert { Code = (int)HttpStatusCode.BadRequest, Message = "Assignment not found" }
                    };

                    return Ok(json);
                }

                authenticationService.RequestOtp("PAYMENT", assignment_id, rndValue, DateTime.UtcNow.AddSeconds(60), UserAuth.UserId);

                if (string.IsNullOrEmpty(viewModel.PaymentMethod))
                {
                    viewModel.PaymentMethod = "Cash/Transfer/Giro";
                }

                if (twilioSetting.Enable)
                {
                    try
                    {
                        Infrastructure.Services.SmsService smsService = new Infrastructure.Services.SmsService(twilioSetting.AccountSid, twilioSetting.AuthToken, twilioSetting.Sender);
                        smsService.SendByTwilio(assignment.Contact.ContactNumber, string.Format(otpMessage, assignment.AssignmentName, viewModel.TotalPembayaran.ToString("C", new CultureInfo("id-ID")), viewModel.PaymentMethod, rndValue));
                    }
                    catch { }
                }

                json = new Trisatech.AspNet.Common.Models.JsonEntity()
                {
                    Error = false,
                    Data = rndValue,
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

        private async Task<PaymentModel> CreateGiroPaymentModel(decimal? GiroAmount, string GiroNumber, DateTime? GiroDueDate, IFormFile GiroPhotoAttachment)
        {
            PaymentModel payment = null;
            if (GiroAmount > 0 && !string.IsNullOrEmpty(GiroNumber) && GiroDueDate != null)
            {
                string giroPhotoUrl = "";
                string giroblobName = Guid.NewGuid().ToString();

                if (GiroPhotoAttachment == null)
                {
                    throw new Exception("Giro photo is required.");
                }

                string attachmentContentType = GiroPhotoAttachment.ContentType.ToLower();
                if (attachmentContentType != "image/png" && attachmentContentType != "image/jpg" && attachmentContentType != "image/jpeg")
                {
                    throw new Exception("Cannot accept giro attachment file type.");
                }

                if (GiroPhotoAttachment.Length > 10000000)
                {
                    throw new Exception("Giro photo file exceeds the maximum limit.");
                }

                try
                {
                    giroPhotoUrl = await azureStorageService.UploadAsync(giroblobName, GiroPhotoAttachment.OpenReadStream(), GiroPhotoAttachment.ContentType);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                payment = new PaymentModel();
                payment.PaymentId = Guid.NewGuid().ToString();
                payment.PaymentChannel = PaymentChannel.Giro;
                payment.GiroPhoto = giroPhotoUrl;
                payment.BlobName = giroblobName;
                
            }
            else if (GiroAmount > 0 && string.IsNullOrEmpty(GiroNumber))
            {
                throw new Exception("Giro number is required.");
            }
            else if (GiroAmount > 0 && !string.IsNullOrEmpty(GiroNumber) && GiroDueDate == null)
            {
                throw new Exception("Giro due date is required.");
            }

            return payment;
        }

        [HttpPost]
        [Route("payment")]
        public async Task<IActionResult> Post([FromHeader(Name = "X-Aru-Token")] string authKey, [FromForm] PaymentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                json = new Trisatech.AspNet.Common.Models.JsonEntity()
                {
                    Error = true,
                    Data = null,
                    Alerts = new Trisatech.AspNet.Common.Models.JsonAlert { Code = (int)HttpStatusCode.BadRequest, Message = "Bad request" }
                };

                return Ok(json);
            }

            try
            {
                SetUserAuth();

                if (UserAuth == null)
                    return Ok(UnAuthorizeResponse.UnauthorizeResponse());

                var assignment = assignmentService.Detail(UserAuth.UserId, model.AssignmentId);
                if(assignment == null)
                {
                    json = new Trisatech.AspNet.Common.Models.JsonEntity()
                    {
                        Error = true,
                        Data = null,
                        Alerts = new Trisatech.AspNet.Common.Models.JsonAlert { Code = (int)HttpStatusCode.NotFound, Message = "Not found" }
                    };

                    return Ok(json);
                }

                if(assignment.AssignmentType == AssignmentType.CICO)
                {
                    json = new Trisatech.AspNet.Common.Models.JsonEntity()
                    {
                        Error = true,
                        Data = null,
                        Alerts = new Trisatech.AspNet.Common.Models.JsonAlert { Code = (int)HttpStatusCode.NotAcceptable, Message = "Tidak dapat memproses kunjungan, karena ini tugas dari sales." }
                    };

                    return Ok(json);
                }

                bool otpIsValid = false;
                List<PaymentModel> payments = new List<PaymentModel>();

                if (!string.IsNullOrEmpty(model.Otp))
                {
                    otpIsValid = authenticationService.CheckOtp(model.Otp, model.AssignmentId);
                    if (!otpIsValid)
                    {
                        json = new Trisatech.AspNet.Common.Models.JsonEntity()
                        {
                            Error = false,
                            Data = false,
                            Alerts = new Trisatech.AspNet.Common.Models.JsonAlert { Code = (int)HttpStatusCode.NotAcceptable, Message = "Otp has been expired." }
                        };

                        return Ok(json);
                    }
                }
                
                var listInvoice = invoiceService.GetInvoices(model.AssignmentId);

                if(listInvoice == null || listInvoice.Count == 0)
                {
                    json = new Trisatech.AspNet.Common.Models.JsonEntity()
                    {
                        Error = false,
                        Data = false,
                        Alerts = new Trisatech.AspNet.Common.Models.JsonAlert { Code = (int)HttpStatusCode.BadRequest, Message = "Invoice & assignment not found" }
                    };

                    return Ok(json);
                }

                #region Giro Attachment
                
                #endregion

                #region assign paymentChannel
                /*
                PaymentChannel paymentChannel = PaymentChannel.Cash;
                if(model.CashAmount > 0)
                {
                    //cash
                    paymentChannel = PaymentChannel.Cash;

                    if (model.TransferAmount > 0 && !string.IsNullOrEmpty(model.TransferBank))
                    {
                        //cash&transfer
                        paymentChannel = PaymentChannel.CashAndTransfer;
                        if (model.GiroAmount > 0 && !string.IsNullOrEmpty(model.GiroNumber))
                        {
                            //cash&transfer&giro
                            paymentChannel = PaymentChannel.CashAndTransferAndGiro;
                        }
                    }
                    else if (model.GiroAmount > 0 && !string.IsNullOrEmpty(model.GiroNumber))
                    {
                        //&giro
                        paymentChannel = PaymentChannel.CashAndGiro;
                    }
                }
                else if(model.TransferAmount > 0 && !string.IsNullOrEmpty(model.TransferBank))
                {
                    //transfer
                    paymentChannel = PaymentChannel.Transfer;
                    if (model.GiroAmount > 0 && !string.IsNullOrEmpty(model.GiroNumber))
                    {
                        //cash&transfer&giro
                        paymentChannel = PaymentChannel.TransferAndGiro;
                    }
                }
                else if(model.GiroAmount > 0 && !string.IsNullOrEmpty(model.GiroNumber))
                {
                    paymentChannel = PaymentChannel.Giro;
                    //giro only
                }
                */
                #endregion
                
                //write to db as payment
                model.PaymentId = Guid.NewGuid().ToString();
                PaymentModel paymentModel = new PaymentModel();
                CopyProperty.CopyPropertiesTo(model, paymentModel);
                
                //payment calcualtion
                paymentModel.PaymentAmount = paymentModel.CashAmount + paymentModel.GiroAmount + paymentModel.TransferAmount;
                paymentModel.InvoiceAmount = listInvoice.Where(x => x.Type == 1).Select(x => x.Amount).Sum();
                paymentModel.PaymentDebt = paymentModel.InvoiceAmount - paymentModel.PaymentAmount;
                paymentModel.InvoiceCode = string.Join(",", listInvoice.Select(x => x.InvoiceCode).ToArray());

                #region GIRO
                //Giro 1
                try
                {
                    //payments.Add(await CreateGiroPaymentModel(model.GiroAmount, model.GiroNumber, model.GiroDueDate, model.GiroPhotoAttachment));
                    //payments.Add(await CreateGiroPaymentModel(model.GiroAmount1, model.GiroNumber1, model.GiroDueDate1, model.GiroPhotoAttachment1));
                    //payments.Add(await CreateGiroPaymentModel(model.GiroAmount2, model.GiroNumber2, model.GiroDueDate2, model.GiroPhotoAttachment2));
                    //payments.Add(await CreateGiroPaymentModel(model.GiroAmount3, model.GiroNumber3, model.GiroDueDate3, model.GiroPhotoAttachment3));
                    //payments.Add(await CreateGiroPaymentModel(model.GiroAmount4, model.GiroNumber4, model.GiroDueDate4, model.GiroPhotoAttachment4));

                    var giroPayment = await CreateGiroPaymentModel(model.GiroAmount, model.GiroNumber, model.GiroDueDate, model.GiroPhotoAttachment);
                    if (giroPayment != null)
                    {
                        giroPayment.AssignmentId = model.AssignmentId;
                        giroPayment.PaymentChannel = PaymentChannel.Giro;
                        giroPayment.InvoiceAmount = paymentModel.InvoiceAmount;
                        giroPayment.PaymentAmount = paymentModel.PaymentAmount;
                        giroPayment.PaymentDebt = paymentModel.PaymentDebt;
                        giroPayment.InvoiceCode = paymentModel.InvoiceCode;
                        giroPayment.GiroAmount = model.GiroAmount;
                        giroPayment.GiroNumber = model.GiroNumber;
                        giroPayment.GiroDueDate = (model.GiroDueDate != null ? model.GiroDueDate.Value.ToUniversalTime() : model.GiroDueDate); ;

                        payments.Add(giroPayment);
                    }
                }
                catch (Exception ex)
                {
                    json = new Trisatech.AspNet.Common.Models.JsonEntity()
                    {
                        Error = true,
                        Data = null,
                        Alerts = new Trisatech.AspNet.Common.Models.JsonAlert { Code = (int)HttpStatusCode.BadRequest, Message = ex.Message }
                    };

                    return Ok(json);
                }

                //Giro 2
                try
                {
                    var giroPayment = await CreateGiroPaymentModel(model.GiroAmount1, model.GiroNumber1, model.GiroDueDate1, model.GiroPhotoAttachment1);

                    if (giroPayment != null)
                    {
                        giroPayment.AssignmentId = model.AssignmentId;
                        giroPayment.PaymentChannel = PaymentChannel.Giro;
                        giroPayment.InvoiceAmount = paymentModel.InvoiceAmount;
                        giroPayment.PaymentAmount = paymentModel.PaymentAmount;
                        giroPayment.PaymentDebt = paymentModel.PaymentDebt;
                        giroPayment.InvoiceCode = paymentModel.InvoiceCode;
                        giroPayment.GiroAmount = (decimal)model.GiroAmount1;
                        giroPayment.GiroNumber = model.GiroNumber1;
                        giroPayment.GiroDueDate = (model.GiroDueDate1 != null ? model.GiroDueDate1.Value.ToUniversalTime() : model.GiroDueDate1); ;

                        payments.Add(giroPayment);
                    }
                }
                catch (Exception ex)
                {
                    json = new Trisatech.AspNet.Common.Models.JsonEntity()
                    {
                        Error = true,
                        Data = null,
                        Alerts = new Trisatech.AspNet.Common.Models.JsonAlert { Code = (int)HttpStatusCode.BadRequest, Message = ex.Message }
                    };

                    return Ok(json);
                }

                //Giro 3
                try
                {
                    var giroPayment = await CreateGiroPaymentModel(model.GiroAmount2, model.GiroNumber2, model.GiroDueDate2, model.GiroPhotoAttachment2);
                    if (giroPayment != null)
                    {
                        giroPayment.AssignmentId = model.AssignmentId;
                        giroPayment.PaymentChannel = PaymentChannel.Giro;
                        giroPayment.InvoiceAmount = paymentModel.InvoiceAmount;
                        giroPayment.PaymentAmount = paymentModel.PaymentAmount;
                        giroPayment.PaymentDebt = paymentModel.PaymentDebt;
                        giroPayment.InvoiceCode = paymentModel.InvoiceCode;
                        giroPayment.GiroAmount = (decimal)model.GiroAmount2;
                        giroPayment.GiroNumber = model.GiroNumber2;
                        giroPayment.GiroDueDate = (model.GiroDueDate2 != null ? model.GiroDueDate2.Value.ToUniversalTime() : model.GiroDueDate2);

                        payments.Add(giroPayment);
                    }
                }
                catch (Exception ex)
                {
                    json = new Trisatech.AspNet.Common.Models.JsonEntity()
                    {
                        Error = true,
                        Data = null,
                        Alerts = new Trisatech.AspNet.Common.Models.JsonAlert { Code = (int)HttpStatusCode.BadRequest, Message = ex.Message }
                    };

                    return Ok(json);
                }

                //Giro 4
                try
                {
                    var giroPayment = await CreateGiroPaymentModel(model.GiroAmount3, model.GiroNumber3, model.GiroDueDate3, model.GiroPhotoAttachment3);
                    if (giroPayment != null)
                    {
                        giroPayment.AssignmentId = model.AssignmentId;
                        giroPayment.PaymentChannel = PaymentChannel.Giro;
                        giroPayment.InvoiceAmount = paymentModel.InvoiceAmount;
                        giroPayment.PaymentAmount = paymentModel.PaymentAmount;
                        giroPayment.PaymentDebt = paymentModel.PaymentDebt;
                        giroPayment.InvoiceCode = paymentModel.InvoiceCode;
                        giroPayment.GiroAmount = (decimal)model.GiroAmount3;
                        giroPayment.GiroNumber = model.GiroNumber3;
                        giroPayment.GiroDueDate = (model.GiroDueDate3 != null ? model.GiroDueDate3.Value.ToUniversalTime():model.GiroDueDate3);

                        payments.Add(giroPayment);
                    }
                }
                catch (Exception ex)
                {
                    json = new Trisatech.AspNet.Common.Models.JsonEntity()
                    {
                        Error = true,
                        Data = null,
                        Alerts = new Trisatech.AspNet.Common.Models.JsonAlert { Code = (int)HttpStatusCode.BadRequest, Message = ex.Message }
                    };

                    return Ok(json);
                }

                //Giro 5
                try
                {
                    var giroPayment = await CreateGiroPaymentModel(model.GiroAmount4, model.GiroNumber4, model.GiroDueDate4, model.GiroPhotoAttachment4);
                    if (giroPayment != null)
                    {
                        giroPayment.AssignmentId = model.AssignmentId;
                        giroPayment.PaymentChannel = PaymentChannel.Giro;
                        giroPayment.InvoiceAmount = paymentModel.InvoiceAmount;
                        giroPayment.PaymentAmount = paymentModel.PaymentAmount;
                        giroPayment.PaymentDebt = paymentModel.PaymentDebt;
                        giroPayment.InvoiceCode = paymentModel.InvoiceCode;
                        giroPayment.GiroAmount = (decimal)model.GiroAmount4;
                        giroPayment.GiroNumber = model.GiroNumber4;
                        giroPayment.GiroDueDate = (model.GiroDueDate4 != null ? model.GiroDueDate4.Value.ToUniversalTime() : model.GiroDueDate4); ;

                        payments.Add(giroPayment);
                    }
                }
                catch (Exception ex)
                {
                    json = new Trisatech.AspNet.Common.Models.JsonEntity()
                    {
                        Error = true,
                        Data = null,
                        Alerts = new Trisatech.AspNet.Common.Models.JsonAlert { Code = (int)HttpStatusCode.BadRequest, Message = ex.Message }
                    };

                    return Ok(json);
                }
                #endregion

                //Cash
                if (model.CashAmount > 0)
                {
                    PaymentModel cashPayment = new PaymentModel();
                    cashPayment.PaymentId = Guid.NewGuid().ToString();
                    cashPayment.AssignmentId = model.AssignmentId;
                    cashPayment.PaymentChannel = PaymentChannel.Cash;
                    cashPayment.InvoiceAmount = paymentModel.InvoiceAmount;
                    cashPayment.PaymentAmount = paymentModel.PaymentAmount;
                    cashPayment.PaymentDebt = paymentModel.PaymentDebt;
                    cashPayment.InvoiceCode = paymentModel.InvoiceCode;
                    cashPayment.CashAmount = model.CashAmount;

                    payments.Add(cashPayment);
                }

                #region Bank transfer
                if (model.TransferAmount > 0 && !string.IsNullOrEmpty(model.TransferBank) && model.TransferDate != null)
                {
                    PaymentModel transferPayment = new PaymentModel();
                    transferPayment.PaymentId = Guid.NewGuid().ToString();
                    transferPayment.AssignmentId = model.AssignmentId;
                    transferPayment.PaymentChannel = PaymentChannel.Transfer;
                    transferPayment.InvoiceAmount = paymentModel.InvoiceAmount;
                    transferPayment.PaymentAmount = paymentModel.PaymentAmount;
                    transferPayment.PaymentDebt = paymentModel.PaymentDebt;
                    transferPayment.InvoiceCode = paymentModel.InvoiceCode;
                    transferPayment.TransferAmount = model.TransferAmount;
                    transferPayment.TransferBank = model.TransferBank;
                    transferPayment.TransferDate = model.TransferDate.Value.ToUniversalTime();

                    payments.Add(transferPayment);
                }

                if (model.TransferAmount1 > 0 && !string.IsNullOrEmpty(model.TransferBank1) && model.TransferDate1 != null)
                {
                    PaymentModel transferPayment = new PaymentModel();
                    transferPayment.PaymentId = Guid.NewGuid().ToString();
                    transferPayment.AssignmentId = model.AssignmentId;
                    transferPayment.PaymentChannel = PaymentChannel.Transfer;
                    transferPayment.InvoiceAmount = paymentModel.InvoiceAmount;
                    transferPayment.PaymentAmount = paymentModel.PaymentAmount;
                    transferPayment.PaymentDebt = paymentModel.PaymentDebt;
                    transferPayment.InvoiceCode = paymentModel.InvoiceCode;
                    transferPayment.TransferAmount = (decimal)model.TransferAmount1;
                    transferPayment.TransferBank = model.TransferBank1;
                    transferPayment.TransferDate = model.TransferDate1.Value.ToUniversalTime();

                    payments.Add(transferPayment);
                }

                if (model.TransferAmount2 > 0 && !string.IsNullOrEmpty(model.TransferBank2) && model.TransferDate2 != null)
                {
                    PaymentModel transferPayment = new PaymentModel();
                    transferPayment.PaymentId = Guid.NewGuid().ToString();
                    transferPayment.AssignmentId = model.AssignmentId;
                    transferPayment.PaymentChannel = PaymentChannel.Transfer;
                    transferPayment.InvoiceAmount = paymentModel.InvoiceAmount;
                    transferPayment.PaymentAmount = paymentModel.PaymentAmount;
                    transferPayment.PaymentDebt = paymentModel.PaymentDebt;
                    transferPayment.InvoiceCode = paymentModel.InvoiceCode;
                    transferPayment.TransferAmount = (decimal)model.TransferAmount2;
                    transferPayment.TransferBank = model.TransferBank2;
                    transferPayment.TransferDate = model.TransferDate2.Value.ToUniversalTime();

                    payments.Add(transferPayment);
                }

                if (model.TransferAmount3 > 0 && !string.IsNullOrEmpty(model.TransferBank3) && model.TransferDate3 != null)
                {
                    PaymentModel transferPayment = new PaymentModel();
                    transferPayment.PaymentId = Guid.NewGuid().ToString();
                    transferPayment.AssignmentId = model.AssignmentId;
                    transferPayment.PaymentChannel = PaymentChannel.Transfer;
                    transferPayment.InvoiceAmount = paymentModel.InvoiceAmount;
                    transferPayment.PaymentAmount = paymentModel.PaymentAmount;
                    transferPayment.PaymentDebt = paymentModel.PaymentDebt;
                    transferPayment.InvoiceCode = paymentModel.InvoiceCode;
                    transferPayment.TransferAmount = (decimal)model.TransferAmount3;
                    transferPayment.TransferBank = model.TransferBank3;
                    transferPayment.TransferDate = model.TransferDate3.Value.ToUniversalTime();

                    payments.Add(transferPayment);
                }
                #endregion

                invoiceService.SubmitPayment(payments, UserAuth.UserId, model.AssignmentId, otpIsValid);

                json = new Trisatech.AspNet.Common.Models.JsonEntity()
                {
                    Error = false,
                    Data = true,
                    Alerts = new Trisatech.AspNet.Common.Models.JsonAlert { Code = (int)HttpStatusCode.OK, Message = "Success" }
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
    }
}