using Trisatech.MWorkforce.Business.Entities;
using Trisatech.MWorkforce.Business.Interfaces;
using Trisatech.MWorkforce.Domain;
using Trisatech.MWorkforce.Domain.Entities;
using Trisatech.AspNet.Common.Interfaces;
using Trisatech.AspNet.Common.Repository;
using Microsoft.EntityFrameworkCore;
using Trisatech.AspNet.Common.Extensions;

namespace Trisatech.MWorkforce.Business.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepository<Assignment> repoAssignment;
		private readonly IRepository<Customer> repoCustomer;
		private readonly MobileForceContext dbContext;

        public AssignmentService(MobileForceContext context)
        {
            dbContext = context;
            unitOfWork = new UnitOfWork<MobileForceContext>(context);
            repoAssignment = this.unitOfWork.GetRepository<Assignment>();
			repoCustomer = unitOfWork.GetRepository<Customer>();
		}

        #region Web MVC
        public List<AssignmentModel> GetListAssignment(string keyword, int limit = 10, int offset = 0, string orderColumn = "AssignmentCode", string orderType = "desc", DateTime? startDate = null, DateTime? endDate = null,string status = null, UserAuthenticated user = null, string role = "")
        {
            try
            {
                if(startDate == null)
                {
                    startDate = DateTime.UtcNow.Date;
                }else{
                    startDate = startDate.Value.Date.ToUniversalTime();
                }

                if (endDate == null)
                    endDate = DateTime.UtcNow.Date.AddDays(1);
                else
                    endDate = endDate.Value.Date.ToUniversalTime().AddDays(1);
                
                repoAssignment.Limit = limit;
                repoAssignment.Offset = offset;
                repoAssignment.Includes = new string[] { "AssignmentDetail", "AssignmentStatus", "AssignmentDetail.User", "AssignmentDetail.User.Account", "AssignmentDetail.Contact" };
                repoAssignment.Condition = PredicateBuilder.True<Assignment>().And(x => x.IsDeleted == 0 && x.IsActive == 1);

                if (!string.IsNullOrEmpty(keyword))
                {
                    repoAssignment.Condition = repoAssignment.Condition.And(x => x.AssignmentName.Contains(keyword) ||
                    x.AssignmentCode.Contains(keyword) ||
                    x.Address.Contains(keyword) ||
                    (x.AssignmentDetail.User != null && x.AssignmentDetail.User.UserName.Contains(keyword)) ||
                    (x.AssignmentDetail.User != null && x.AssignmentDetail.User.UserCode.Contains(keyword)));
                }

                if (startDate != null && endDate != null)
                {
                    repoAssignment.Condition = repoAssignment.Condition.And(x => 
                    (x.AssignmentDate >= startDate.Value && x.AssignmentDate < endDate.Value));
                }

                if (status != null)
                {
                    repoAssignment.Condition = repoAssignment.Condition.And(x => (x.AssignmentStatusCode == status));
                }

                if (!string.IsNullOrEmpty(role))
                {
                    repoAssignment.Condition = repoAssignment.Condition.And(x => x.AssignmentDetail.User.Account.RoleCode == role);
                }

                repoAssignment.OrderBy = new Trisatech.AspNet.Common.Models.SqlOrderBy
                {
                    Column = orderColumn,
                    Type = orderType
                };
                
                #region Find based on custom model
				if (user == null)
				{
					var dbResult = repoAssignment.Find<AssignmentModel>(x => new AssignmentModel
					{
						AssignmentId = x.AssignmentId,
						AssignmentCode = x.AssignmentCode,
						AssignmentStatusCode = x.AssignmentStatusCode,
						AssignmentName = x.AssignmentName,
						AssignmentAddress = x.Address,
						AssignmentDate = x.AssignmentDate,
						StartTime = x.AssignmentDetail.StartTime,
						EndTime = x.AssignmentDetail.EndTime,
						Latitude = x.Latitude,
						Longitude = x.Longitude,
						Remarks = x.Remarks,
						//AssignmentType = x.AssignmentType,
						Status = x.AssignmentStatus.AssignmentStatusName,
						AgentCode = x.AssignmentDetail.User?.UserCode,
						AgentName = x.AssignmentDetail.User?.UserName,
						ContactName = x.AssignmentDetail.Contact?.ContactName,
						ContactNumber = x.AssignmentDetail.Contact?.ContactNumber,
					}).ToList();

					return dbResult;

					
				}else //jika menggunakan role OPR dan SALES
				{
                    var condition = PredicateBuilder.True<Assignment>().And(x => x.IsDeleted == 0 && x.IsActive == 1);

                    if (!string.IsNullOrEmpty(keyword))
                    {
                        condition = condition.And(x => x.AssignmentName.Contains(keyword) ||
                    x.AssignmentCode.Contains(keyword) ||
                    x.Address.Contains(keyword) ||
                    (x.AssignmentDetail.User != null && x.AssignmentDetail.User.UserName.Contains(keyword)) ||
                    (x.AssignmentDetail.User != null && x.AssignmentDetail.User.UserCode.Contains(keyword)));
                    }

                    if (startDate != null && endDate != null)
                    {
                        condition = condition.And(x => (x.AssignmentDate >= startDate.Value && x.AssignmentDate <= endDate.Value));
                    }

                    if (status != null)
                    {
                        condition = condition.And(x => (x.AssignmentStatusCode == status));
                    }

                    IQueryable<Assignment> query = (from a in dbContext.Accounts
                                                  join b in dbContext.Users on a.AccountId equals b.AccountId
                                                  join c in dbContext.UserTerritories on b.UserId equals c.UserId
                                                  join d in dbContext.UserTerritories on c.TerritoryId equals d.TerritoryId
                                                  join f in dbContext.AssignmentDetails on d.UserId equals f.UserId
                                                  join e in dbContext.Assignments on f.AssignmentId equals e.AssignmentId
                                                  where a.AccountId == user.AccountId
                                                  select e);

                    var result = query.Where(condition).Select(e => new AssignmentModel
                    {
                        AssignmentId = e.AssignmentId,
                        AssignmentCode = e.AssignmentCode,
                        AssignmentStatusCode = e.AssignmentStatusCode,
                        AssignmentName = e.AssignmentName,
                        AssignmentAddress = e.Address,
                        AssignmentDate = e.AssignmentDate,
                        StartTime = e.AssignmentDetail.StartTime,
                        EndTime = e.AssignmentDetail.EndTime,
                        Latitude = e.Latitude,
                        Longitude = e.Longitude,
                        Remarks = e.Remarks,
                        //AssignmentType = x.AssignmentType,
                        Status = e.AssignmentStatus.AssignmentStatusName,
                        AgentCode = e.AssignmentDetail.User.UserCode,
                        AgentName = e.AssignmentDetail.User.UserName,
                        ContactName = e.AssignmentDetail.Contact.ContactName,
                        ContactNumber = e.AssignmentDetail.Contact.ContactNumber
                    }).Skip(offset).Take(limit).ToList();
                    
					return result;
				}
				#endregion

			}
            catch { throw; }
        }
        
        public long GetListAssignmentCount(string keyword, string orderColumn = "AssignmentCode", string orderType = "desc",string status = null, DateTime? startDate = null, DateTime? endDate = null, string role = "")
        {
            try
            {
                if (startDate == null)
                    startDate = DateTime.UtcNow.Date;
                else
                    startDate = startDate.Value.Date.ToUtc();
                
                if (endDate == null)
                    endDate = DateTime.UtcNow.Date.AddDays(1);
                else
                    endDate = endDate.Value.Date.ToUtc(24);

                repoAssignment.Includes = new string[] { "AssignmentDetail", "AssignmentStatus", "AssignmentDetail.User", "AssignmentDetail.User.Account", "AssignmentDetail.Contact" };
                repoAssignment.Condition = PredicateBuilder.True<Assignment>().And(x => x.IsDeleted == 0 && x.IsActive == 1);

                if (!string.IsNullOrEmpty(keyword))
                {
                    repoAssignment.Condition = repoAssignment.Condition.And(x => x.AssignmentName.Contains(keyword) ||
                    x.AssignmentCode.Contains(keyword) ||
                    x.Address.Contains(keyword) ||
                    (x.AssignmentDetail.User != null && x.AssignmentDetail.User.UserName.Contains(keyword)) ||
                    (x.AssignmentDetail.User != null && x.AssignmentDetail.User.UserCode.Contains(keyword)));
                }

                if (startDate != null && endDate != null)
                {
                    repoAssignment.Condition = repoAssignment.Condition.And(x => 
                    (x.AssignmentDate >= startDate.Value
                    && x.AssignmentDate < endDate.Value));
                }

                if (!string.IsNullOrEmpty(status))
                {
                    repoAssignment.Condition = repoAssignment.Condition.And(x => (x.AssignmentStatusCode == status));
                }

                if (!string.IsNullOrEmpty(role))
                {
                    repoAssignment.Condition = repoAssignment.Condition.And(x => x.AssignmentDetail.User.Account.RoleCode == role);
                }

                repoAssignment.OrderBy = new Trisatech.AspNet.Common.Models.SqlOrderBy
                {
                    Column = orderColumn,
                    Type = orderType
                };

                long? count = repoAssignment.Count();
                if (count == null)
                    return 0;

                return (long)count;

            }
            catch { throw; }
        }

        public async Task<List<ReportAssignmentModel>> GetListAssignmentReport(string keyword, DateTime date, int limit = 10, int offset = 0, string orderColumn = "UserCode", string orderType = "asc")
        {
            try
            {
                if (string.IsNullOrEmpty(keyword))
                    keyword = "";

                var resultDb = (from a in dbContext.AssignmentGroups
                                join b in dbContext.Users on a.CreatedBy equals b.UserId
                                where a.StartTime.AddHours(7).Date == date.Date && (b.UserCode.Contains(keyword) || b.UserName.Contains(keyword))
                                orderby orderColumn
                                select new ReportAssignmentModel
                                {
                                    UserCode = b.UserCode,
                                    UserId = b.UserId,
                                    Name = b.UserName,
                                    StartTime = a.StartTime,
                                    EndedTime = a.EndTime,
                                    UserName = b.UserName,
                                    TotalLostTime = a.TotalLostTime,
                                    TotalTask = (from c in dbContext.AssignmentDetails
                                                 where c.UserId == b.UserId && 
                                                 (c.Assignment.AssignmentStatusCode == AppConstant.AssignmentStatus.AGENT_STARTED || c.Assignment.AssignmentStatusCode == AppConstant.AssignmentStatus.TASK_COMPLETED) 
                                                 && (c.EndTime != null && c.EndTime.Value.Date == a.StartTime.Date)
                                                 select c.AssignmentDetailId).Count(),
                                    TotalTaskVerified = (from c in dbContext.AssignmentDetails
                                                         where c.UserId == b.UserId 
                                                         && c.IsVerified
                                                         && (c.EndTime != null && c.EndTime.Value.Date == a.StartTime.Date)
                                                         select c.AssignmentDetailId).Count()
                                }).Skip(offset).Take(limit);

                var query = resultDb.AsQueryable();

                return await query.ToListAsync();
            }
            catch { throw; }
        }

        public async Task<long> GetListAssignmentReportCount(string keyword, DateTime date, string orderColumn = "UserCode", string orderType = "asc")
        {
            try
            {
                if (string.IsNullOrEmpty(keyword))
                    keyword = "";

                var resultDb = await (from a in dbContext.AssignmentGroups
                                join b in dbContext.Users on a.CreatedBy equals b.UserId
                                where a.StartTime.AddHours(7).Date == date.Date && (b.UserCode.Contains(keyword) || b.UserName.Contains(keyword))
                                orderby orderColumn
                                select new ReportAssignmentModel
                                {
                                    UserCode = b.UserCode
                                }).LongCountAsync();

                return resultDb;
            }catch(DbUpdateException sqlEx)
            {
                throw new ApplicationException(sqlEx.Message);
            }
            catch { throw; }
        }

        public void Add(List<AssignmentModel> assignments, string createdBy, bool invoice = true)
        {
            try
            {
                var repoAssignmentDetail = unitOfWork.GetRepository<AssignmentDetail>();
                var repoContact = unitOfWork.GetRepository<Contact>();
                var repoInvoice = unitOfWork.GetRepository<Invoice>();
                var repoUser = unitOfWork.GetRepository<User>();
                var repoCustomerSales = unitOfWork.GetRepository<CustomerContactAgent>();

                DateTime utcNow = DateTime.UtcNow;

                unitOfWork.Strategy().Execute(() =>
                {
                    unitOfWork.BeginTransaction();

                    foreach (var item in assignments)
                    {
                        #region insert new customer

                        repoCustomer.Includes = new string[] { "Contacts" };
                        repoCustomer.Condition = PredicateBuilder.True<Customer>().And(x => x.CustomerCode == item.Contact.CustomerCode && x.IsDeleted == 0 && x.IsActive == 1);

                        string customerId = Guid.NewGuid().ToString();
                        string contactId = Guid.NewGuid().ToString();

                        var customerExist = repoCustomer.Find().FirstOrDefault();
                        if (customerExist == null)
                        {
                            Customer newCustomer = new Customer
                            {
                                CustomerId = customerId,
                                CustomerCode = item.Contact.CustomerCode,
                                CustomerName = item.Contact.ContactName,
                                CreatedBy = createdBy,
                                CreatedDt = utcNow,
                                IsActive = 1,
                                IsDeleted = 0,
                                CustomerAddress = item.AssignmentAddress
                            };
                            Contact contact = new Contact
                            {
                                ContactId = contactId,
                                ContactName = item.Contact.ContactName,
                                ContactNumber = item.Contact.ContactNumber,
                                CustomerId = newCustomer.CustomerId,
                                CreatedBy = createdBy,
                                CreatedDt = utcNow,
                                IsActive = 1,
                                IsDeleted = 0
                            };

                            repoCustomer.Insert(newCustomer);
                            repoContact.Insert(contact);
                        }
                        else
                        {
                            if (customerExist.Contacts != null && customerExist.Contacts.Count > 0)
                            {
                                var contactExist = customerExist.Contacts.Where(x => x.ContactNumber == item.Contact.ContactNumber).FirstOrDefault();
                                if (contactExist != null)
                                {
                                    contactId = contactExist.ContactId;
                                }
                                else
                                {
                                    //insert new contact
                                    Contact contact = new Contact
                                    {
                                        ContactId = contactId,
                                        ContactName = item.Contact.ContactName,
                                        ContactNumber = item.Contact.ContactNumber,
                                        CustomerId = customerExist.CustomerId,
                                        CreatedBy = createdBy,
                                        CreatedDt = utcNow,
                                        IsActive = 1,
                                        IsDeleted = 0
                                    };
                                    repoContact.Insert(contact);
                                }
                            }
                        }
                        #endregion

                        #region User/Agent/Sales
                        repoUser.Condition = PredicateBuilder.True<User>().And(x => x.UserCode == item.AgentCode && x.IsActive == 1 && x.IsDeleted == 0);
                        var user = repoUser.Find().FirstOrDefault();
                        #endregion

                        if (user == null)
                        {
                            throw new ApplicationException($"Agent ({item.AgentCode}) has not been registered.");
                        }

                        #region insert new Assignment
                        double? taskLat = (customerExist != null && customerExist.CustomerLatitude != null ? customerExist.CustomerLatitude : item.Latitude);
                        double? taskLng = (customerExist != null && customerExist.CustomerLongitude != null ? customerExist.CustomerLongitude : item.Longitude);

                        Assignment newAssignment = new Assignment()
                        {
                            CreatedBy = createdBy,
                            CreatedDt = utcNow,
                            IsActive = 1,
                            IsDeleted = 0,
                            AssignmentType = item.AssignmentType,
                            AssignmentName = item.AssignmentName,
                            AssignmentDate = item.AssignmentDate,
                            AssignmentCode = item.AssignmentCode,
                            Address = item.AssignmentAddress,
                            AssignmentId = item.AssignmentId,
                            AssignmentStatusCode = item.AssignmentStatusCode,
                            Latitude = taskLat,
                            Longitude = taskLng
                        };

                        AssignmentDetail assignmentDetail = new AssignmentDetail
                        {
                            CreatedBy = createdBy,
                            CreatedDt = utcNow,
                            IsActive = 1,
                            IsDeleted = 0,
                            UserCode = user.UserCode,
                            AssignmentId = newAssignment.AssignmentId,
                            AssignmentDetailId = Guid.NewGuid().ToString(),
                            ContactId = contactId,
                            UserId = user.UserId,
                            IsVerified = false
                        };

                        if (invoice)
                        {
                            foreach (var itemInvoice in item.Invoices)
                            {
                                repoInvoice.Insert(new Invoice
                                {
                                    CreatedBy = createdBy,
                                    CreatedDt = utcNow,
                                    IsActive = 1,
                                    IsDeleted = 0,
                                    DueDate = itemInvoice.DueDate,
                                    Amount = itemInvoice.Amount,
                                    AssignmentCode = newAssignment.AssignmentCode,
                                    AssignmentId = newAssignment.AssignmentId,
                                    InvoiceCode = itemInvoice.InvoiceCode,
                                    InvoiceId = Guid.NewGuid().ToString(),
                                    Status = itemInvoice.Status
                                });
                            }
                        }

                        repoAssignment.Insert(newAssignment);
                        repoAssignmentDetail.Insert(assignmentDetail);
                        #endregion

                        #region Insert new Customer Contact Agent
                        try
                        {
                            repoCustomerSales.Condition = PredicateBuilder.True<CustomerContactAgent>().And(x => x.ContactId == item.Contact.ContactId && x.SalesId == assignmentDetail.UserId);

                            var customerSales = repoCustomerSales.Find().FirstOrDefault();
                            if (customerSales == null)
                            {
                                CustomerContactAgent customerContactAgent = new CustomerContactAgent();

                                customerContactAgent.CustomerContactAgentId = Guid.NewGuid().ToString();
                                customerContactAgent.ContactId = item.Contact.ContactId;
                                customerContactAgent.SalesId = assignmentDetail.UserId;
                                customerContactAgent.CreatedBy = createdBy;
                                customerContactAgent.CreatedDt = utcNow;
                                customerContactAgent.IsDeleted = 0;
                                customerContactAgent.IsActive = 1;

                                repoCustomerSales.Insert(customerContactAgent);
                            }
                        }
                        catch
                        {
                            //
                        }
                        #endregion
                    }
                    unitOfWork.Commit();
                });
            }catch(ApplicationException appEx)
            {
                unitOfWork.Rollback();
                throw appEx;
            }catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        public void Add(AssignmentModel assignment, string createdBy, bool withInvoice = false)
        {
            try
            {
                var repoAssignmentDetail = unitOfWork.GetRepository<AssignmentDetail>();
                var repoCustomer = unitOfWork.GetRepository<Customer>();
                var repoContact = unitOfWork.GetRepository<Contact>();
                var repoInvoice = unitOfWork.GetRepository<Invoice>();

                unitOfWork.Strategy().Execute(() =>
                {
                    unitOfWork.BeginTransaction();
                    DateTime utcNow = DateTime.UtcNow;

                    #region insert new Assignment
                    Assignment newAssignment = new Assignment()
                    {
                        CreatedBy = createdBy,
                        CreatedDt = utcNow,
                        IsActive = 1,
                        IsDeleted = 0,
                        AssignmentType = assignment.AssignmentType,
                        AssignmentName = assignment.AssignmentName,
                        AssignmentDate = assignment.AssignmentDate,
                        AssignmentCode = assignment.AssignmentCode,
                        Address = assignment.AssignmentAddress,
                        AssignmentId = assignment.AssignmentId,
                        AssignmentStatusCode = assignment.AssignmentStatusCode,
                        Latitude = assignment.Latitude,
                        Longitude = assignment.Longitude
                    };

                    AssignmentDetail assignmentDetail = new AssignmentDetail
                    {
                        CreatedBy = createdBy,
                        CreatedDt = utcNow,
                        IsActive = 1,
                        IsDeleted = 0,
                        AssignmentId = newAssignment.AssignmentId,
                        AssignmentDetailId = Guid.NewGuid().ToString(),
                        ContactId = assignment.Contact.ContactId,
                        UserId = assignment.AgentId
                    };

                    if (withInvoice)
                    {
                        foreach (var invoice in assignment.Invoices)
                        {
                            repoInvoice.Insert(new Invoice
                            {
                                CreatedBy = createdBy,
                                CreatedDt = utcNow,
                                IsActive = 1,
                                IsDeleted = 0,
                                Amount = invoice.Amount,
                                AssignmentCode = newAssignment.AssignmentCode,
                                AssignmentId = newAssignment.AssignmentId,
                                InvoiceCode = invoice.InvoiceCode,
                                InvoiceId = Guid.NewGuid().ToString(),
                                Status = invoice.Status
                            });
                        }
                    }

                    repoAssignment.Insert(newAssignment);
                    repoAssignmentDetail.Insert(assignmentDetail);
                    #endregion

                    unitOfWork.Commit();
                });
            }catch(ApplicationException appEx)
            {
                unitOfWork.Rollback();
                throw appEx;
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }
        
        public void Delete(string id, string deletedBy)
        {
            try
            {
                repoAssignment.Condition = PredicateBuilder.True<Assignment>().And(x => x.AssignmentId == id && x.IsDeleted == 0 && x.IsActive == 1);
                var assignment = repoAssignment.Find().FirstOrDefault();

                if (assignment == null)
                    throw new ApplicationException("not found");

                assignment.IsDeleted = 1;
                assignment.IsActive = 0;
                assignment.UpdatedDt = DateTime.UtcNow;
                assignment.UpdatedBy = deletedBy;

                var resp = repoAssignment.Update(assignment, true);
                if (!string.IsNullOrEmpty(resp))
                    throw new ApplicationException(resp);
            }
            catch (ApplicationException appEx)
            {
                throw appEx;
            }
            catch { throw; }
        }
        #endregion

        public AssignmentModel Detail(string userId = "", string id = "")
        {
            try
            {
                var repoAssignment = unitOfWork.GetRepository<Assignment>();
                
                repoAssignment.Includes = new string[] { "AssignmentDetail", "AssignmentStatus", "AssignmentDetail.Contact", "AssignmentDetail.User", "Invoices", "Orders", "Payments" };
                repoAssignment.Condition = PredicateBuilder.True<Assignment>().And(x => x.IsDeleted == 0 && x.IsActive == 1 && x.AssignmentId == id);

                if (!string.IsNullOrEmpty(userId))
                {
                    repoAssignment.Condition = repoAssignment.Condition.And(x => x.AssignmentDetail.UserId == userId);
                }

                #region Find based on custom model
                var dbResult = repoAssignment.Find<AssignmentModel>(x => new AssignmentModel
                {
                    AssignmentId = x.AssignmentId,
                    AssignmentCode = x.AssignmentCode,
                    AssignmentStatusCode = x.AssignmentStatusCode,
                    AssignmentName = x.AssignmentName,
                    StartTime = x.AssignmentDetail.StartTime,
                    EndTime = x.AssignmentDetail.EndTime,
                    AssignmentAddress = x.Address,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    Remarks = x.AssignmentDetail.Remarks,
                    Status = x.AssignmentStatus.AssignmentStatusName,
                    Contact = (x.AssignmentDetail.Contact == null ? null : new ContactModel
                    {
                        ContactName = x.AssignmentDetail.Contact.ContactName,
                        ContactNumber = x.AssignmentDetail.Contact.ContactNumber,
                        Remarks = x.AssignmentDetail.Contact.Remarks,
                        ContactId = x.AssignmentDetail.ContactId
                    }),
                    AgentId = x.AssignmentDetail.UserId,
                    AgentCode = x.AssignmentDetail.User?.UserCode,
                    AgentName = x.AssignmentDetail.User?.UserName,
                    AssignmentDate = x.AssignmentDate,
                    Invoices = (from a in x.Invoices
                                select new InvoiceModel
                                {
                                    InvoiceId = a.InvoiceId,
                                    Amount = a.Amount,
                                    DueDate = a.DueDate,
                                    Type = (a.Amount > 0 ? (short)1 : (short)0),
                                    InvoiceCode = a.InvoiceCode,
                                    Status = a.InvoiceCode
                                }).ToList(),
                    Orders = (from b in x.Orders
                              select new OrderModel
                              {
                                  OrderId = b.OrderId,
                                  ProductAmount = b.ProductAmount,
                                  ProductCode = b.ProductCode,
                                  ProductName = b.ProductName,
                                  Quantity = b.Quantity,
                                  Discount= b.Discount
                              }).ToList(),
                    Payments = (from c in x.Payments
                                select new PaymentModel
                                {
                                    PaymentId = c.PaymentId,
                                    TransferBank = c.TransferBank,
                                    GiroNumber = c.GiroNumber,
                                    GiroPhoto = c.GiroPhoto,
                                    InvoiceCode = c.InvoiceCode,
                                    PaymentAmount = c.PaymentAmount,
                                    PaymentChannel = c.PaymentChannel,
                                    PaymentDebt = c.PaymentDebt,
                                    CashAmount = c.CashAmount,
                                    GiroAmount = c.GiroAmount,
                                    InvoiceAmount = c.InvoiceAmount,
                                    TransferAmount = c.TransferAmount,
                                    TransferDate = c.TransferDate,
                                    GiroDueDate = c.GiroDueDate
                                }).ToList()
                }).FirstOrDefault();

                if (dbResult == null)
                    throw new Exception("Assignment not found");

                return dbResult;
                #endregion
            }
            catch { throw; }
        }

        public void Edit(AssignmentModel assignment)
        {
            throw new NotImplementedException();
        }

        #region API 

        /// <summary>
        /// Create assignment for special purpose(gasoline)
        /// </summary>
        public void CreateAssignment(CreateGasolineModel createGasolineModel, string createdBy)
        {
            try
            {
                unitOfWork.Strategy().Execute(()=>
                {
                    unitOfWork.BeginTransaction();

                    //Assignment
                    DateTime utcNow = DateTime.UtcNow;
                    Assignment assignment = new Assignment
                    {
                        AssignmentId = createGasolineModel.AssignmentId,
                        AssignmentCode = createGasolineModel.AssignmentCode,
                        AssignmentStatusCode = createGasolineModel.AssignmentStatusCode,
                        Remarks = createGasolineModel.Remarks,
                        AssignmentName = createGasolineModel.AssignmentName,
                        AssignmentDate = createGasolineModel.AssignmentDate,
                        Longitude = createGasolineModel.Longitude,
                        Latitude = createGasolineModel.Latitude,
                        Address = createGasolineModel.Address,
                        AssignmentType = createGasolineModel.AssignmentType,
                        CreatedBy = createdBy,
                        CreatedDt = utcNow,
                        IsActive = 1,
                        IsDeleted = 0
                    };

                    var resp = repoAssignment.Insert(assignment);
                    if (!string.IsNullOrEmpty(resp))
                        throw new ApplicationException(resp);

                    //Assignment Detail
                    var repoAssignmentDetail = unitOfWork.GetRepository<AssignmentDetail>();

                    AssignmentDetail assignmentDetail = new AssignmentDetail
                    {
                        AssignmentDetailId = createGasolineModel.AssignmentDetailId,
                        AssignmentId = assignment.AssignmentId,
                        UserId = createGasolineModel.UserId,
                        StartTime = createGasolineModel.StartTime,
                        EndTime = createGasolineModel.EndTime,
                        Remarks = createGasolineModel.Note,
                        Attachment = createGasolineModel.Attachment,
                        Attachment1 = createGasolineModel.Attachment1,
                        Attachment2 = createGasolineModel.Attachment2,
                        AttachmentBlobId = createGasolineModel.AttachmentBlobId,
                        AttachmentBlobId1 = createGasolineModel.AttachmentBlobId1,
                        AttachmentBlobId2 = createGasolineModel.AttachmentBlobId2,
                        IsDeleted = 0,
                        IsActive = 1,
                        CreatedBy = createdBy,
                        CreatedDt = utcNow
                    };

                    resp = repoAssignmentDetail.Insert(assignmentDetail);
                    if (!string.IsNullOrEmpty(resp))
                        throw new ApplicationException(resp);

                    unitOfWork.Commit();
                });
                
            }catch(ApplicationException appEx)
            {
                unitOfWork.Rollback();
                throw appEx;
            }
            catch { unitOfWork.Rollback(); throw; }
        }

        public List<AssignmentModel> GetAssignments(string userid, int status = 0, int limit = 10, int offset = 0, string orderColumn = "AssignmentDate", string orderType = "desc", DateTime? from = null, DateTime? to = null)
        {
            try
            {
                var repoAssignment = unitOfWork.GetRepository<Assignment>();

                repoAssignment.Limit = limit;
                repoAssignment.Offset = offset;
                repoAssignment.Includes = new string[] { "AssignmentDetail", "AssignmentStatus", "Payments", "Invoices" };
                repoAssignment.Condition = PredicateBuilder.True<Assignment>().And(x => x.IsDeleted == 0 && x.IsActive == 1 && x.AssignmentDetail.UserId == userid);
                
                
                if(status == 0)
                {
                    repoAssignment.Condition = repoAssignment.Condition.And(x => x.AssignmentStatus.AssignmentStatusCode != AppConstant.AssignmentStatus.TASK_COMPLETED && x.AssignmentStatus.AssignmentStatusCode != AppConstant.AssignmentStatus.TASK_FAILED);
                }
                else
                {
                    repoAssignment.Condition = repoAssignment.Condition.And(x => x.AssignmentStatus.AssignmentStatusCode == AppConstant.AssignmentStatus.TASK_COMPLETED || x.AssignmentStatus.AssignmentStatusCode == AppConstant.AssignmentStatus.TASK_FAILED);
                }

                if (from != null && to != null)
                {
                    from = from.Value.Date.ToUtc();
                    to = to.Value.Date.ToUniversalTime().AddDays(1);

                    repoAssignment.Condition = repoAssignment.Condition
                    .And(x => x.AssignmentDate >= from.Value && x.AssignmentDate < to.Value);
                }
                else if (from != null)
                {
                    from = from.Value.ToUniversalTime();   
                    repoAssignment.Condition = repoAssignment.Condition.And(x => x.AssignmentDate >= from.Value && x.AssignmentDate < from.Value.AddDays(1));
                }

                repoAssignment.OrderBy = new Trisatech.AspNet.Common.Models.SqlOrderBy
                {
                    Column = orderColumn,
                    Type = orderType
                };

                #region Find based on custom model
                var dbResult = repoAssignment.Find<AssignmentModel>(x => new AssignmentModel
                {
                    AssignmentId = x.AssignmentId,
                    AssignmentCode = x.AssignmentCode,
                    AssignmentStatusCode = x.AssignmentStatusCode,
                    AssignmentName = x.AssignmentName,
                    AssignmentDate = x.AssignmentDate,
                    AssignmentType = x.AssignmentType,
                    StartTime = x.AssignmentDetail.StartTime,
                    EndTime = x.AssignmentDetail.EndTime,
                    LostTime = (double)x.AssignmentDetail.LostTime/3600,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    AssignmentAddress = x.Address,
                    Remarks = x.Remarks,
                    Status = x.AssignmentStatus.AssignmentStatusName,
                    IsAssignmentDetailCompleted = (x.Payments != null && x.Payments.Count > 0 ? true : false)
                }).ToList();

                return dbResult;
                #endregion
            }
            catch{ throw; }
        }

        public long? GetAssignmentsCount(string userid, int status = 0, int limit = 10, int offset = 0, string orderColumn = "AssignmentDate", string orderType = "desc", DateTime? from = null, DateTime? to = null)
        {
            try
            {
                var repoAssignment = unitOfWork.GetRepository<Assignment>();
                repoAssignment.Includes = new string[] { "AssignmentDetail", "AssignmentStatus", "AssignmentDetail.Contact", "AssignmentDetail.User", "Invoices", "Orders", "Payments" };
                repoAssignment.Condition = PredicateBuilder.True<Assignment>().And(x => x.IsDeleted == 0 && x.IsActive == 1 && x.AssignmentDetail.UserId == userid);
                
                if (status == 0)
                {
                    repoAssignment.Condition = repoAssignment.Condition.And(x => x.AssignmentStatus.AssignmentStatusCode != AppConstant.AssignmentStatus.TASK_COMPLETED && x.AssignmentStatus.AssignmentStatusCode != AppConstant.AssignmentStatus.TASK_FAILED);
                }
                else
                {
                    repoAssignment.Condition = repoAssignment.Condition.And(x => x.AssignmentStatus.AssignmentStatusCode == AppConstant.AssignmentStatus.TASK_COMPLETED || x.AssignmentStatus.AssignmentStatusCode == AppConstant.AssignmentStatus.TASK_FAILED);
                }

                if (from != null && to != null)
                {
                    from = from.Value.ToUtc();
                    to = to.Value.ToUtc(24);
                    repoAssignment.Condition = repoAssignment.Condition.And(x => x.AssignmentDate >= from.Value && x.AssignmentDate < to.Value);
                }
                else if (from != null)
                {
                    from = from.Value.ToUtc();
                    repoAssignment.Condition = repoAssignment.Condition.And(x => x.AssignmentDate >= from.Value && x.AssignmentDate < from.Value.AddDays(1));
                }

                repoAssignment.OrderBy = new Trisatech.AspNet.Common.Models.SqlOrderBy
                {
                    Column = orderColumn,
                    Type = orderType
                };

                #region Find based on custom model
                return repoAssignment.Count();
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<AssignmentModel> GetAssignmentsByDate(string userid, DateTime date, DateTime? to, short status = 0, int limit = 10, int offset = 0)
        {
            try
            {
                date = date.Date.ToUtc();

                var repoAssignment = unitOfWork.GetRepository<Assignment>();

                repoAssignment.Limit = limit;
                repoAssignment.Offset = offset;
                repoAssignment.Includes = new string[] { "AssignmentDetail", "AssignmentStatus" };
                repoAssignment.Condition = PredicateBuilder.True<Assignment>().And(x => x.IsDeleted == 0 && x.IsActive == 1 && x.AssignmentDetail.UserId == userid);

                if (status == 0)
                {
                    repoAssignment.Condition = repoAssignment.Condition.And(x => x.AssignmentStatus.AssignmentStatusCode != AppConstant.AssignmentStatus.TASK_COMPLETED && x.AssignmentStatus.AssignmentStatusCode != AppConstant.AssignmentStatus.TASK_FAILED);
                }
                else
                {
                    repoAssignment.Condition = repoAssignment.Condition.And(x => x.AssignmentStatus.AssignmentStatusCode == AppConstant.AssignmentStatus.TASK_COMPLETED || x.AssignmentStatus.AssignmentStatusCode == AppConstant.AssignmentStatus.TASK_FAILED);
                }
                
                if(to != null)
                {
                    to = to.Value.ToUtc(24);
                    repoAssignment.Condition = repoAssignment.Condition.And(x => x.AssignmentDate >= date && x.AssignmentDate < to.Value);
                }
                else
                {
                    repoAssignment.Condition = repoAssignment.Condition.And(x => x.AssignmentDate >= date && x.AssignmentDate < date.AddDays(1));
                }

                #region Find based on custom model
                var dbResult = repoAssignment.Find<AssignmentModel>(x => new AssignmentModel
                {
                    AssignmentId = x.AssignmentId,
                    AssignmentCode = x.AssignmentCode,
                    AssignmentStatusCode = x.AssignmentStatusCode,
                    AssignmentName = x.AssignmentName,
                    AssignmentAddress = x.Address,
                    AssignmentDate = x.AssignmentDate,
                    StartTime = x.AssignmentDetail.StartTime,
                    EndTime = x.AssignmentDetail.EndTime,
                    AssignmentType = x.AssignmentType,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    Remarks = x.AssignmentDetail.Remarks,
                    Status = x.AssignmentStatus.AssignmentStatusName,
                    IsAssignmentDetailCompleted = (x.Payments != null && x.Payments.Count > 0 ? true : false)
                }).ToList();
                return dbResult;
                #endregion

            }
            catch { throw; }
        }

        public List<AssignmentStatusModel> GetAssignmentStatus()
        {
            try
            {
                var repoAssignmentStatus = unitOfWork.GetRepository<AssignmentStatus>();

                var result = repoAssignmentStatus.Find<AssignmentStatusModel>(x => new AssignmentStatusModel
                {
                    AssignmentStatusCode = x.AssignmentStatusCode,
                    AssignmentStatusName = x.AssignmentStatusName,
                    Description = x.Description
                }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public List<AssignmentModel> Search(string userid, string keywords, short status = 0, DateTime? date = null, int limit = 10, int offset = 0)
        {
            try
            {
                var repoAssignment = unitOfWork.GetRepository<Assignment>();

                repoAssignment.Limit = limit;
                repoAssignment.Offset = offset;
                repoAssignment.Includes = new string[] { "AssignmentDetail", "AssignmentStatus" };
                repoAssignment.Condition = PredicateBuilder.True<Assignment>().And(x => x.IsDeleted == 0 && x.IsActive == 1 && x.AssignmentDetail.UserId == userid);

                if (status == 0)
                {
                    repoAssignment.Condition = repoAssignment.Condition.And(x => x.AssignmentStatus.AssignmentStatusCode != AppConstant.AssignmentStatus.TASK_COMPLETED && x.AssignmentStatus.AssignmentStatusCode != AppConstant.AssignmentStatus.TASK_FAILED);
                }
                else
                {
                    repoAssignment.Condition = repoAssignment.Condition.And(x => x.AssignmentStatus.AssignmentStatusCode == AppConstant.AssignmentStatus.TASK_COMPLETED || x.AssignmentStatus.AssignmentStatusCode == AppConstant.AssignmentStatus.TASK_FAILED);
                }

                if (!string.IsNullOrEmpty(keywords))
                {
                    repoAssignment.Condition = repoAssignment.Condition.And(x => x.AssignmentName.Contains(keywords));
                }

                if (date != null)
                {
                    date = date.Value.ToUtc();
                    repoAssignment.Condition = repoAssignment.Condition.And(x => x.AssignmentDate >= date && x.AssignmentDate < date.Value.AddDays(1));
                }

                #region Find based on custom model
                var dbResult = repoAssignment.Find<AssignmentModel>(x => new AssignmentModel
                {
                    AssignmentId = x.AssignmentId,
                    AssignmentCode = x.AssignmentCode,
                    AssignmentStatusCode = x.AssignmentStatusCode,
                    AssignmentName = x.AssignmentName,
                    AssignmentAddress = x.Address,
                    AssignmentDate = x.AssignmentDate,
                    StartTime = x.AssignmentDetail.StartTime,
                    EndTime = x.AssignmentDetail.EndTime,
                    AssignmentType = x.AssignmentType,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    Remarks = x.AssignmentDetail.Remarks,
                    Status = x.AssignmentStatus.AssignmentStatusName
                }).ToList();
                return dbResult;
                #endregion

                #region Find()
                /*
                var dbResult = repoAssignment.Find().ToList();

                List<AssignmentModel> assignmentModels = new List<AssignmentModel>();
                if (dbResult != null && dbResult.Count > 0)
                {
                    foreach (var item in dbResult)
                    {
                        assignmentModels.Add(new AssignmentModel
                        {
                            AssignmentId = item.AssignmentId,
                            AssignmentCode = item.AssignmentCode,
                            AssignmentStatusCode = item.AssignmentStatusCode,
                            AssignmentName = item.AssignmentName,
                            StartTime = item.AssignmentDetail.StartTime,
                            EndTime = item.AssignmentDetail.EndTime,
                            Latitude = item.Latitude,
                            Longitude = item.Longitude,
                            Remarks = item.AssignmentDetail.Remarks,
                            Status = item.AssignmentStatus.AssignmentStatusName
                        });
                    }
                }

                return assignmentModels;
                */
                #endregion

                #region LINQ
                /*
                var list = (from a in dbContext.Assignments
                            join b in dbContext.AssignmentDetails on a.AssignmentId equals b.AssignmentId
                            where b.UserId == userid && a.AssignmentName.Contains(keywords)
                            orderby a.CreatedDt descending
                            select new AssignmentModel
                            {
                                AssignmentId = a.AssignmentId,
                                AssignmentCode = a.AssignmentCode,
                                AssignmentStatusCode = a.AssignmentStatusCode,
                                AssignmentName = a.AssignmentName,
                                StartTime = b.StartTime,
                                EndTime = b.EndTime,
                                Latitude = a.Latitude,
                                Longitude = a.Longitude,
                                Remarks = b.Remarks,
                                Status = a.AssignmentStatus.AssignmentStatusName
                            }).ToList();

                return list;
                */
                #endregion
            }
            catch { throw; }
        }

        public void Complete(string userId, string assignmentId, 
            double latitude, 
            double longitude, 
            string remarks, 
            DateTime endTime, 
            string assignmentStatus, 
            string reasonCode = "", string attachmentUrl = "", string attachmentBlobId = "")
        {
            try
            {
                var repoUserActivity = unitOfWork.GetRepository<UserActivity>();
                var repoAssignmentDetail = unitOfWork.GetRepository<AssignmentDetail>();

                repoAssignment.Includes = new string[] { "AssignmentDetail", "AssignmentStatus", "AssignmentDetail.Contact" };

                repoAssignment.Condition = PredicateBuilder.True<Assignment>().And(x => x.AssignmentId == assignmentId && 
                                            x.AssignmentDetail.UserId == userId && 
                                            //x.AssignmentStatusCode == "AGENT_STARTED" && 
                                            x.IsActive == 1 && 
                                            x.IsDeleted == 0);

                var assignment = repoAssignment.Find().FirstOrDefault();
                if (assignment == null)
                    throw new ApplicationException("not found");

                unitOfWork.Strategy().Execute(()=>
                {
                    unitOfWork.BeginTransaction();

                    DateTime utcNow = DateTime.UtcNow;

                    var assignmentDetail = assignment.AssignmentDetail;

                    assignmentDetail.EndTime = endTime;
                    assignmentDetail.Remarks = remarks;
                    assignmentDetail.UpdatedDt = utcNow;
                    assignmentDetail.UpdatedBy = userId;

                    if (!string.IsNullOrEmpty(attachmentUrl) && !string.IsNullOrEmpty(attachmentBlobId))
                    {
                        assignmentDetail.Attachment = attachmentUrl;
                        assignmentDetail.AttachmentBlobId = attachmentBlobId;
                    }

                    assignment.AssignmentStatusCode = assignmentStatus;

                    if (!string.IsNullOrEmpty(reasonCode))
                        assignment.RefAssignmentCode = reasonCode;

                    assignment.UpdatedBy = userId;
                    assignment.UpdatedDt = utcNow;

                    var userActivity = new UserActivity
                    {
                        AssignmentId = assignment.AssignmentId,
                        Latitude = latitude,
                        Longitude = longitude,
                        AssignmentStatusCode = assignmentStatus,
                        ActivityTypeEnum = UserActivityEnum.COMPLETE_ASSIGNMENT,
                        CreatedBy = userId,
                        CreatedDt = utcNow
                    };

                    string respStr = repoAssignmentDetail.Update(assignmentDetail);
                    if (!string.IsNullOrEmpty(respStr))
                        throw new ApplicationException(respStr);

                    respStr = repoAssignment.Update(assignment);
                    if (!string.IsNullOrEmpty(respStr))
                        throw new ApplicationException(respStr);

                    repoUserActivity.Insert(userActivity);

                    unitOfWork.Commit();
                });
                
               AsyncUpdateCustomerInfo(repoCustomer,
                   assignment.AssignmentDetail.Contact.CustomerId,
                   latitude, 
                   longitude);
            }
            catch (ApplicationException appEx)
            {
                unitOfWork.Rollback();
                throw appEx;
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        private void AsyncUpdateCustomerInfo(IRepository<Customer> repoCustomer, string customerId, double lat, double lng)
        {
            try
            {
                repoCustomer.Condition = PredicateBuilder.True<Customer>().And(x => x.CustomerId == customerId);

                var customer = repoCustomer.Find().FirstOrDefault();
                
                if(customer != null)
                {
                    unitOfWork.Strategy().Execute(() =>
                    {
                        customer.CustomerLatitude = lat;
                        customer.CustomerLongitude = lng;

                        repoCustomer.Update(customer, true);
                    });
                }
            }
            catch
            {
                //
            }
        }

        public void Start(string userId, 
            string assignmentId, 
            double latitude, 
            double longitude, 
            DateTime startTime, 
            string assignmentStatus, 
            int lostTime = 0,
            int salesTime = 0,
            int googleTime = 0)
        {
            try
            {
                var repoUserActivity = unitOfWork.GetRepository<UserActivity>();
                var repoAssignmentDetail = unitOfWork.GetRepository<AssignmentDetail>();

                repoAssignment.Includes = new string[] { "AssignmentDetail", "AssignmentStatus" };
                repoAssignment.Condition = PredicateBuilder.True<Assignment>().And(x => x.AssignmentId == assignmentId 
                && x.AssignmentDetail.UserId == userId 
                && x.AssignmentStatusCode == AppConstant.AssignmentStatus.TASK_RECEIVED
                && x.IsActive == 1 
                && x.IsDeleted == 0);
                
                var assignment = repoAssignment.Find().FirstOrDefault();

                if (assignment == null)
                    throw new ApplicationException("not found");

                unitOfWork.Strategy().Execute(()=>
                {
                    unitOfWork.BeginTransaction();

                    DateTime utcNow = DateTime.UtcNow;

                    repoAssignmentDetail.Condition = PredicateBuilder.True<AssignmentDetail>().And(x => x.AssignmentDetailId == assignment.AssignmentDetail.AssignmentDetailId);

                    var assignmentDetail = repoAssignmentDetail.Find().FirstOrDefault();

                    if (assignmentDetail == null)
                        throw new ApplicationException("not found");

                    assignmentDetail.StartTime = startTime;
                    assignmentDetail.UpdatedDt = utcNow;
                    assignmentDetail.UpdatedBy = userId;
                    assignmentDetail.LostTime = lostTime;
                    assignmentDetail.SalesTime = salesTime;
                    assignmentDetail.GoogleTime = googleTime;

                    assignment.AssignmentStatusCode = assignmentStatus;
                    assignment.UpdatedBy = userId;
                    assignment.UpdatedDt = utcNow;

                    var userActivity = new UserActivity
                    {
                        AssignmentId = assignment.AssignmentId,
                        Latitude = latitude,
                        Longitude = longitude,
                        AssignmentStatusCode = assignmentStatus,
                        ActivityTypeEnum = UserActivityEnum.START_ASSIGNMENT,
                        CreatedBy = userId,
                        CreatedDt = utcNow
                    };

                    string respStr = repoAssignmentDetail.Update(assignmentDetail);

                    if (!string.IsNullOrEmpty(respStr))
                        throw new ApplicationException(respStr);

                    respStr = repoAssignment.Update(assignment);
                    if (!string.IsNullOrEmpty(respStr))
                        throw new ApplicationException(respStr);

                    repoUserActivity.Insert(userActivity);

                    unitOfWork.Commit();
                });
                
            }
            catch (ApplicationException appEx)
            {
                unitOfWork.Rollback();
                throw appEx;
            }catch(Exception ex)
            {
                unitOfWork.Rollback();
                throw ex;
            }
        }

        public List<AssignmentModel> GetAssignmentDetail(string user, int status = 0, int limit = 10, int offset = 0, string orderColumn = "AssignmentDate", string orderType = "desc", DateTime? from = null, DateTime? to = null)
        {
            try
            {
                var repoAssignment = unitOfWork.GetRepository<Assignment>();

                repoAssignment.Limit = limit;
                repoAssignment.Offset = offset;
                repoAssignment.Includes = new string[] { "AssignmentDetail", "AssignmentStatus", "AssignmentDetail.Contact", "AssignmentDetail.User", "Invoices", "Orders", "Payments" };
                repoAssignment.Condition = PredicateBuilder.True<Assignment>().And(x => 
                x.IsDeleted == 0 &&
                x.IsActive == 1 &&
                x.AssignmentDetail.UserId == user);
                
                if (status == 0)
                {
                    repoAssignment.Condition = repoAssignment.Condition.And(x => x.AssignmentStatus.AssignmentStatusCode != AppConstant.AssignmentStatus.TASK_COMPLETED && x.AssignmentStatus.AssignmentStatusCode != AppConstant.AssignmentStatus.TASK_FAILED);
                }
                else
                {
                    repoAssignment.Condition = repoAssignment.Condition.And(x => x.AssignmentStatus.AssignmentStatusCode == AppConstant.AssignmentStatus.TASK_COMPLETED || x.AssignmentStatus.AssignmentStatusCode == AppConstant.AssignmentStatus.TASK_FAILED);
                }

                if (from != null && to != null)
                {
                    from = from.Value.Date.ToUtc();
                    to = to.Value.Date.ToUtc(24);
                    repoAssignment.Condition = repoAssignment.Condition.And(x => x.AssignmentDate >= from.Value && x.AssignmentDate < to.Value);
                }
                else if (from != null)
                {
                    from = from.Value.Date.ToUtc();
                    repoAssignment.Condition = repoAssignment.Condition.And(x => x.AssignmentDate >= from.Value && x.AssignmentDate < from.Value.AddDays(1));
                }

                repoAssignment.OrderBy = new Trisatech.AspNet.Common.Models.SqlOrderBy
                {
                    Column = orderColumn,
                    Type = orderType
                };

                #region Find based on custom model
                var dbResult = repoAssignment.Find<AssignmentModel>(x => new AssignmentModel
                {
                    AssignmentId = x.AssignmentId,
                    AssignmentCode = x.AssignmentCode,
                    AssignmentStatusCode = x.AssignmentStatusCode,
                    AssignmentType = x.AssignmentType,
                    AssignmentName = x.AssignmentName,
                    StartTime = x.AssignmentDetail.StartTime,
                    EndTime = x.AssignmentDetail.EndTime,
                    AssignmentAddress = x.Address,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    Remarks = x.AssignmentDetail.Remarks,
                    Status = x.AssignmentStatus.AssignmentStatusName,
                    Contact = (x.AssignmentDetail.Contact == null ? null : new ContactModel
                    {
                        ContactName = x.AssignmentDetail.Contact.ContactName,
                        ContactNumber = x.AssignmentDetail.Contact.ContactNumber,
                        Remarks = x.AssignmentDetail.Contact.Remarks,
                        ContactId = x.AssignmentDetail.ContactId
                    }),
                    AgentId = x.AssignmentDetail.UserId,
                    AgentCode = x.AssignmentDetail.User?.UserCode,
                    AgentName = x.AssignmentDetail.User?.UserName,
                    AssignmentDate = x.AssignmentDate,
                    Invoices = (from a in x.Invoices
                                select new InvoiceModel
                                {
                                    InvoiceId = a.InvoiceId,
                                    Amount = a.Amount,
                                    DueDate = a.DueDate,
                                    Type = (a.Amount > 0 ? (short)1 : (short)0),
                                    InvoiceCode = a.InvoiceCode,
                                    Status = a.InvoiceCode
                                }).ToList(),
                    Orders = (from b in x.Orders
                              select new OrderModel
                              {
                                  OrderId = b.OrderId,
                                  ProductAmount = b.ProductAmount,
                                  ProductCode = b.ProductCode,
                                  ProductName = b.ProductName,
                                  Quantity = b.Quantity,
                                  Discount = b.Discount
                              }).ToList(),
                    Payments = (from c in x.Payments
                                select new PaymentModel
                                {
                                    PaymentId = c.PaymentId,
                                    TransferBank = c.TransferBank,
                                    GiroNumber = c.GiroNumber,
                                    GiroPhoto = c.GiroPhoto,
                                    InvoiceCode = c.InvoiceCode,
                                    PaymentAmount = c.PaymentAmount,
                                    PaymentChannel = c.PaymentChannel,
                                    PaymentDebt = c.PaymentDebt,
                                    CashAmount = c.CashAmount,
                                    GiroAmount = c.GiroAmount,
                                    InvoiceAmount = c.InvoiceAmount,
                                    TransferAmount = c.TransferAmount,
                                    TransferDate = c.TransferDate,
                                    GiroDueDate = c.GiroDueDate
                                }).ToList(),
                }).ToList();

                return dbResult;
                #endregion
            }
            catch { throw; }
        }

        public bool AllowToStartAssignment(string userId, DateTime utcNow)
        {
            if (string.IsNullOrEmpty(userId))
                return false;

            try
            {
                repoAssignment.Includes = new string[] { "AssignmentDetail" };
                repoAssignment.Condition = PredicateBuilder.True<Assignment>()
                    .And(x => x.AssignmentDetail.UserId == userId 
                    && x.AssignmentDetail.StartTime.Date == utcNow.Date && x.AssignmentStatusCode == AppConstant.AssignmentStatus.AGENT_STARTED);

                if (repoAssignment.Count() == null || (int)repoAssignment.Count() == 0)
                    return true;

                return false;
            }catch(ApplicationException appEx)
            {
                throw appEx;
            }
            catch { throw; }
        }

        public async Task CopyTask(string userId, string targetUserId, DateTime utcNow)
        {
            var fromDate = utcNow.Date;
            var toDate = fromDate.AddDays(1);

            var userTask = await (from a in dbContext.Assignments
                            .Include(x=>x.AssignmentDetail)
                            .ThenInclude(x=>x.User)
                            .Include(x=>x.Invoices)
                            where a.IsDeleted == 0 && a.AssignmentDetail.UserId == userId &&
                            a.AssignmentDate >= fromDate && a.AssignmentDate < toDate
                            select a).ToListAsync();

            if (userTask == null || userTask.Count == 0)
                return;

            var userTarget = await dbContext.Users.FirstOrDefaultAsync(x => x.UserId == targetUserId && x.IsDeleted == 0);

            if (userTarget == null)
                return;

            InsertNewAssignment(userTask, userTarget);
        }

        private void InsertNewAssignment(List<Assignment> userTask, User user)
        {
            try
            {
                var repoAssignmentDetail = unitOfWork.GetRepository<AssignmentDetail>();
                var repoInvoice = unitOfWork.GetRepository<Invoice>();
                
                unitOfWork.Strategy().Execute(() =>
                {
                    unitOfWork.BeginTransaction();

                    foreach (var item in userTask)
                    {
                        #region insert new Assignment
                        Assignment newTask = new Assignment();
                        AssignmentDetail newTaskDetail = new AssignmentDetail();

                        CopyAssignment(item, newTask);
                        CopyAssignmentDetail(item.AssignmentDetail, newTaskDetail, user.UserId, user.UserCode, newTask.AssignmentId);

                        if(item.Invoices != null && item.Invoices.Count > 0)
                        {
                            AddInvoice(item.Invoices, newTask.AssignmentCode, newTask.AssignmentId, repoInvoice);
                        }
                        
                        repoAssignment.Insert(newTask);
                        repoAssignmentDetail.Insert(newTaskDetail);
                        #endregion
                    }
                    unitOfWork.Commit();
                });

            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        private void CopyAssignmentDetail(AssignmentDetail assignmentDetail, AssignmentDetail newTaskDetail, string userId, string userCode, string assignmentId)
        {
            newTaskDetail.AssignmentDetailId = Guid.NewGuid().ToString();
            newTaskDetail.UserId = userId;
            newTaskDetail.UserCode = userCode;
            newTaskDetail.AssignmentId = assignmentId;
            newTaskDetail.IsVerified = false;
            newTaskDetail.ContactId = assignmentDetail.ContactId;
            newTaskDetail.CreatedBy = assignmentDetail.CreatedBy;
            newTaskDetail.CreatedDt = assignmentDetail.CreatedDt;
            newTaskDetail.IsActive = 1;
            newTaskDetail.IsDeleted = 0;
        }

        private void CopyAssignment(Assignment item, Assignment newTask)
        {

            newTask.AssignmentId = Guid.NewGuid().ToString();
            newTask.AssignmentType = AssignmentType.CICO;
            newTask.AssignmentCode = item.AssignmentCode;
            newTask.AssignmentStatusCode = item.AssignmentStatusCode;
            newTask.CreatedBy = item.CreatedBy;
            newTask.CreatedDt = item.CreatedDt;
            newTask.IsActive = 1;
            newTask.IsDeleted = 0;
            newTask.AssignmentName = item.AssignmentName;
            newTask.AssignmentDate = item.AssignmentDate;
            newTask.Address = item.Address;
            newTask.Latitude = item.Latitude;
            newTask.Longitude = item.Longitude;
        }

        private void AddInvoice(ICollection<Invoice> invoices, string assignmentCode, string assignmentId, IRepository<Invoice> repoInvoice)
        {
            foreach(var item in invoices)
            {
                Invoice invoice = new Invoice();

                CopyInvoice(item, invoice, assignmentId, assignmentCode);
                repoInvoice.Insert(invoice);
            }
        }

        private void CopyInvoice(Invoice source, Invoice destination, string assignmentId, string assignmentCode)
        {
            destination.InvoiceId = Guid.NewGuid().ToString();
            destination.AssignmentCode = assignmentCode;
            destination.AssignmentId = assignmentId;
            destination.DueDate = source.DueDate;
            destination.Amount = source.Amount;
            destination.InvoiceCode = source.InvoiceCode;
            destination.Status = source.Status;
            destination.CreatedBy = source.CreatedBy;
            destination.CreatedDt = source.CreatedDt;
            destination.IsActive = 1;
            destination.IsDeleted = 0;


        }

        #endregion
    }
}
