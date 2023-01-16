using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Trisatech.MWorkforce.Business.Entities;
using Trisatech.MWorkforce.Business.Interfaces;
using Trisatech.MWorkforce.Infrastructure.Interface;
using Trisatech.MWorkforce.Cms.Models;
using Trisatech.MWorkforce.Cms.Services;
using Trisatech.MWorkforce.Cms.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Trisatech.AspNet.Common.Extensions;
using Trisatech.AspNet.Common.Helpers;

namespace Trisatech.MWorkforce.Cms.Controllers
{
    [Authorize(Roles = "SA,OPR,SALES")]
    public class TaskManagementController : Controller
    {
        private const string PATH_DELIMETER = @"\";
        private readonly ITextFileReader textFileReader;
        private readonly IAssignmentService assignmentService;
        private readonly IUserManagementService userManagementService;
        private readonly ICustomerService customerService;
        private readonly ISequencerNumberService sequencerNumber;
        private readonly ApplicationSetting applicationSetting;
        private readonly ILogger logger;

        public TaskManagementController(ILoggerFactory logger,
            ITextFileReader textFileReader,
            IUserManagementService userManagementService,
            IAssignmentService assignmentService, 
            ICustomerService customerService,
            ISequencerNumberService sequencerNumber,
            IOptions<ApplicationSetting> options)
        {
            this.customerService = customerService;
            this.textFileReader = textFileReader;
            this.assignmentService = assignmentService;
            this.sequencerNumber = sequencerNumber;
            this.userManagementService = userManagementService;
            this.applicationSetting = options.Value;
            this.logger = logger.CreateLogger("TaskManagement.Controller");
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if(context.RouteData != null && context.RouteData.Values != null)
            {
                if(context.RouteData.Values["action"].ToString().ToUpper() == "UPLOAD")
                {
                    ViewBag.SelectedMenu = "upload";
                }else if (context.RouteData.Values["action"].ToString().ToLower() == "DRIVER")
                {
                    ViewBag.SelectedMenu = "driver";
                }
                else
                {
                    ViewBag.SelectedMenu = "task";
                }
            }
            base.OnActionExecuted(context);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Driver()
        {
            return View();
        }

        #region Add task from file
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var json = new JsonResponseViewModel();
            try
            {
                if (file == null)
                {
                    json.success = false;
                    json.message = "No file selected.";
                    return Json(json);
                }

                if (Path.GetExtension(file.FileName) != ".csv")
                {
                    json.success = false;
                    json.message = "File type must be .csv";
                    return Json(json);
                }

                if (file.Length > 10000000)
                {
                    json.success = false;
                    json.message = "file too large";

                    return Json(json);
                }

                string filePath = Path.GetTempPath() + PATH_DELIMETER + file.FileName;
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);

                    stream.Close();
                }

                var result = textFileReader.Read(filePath, ",", 12);
                List<TaskUploadViewModel> listTask;

                try
                {
                    listTask = new ConvertTaskViewModel(result).TaskUploadViewModel;
                }catch(Exception ex)
                {
                    json.success = false;
                    json.message = "Ada kesalahan format data, silahkan review dan perbaiki. Kemudian upload kembali.";
                    return Json(json);
                }

                json.success = true;
                json.message = "Success";
                json.data = listTask;

                return Json(json);
            }
            catch(Exception ex)
            {
                json.message = ex.Message;
                json.success = false;
                return Json(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Process([FromBody] TaskDataUploadViewModel data)
        {
            if(data == null || data.data == null)
            {
                return Json(new JsonResponseViewModel { success = false, message = "Task can't be empty" });
            }

            try
            {
                var userAuth = Trisatech.MWorkforce.Cms.Helpers.AppCookieHelper.Get<UserAuthenticated>(this.HttpContext);

                var taskGroupByInvoice = data.data.GroupBy(x => x.CustomerCode).ToList();
                if(taskGroupByInvoice != null)
                {
                    List<AssignmentModel> assignments = new List<AssignmentModel>();

                    foreach(var item in taskGroupByInvoice)
                    {
                        if(item != null && item.Any())
                        {
                            var task = item.FirstOrDefault();
                            string assignmentCode = DateTime.Now.ToString("yyyyMMdd") + sequencerNumber.SequenceNumber;
                            string assignmentId = Guid.NewGuid().ToString();

                            AssignmentModel newAssignment = new AssignmentModel()
                            {
                                AgentCode = task.Sales,
                                AssignmentName = task.CustomerName,
                                AssignmentAddress = task.Address,
                                AssignmentCode = assignmentCode,
                                AssignmentType = Domain.Entities.AssignmentType.VISIT,
                                AssignmentId = assignmentId,
                                AssignmentStatusCode = Domain.AppConstant.AssignmentStatus.TASK_RECEIVED,
                                AssignmentDate = task.AssignmentDate.Date.AddHours(-7),
                                Latitude = task.Latitude,
                                Longitude = task.Longitude,
                                Contact = new ContactModel
                                {
                                    ContactName = task.CustomerName,
                                    ContactNumber = task.Phone.MobilePhoneFormat(),
                                    CustomerCode = task.CustomerCode
                                }
                            };
                            
                            List<InvoiceModel> invoices = new List<InvoiceModel>();
                            foreach(var invoice in item.ToList())
                            {
                                string invoiceId = Guid.NewGuid().ToString();
                                invoices.Add(new InvoiceModel
                                {
                                    AssignmentCode = assignmentCode,
                                    InvoiceCode = invoice.NoFaktur,
                                    InvoiceId = invoiceId,
                                    DueDate = (invoice.AssignmentDueDate != null ? invoice.AssignmentDueDate.Value.Date.AddHours(-7) : DateTime.UtcNow.Date.AddHours(-7)),
                                    Amount = invoice.Amount,
                                    Status = (invoice.Amount > 0 ? "Tagihan":"Cash back")
                                });
                            }

                            newAssignment.Invoices = invoices;
                            assignments.Add(newAssignment);
                        }
                    }
                    
                    assignmentService.Add(assignments, userAuth.UserId, true);
                }
                
                return Json(new JsonResponseViewModel { success = true, message = "success" });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseViewModel { success = false, message = ex.Message });
            }
        }
        #endregion

        public IActionResult Create()
        {
            CreateAssignmentViewModel model = new CreateAssignmentViewModel();

            long totalSales = userManagementService.TotalUsers();
            var sales = userManagementService.Users("", (int)totalSales, 0).Where(x=>x.RoleCode == "SALES").ToList();
            var contact = customerService.GetAllContact();

            ViewBag.SalesList = sales;
            ViewBag.ContactList = contact;
            ViewBag.GoogleMapsKey = applicationSetting.GoogleMapsKey;

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateAssignmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userAuth = Trisatech.MWorkforce.Cms.Helpers.AppCookieHelper.Get<UserAuthenticated>(this.HttpContext);
                if(userAuth != null)
                {
                    try
                    {
                        AssignmentModel newItem = new AssignmentModel();

                        var agent = userManagementService.DetailUser(model.SalesId);
                        if (agent != null)
                        {
                            newItem.AssignmentId = Guid.NewGuid().ToString();
                            newItem.AssignmentName = model.AssignmentName;
                            newItem.AssignmentStatusCode = Domain.AppConstant.AssignmentStatus.TASK_RECEIVED;
                            newItem.AssignmentDate = model.AssignmentDate.ToUtc();
                            newItem.AssignmentCode = DateTime.Now.Date.ToString("yyyyMMdd") + sequencerNumber.SequenceNumber;
                            newItem.AgentId = model.SalesId;
                            newItem.AssignmentAddress = model.Address;
                            newItem.Latitude = model.Latitude;
                            newItem.AssignmentType = Domain.Entities.AssignmentType.VISIT;
                            newItem.Longitude = model.Longitude;
                            newItem.Remarks = model.Remarks;
                            newItem.Contact = new ContactModel
                            {
                                ContactId = model.ContactId
                            };

                            assignmentService.Add(newItem, userAuth.UserId, false);

                            return RedirectToAction(nameof(Index));
                        }

                        ModelState.AddModelError("SalesId", "Sales not found in system, please select sales again.");
                    }
                    catch(Exception ex)
                    {
                        ViewBag.ErrorMessage = ex.Message;
                    }
                }
            }

            #region References
            long totalSales = userManagementService.TotalUsers();
            var sales = userManagementService.Users("", (int)totalSales, 0).Where(x => x.RoleCode == "SALES").ToList();
            var contact = customerService.GetAllContact();

            ViewBag.SalesList = sales;
            ViewBag.ContactList = contact;
            ViewBag.GoogleMapsKey = applicationSetting.GoogleMapsKey;
            #endregion

            return View(model);
        }

        public IActionResult Details(string id)
        {
            AssignmentModel assignmentModel;

            try
            {
                assignmentModel = assignmentService.Detail("", id);
            }
            catch
            {
                return NotFound();
            }

            ViewBag.GoogleMapsKey = applicationSetting.GoogleMapsKey;
            return View(assignmentModel);
        }

        public IActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction(nameof(Details), new { id = id });
            }

            CreateAssignmentViewModel model = new CreateAssignmentViewModel();
            
            try
            {
                var result = assignmentService.Detail("", id);
                if(result != null)
                {
                    model.AssignmentDate = result.AssignmentDate;
                    model.AssignmentName = result.AssignmentName;
                    model.Address = result.AssignmentAddress;
                    model.ContactId = result.Contact.ContactId;
                    model.Latitude = result.Latitude;
                    model.Longitude = result.Longitude;
                    model.Remarks = result.Remarks;
                    model.SalesId = result.AgentId;

                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }

            }catch(Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }

            #region References
            long totalSales = userManagementService.TotalUsers();
            var sales = userManagementService.Users("", (int)totalSales, 0).Where(x => x.RoleCode == "SALES").ToList();
            var contact = customerService.GetAllContact();

            ViewBag.SalesList = sales;
            ViewBag.ContactList = contact;
            ViewBag.GoogleMapsKey = applicationSetting.GoogleMapsKey;
            #endregion
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, CreateAssignmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return RedirectToAction(nameof(Details), new { id = id });
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = ex.Message;
                }
            }

            #region References
            long totalSales = userManagementService.TotalUsers();
            var sales = userManagementService.Users("", (int)totalSales, 0).Where(x => x.RoleCode == "SALES").ToList();
            var contact = customerService.GetAllContact();

            ViewBag.SalesList = sales;
            ViewBag.ContactList = contact;
            ViewBag.GoogleMapsKey = applicationSetting.GoogleMapsKey;
            #endregion
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string id)
        {
            try
            {
                var userAuth = Trisatech.MWorkforce.Cms.Helpers.AppCookieHelper.Get<UserAuthenticated>(this.HttpContext);

                if (string.IsNullOrEmpty(id))
                    return NotFound();

                assignmentService.Delete(id, userAuth.UserId);

            }catch(Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Details), new { id = id });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}