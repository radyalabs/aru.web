using Trisatech.MWorkforce.Business.Entities;
using Trisatech.MWorkforce.Business.Interfaces;
using Trisatech.MWorkforce.Domain;
using Trisatech.MWorkforce.Domain.Entities;
using Trisatech.AspNet.Common.Interfaces;
using Trisatech.AspNet.Common.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Trisatech.AspNet.Common.Helpers;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Trisatech.MWorkforce.Business.Services
{
    public class CustomerService : ICustomerService
    {
        private IUnitOfWork unitOfWork;
        private IRepository<Customer> repoCustomer;
        private IRepository<CustomerDetail> repoCustDetail;
        private IRepository<Contact> repoContact;
        private MobileForceContext dbContext;

        public CustomerService(MobileForceContext dbContext)
        {
            this.dbContext = dbContext;
            unitOfWork = new Trisatech.AspNet.Common.Repository.UnitOfWork<MobileForceContext>(dbContext);
            repoCustomer = unitOfWork.GetRepository<Customer>();
            repoContact = unitOfWork.GetRepository<Contact>();
            repoCustDetail = unitOfWork.GetRepository<CustomerDetail>();
        }
        public void Add(CustomerModel customerModel, string createdBy, bool addAsContact = false)
        {
            try
            {
                unitOfWork.Strategy().Execute(() =>
                {
                    unitOfWork.BeginTransaction();

                    repoCustomer.Condition = PredicateBuilder.True<Customer>().And(x => x.CustomerEmail == customerModel.CustomerEmail && x.IsDeleted == 1);
                    var customerExist = repoCustomer.Find().FirstOrDefault();
                    if (customerExist != null)
                        throw new ApplicationException(customerModel.CustomerEmail + " already exist.");
                    customerExist = null;

                    #region add customer to db
                    Customer customer = new Customer();
                    CopyProperty.CopyPropertiesTo(customerModel, customer);
                    customer.CreatedBy = createdBy;
                    customer.CreatedDt = DateTime.UtcNow;
                    customer.IsActive = 1;
                    customer.IsDeleted = 0;


                    repoCustomer.Insert(customer);

                    if (addAsContact)
                    {
                        var contactRepo = unitOfWork.GetRepository<Contact>();
                        var contact = new Contact
                        {
                            ContactId = Guid.NewGuid().ToString(),
                            CustomerId = customer.CustomerId,
                            ContactName = customer.CustomerName,
                            ContactNumber = customer.CustomerPhoneNumber,
                            CreatedBy = "",
                            CreatedDt = DateTime.UtcNow,
                            Email = customer.CustomerEmail,
                            IsActive = 1,
                            IsDeleted = 0
                        };

                        contactRepo.Insert(contact);
                    }
                    #endregion

                    unitOfWork.Commit();
                });
                
            }catch(ApplicationException ex)
            {
                unitOfWork.Rollback();
                throw ex;
            }catch(Exception ex)
            {
                unitOfWork.Rollback();
                throw ex;
            }
        }
        public void AddCustomer(CustomerDetailModel model, string createdBy)
        {
            try
            {
                unitOfWork.Strategy().Execute(() =>
                {
                    unitOfWork.BeginTransaction();

                    var repoCustomerDetail = unitOfWork.GetRepository<CustomerDetail>();
                    DateTime utcNow = DateTime.UtcNow;
                    Customer customer = new Customer();
                    CustomerDetail customerDetail = new CustomerDetail();
                    Contact ownerContact = new Contact();
                    Contact pic = new Contact();

                    //customer
                    customer.CustomerId = Guid.NewGuid().ToString();
                    customer.CustomerName = model.StoreName;
                    customer.CustomerAddress = model.OwnerAddress;
                    customer.CustomerCity = model.OwnerCity;
                    customer.CustomerDistrict = model.OwnerDistrict;
                    customer.CustomerVillage = model.OwnerVillage;
                    customer.CustomerPhoneNumber = model.OwnerPhoneNumber;
                    customer.Desc = model.Note;
                    customer.CustomerPhotoId = model.PhotoIdCardUrl;
                    customer.CustomerPhotoBlobId = model.PhotoIdCardBlobId;
                    customer.CustomerPhotoNPWP = model.PhotoNPWPUrl;
                    customer.CustomerPhotoNPWPBlobId = model.PhotoNPWPBlobId;
                    customer.CustomerLatitude = model.StoreLatitude;
                    customer.CustomerLongitude = model.StoreLongitude;
                    customer.CreatedBy = createdBy;
                    customer.CreatedDt = utcNow;
                    customer.IsDeleted = 0;
                    customer.IsActive = 1;

                    //customer detail
                    customerDetail.BrandName = model.StoreName;
                    customerDetail.BrandAddress = model.StoreAddress;
                    customerDetail.BrandCity = model.StoreCity;
                    customerDetail.BrandDistrict = model.StoreName;
                    customerDetail.BrandVillage = model.StoreName;
                    customerDetail.BrandStatus = model.StoreName;
                    customerDetail.BrandType = model.StoreName;
                    customerDetail.Reserved = model.WidthRoad;
                    customerDetail.Desc = model.Note;
                    customerDetail.BrandAge = model.StoreAge;
                    customerDetail.BrandPhotoUrl = model.StorePhotoUrl;
                    customerDetail.BrandPhotoUrl1 = model.BrandingPhotoUrl;
                    customerDetail.BrandPhotoBlobId = model.StorePhotoBlobId;
                    customerDetail.brandPhoto1BlobId = model.BrandingPhotoBlobId;
                    customerDetail.CustomerId = customer.CustomerId;
                    customerDetail.BrandLatitude = model.StoreLatitude;
                    customerDetail.BrandLongitude = model.StoreLongitude;

                    //contact
                    ownerContact.ContactId = Guid.NewGuid().ToString();
                    ownerContact.ContactName = model.OwnerName;
                    ownerContact.ContactNumber = model.OwnerPhoneNumber;
                    ownerContact.Position = "Pemilik Toko";
                    ownerContact.CustomerId = customer.CustomerId;
                    ownerContact.IsActive = 1;
                    ownerContact.IsDeleted = 0;
                    ownerContact.CreatedBy = createdBy;
                    ownerContact.CreatedDt = utcNow;

                    pic.ContactId = Guid.NewGuid().ToString();
                    pic.ContactName = model.PICName;
                    pic.ContactNumber = model.PICPhoneNumber;
                    pic.Position = "PIC";
                    pic.CustomerId = customer.CustomerId;
                    pic.IsActive = 1;
                    pic.IsDeleted = 0;
                    pic.CreatedBy = createdBy;
                    pic.CreatedDt = utcNow;

                    repoCustomer.Insert(customer);
                    repoCustomerDetail.Insert(customerDetail);
                    repoContact.Insert(ownerContact);
                    repoContact.Insert(pic);

                    unitOfWork.Commit();
                });
                
            }
            catch (ApplicationException appEx)
            {
                unitOfWork.Rollback();
                throw appEx;
            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                throw ex;
            }
        }

        public void Delete(string id, string deletedBy)
        {
            try
            {
                repoCustomer.Condition = PredicateBuilder.True<Customer>().And(x => x.IsDeleted == 0 && x.IsActive == 1 && x.CustomerId == id);
                var customer = repoCustomer.Find().FirstOrDefault();

                if (customer == null)
                    throw new ApplicationException("not found");
                customer.IsDeleted = 1;
                customer.UpdatedBy = deletedBy;
                customer.UpdatedDt = DateTime.UtcNow;

                var resp = repoCustomer.Update(customer, true);
                if (!string.IsNullOrEmpty(resp))
                    throw new ApplicationException(resp);
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Edit(CustomerModel customerModel, string updatedBy)
        {
            try
            {
                repoCustomer.Condition = PredicateBuilder.True<Customer>().And(x => x.CustomerEmail == customerModel.CustomerEmail && x.IsActive == 1 && x.IsDeleted == 0);
                var customerExist = repoCustomer.Find().FirstOrDefault();

                if (customerExist != null && customerExist.CustomerId != customerModel.CustomerId)
                    throw new ApplicationException(customerModel.CustomerEmail + " already exist.");

                repoCustomer.Condition = PredicateBuilder.True<Customer>().And(x => x.CustomerId == customerModel.CustomerId && x.IsActive == 1 && x.IsDeleted == 0);
                var customer = repoCustomer.Find().FirstOrDefault();

                if (customer == null)
                    throw new ApplicationException("not found");

                customer.CustomerEmail = customerModel.CustomerEmail;
                customer.CustomerName = customerModel.CustomerName;
                //customer.CustomerCode = customerModel.CustomerCode;
                customer.CustomerCity = customerModel.CustomerCity;
                customer.CustomerAddress = customerModel.CustomerAddress;
                customer.CustomerPhoneNumber = customerModel.CustomerPhoneNumber;
                customer.CustomerLatitude = customerModel.CustomerLatitude;
                customer.CustomerLongitude = customerModel.CustomerLongitude;
                customer.UpdatedBy = updatedBy;
                customer.UpdatedDt = DateTime.UtcNow;

                repoCustomer.Update(customer, true);
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CustomerModel> Get(string keyword = "", int limit = 20, int offset = 0)
        {
            try
            {
                repoCustomer.Condition = PredicateBuilder.True<Customer>().And(x => x.IsActive == 1 && x.IsDeleted == 0);
                repoCustomer.Includes = new string[] { "Contacts" };
                repoCustomer.Limit = limit;
                repoCustomer.Offset = offset;
                repoCustomer.OrderBy = new Trisatech.AspNet.Common.Models.SqlOrderBy { Type = "asc", Column = "CustomerName" };
                
                if (!string.IsNullOrEmpty(keyword))
                {
                    repoCustomer.Condition = repoCustomer.Condition.And(x => x.CustomerName.Contains(keyword) || x.CustomerEmail.Contains(keyword));
                }
                
                #region Find based on custom model
                var dbResult = repoCustomer.Find<CustomerModel>(x => new CustomerModel
                {
                    CustomerId = x.CustomerId,
                    CustomerName = x.CustomerName,
                    CustomerEmail = x.CustomerEmail,
                    CustomerAddress = x.CustomerAddress,
                    CustomerCity = x.CustomerCity,
                    CustomerLatitude = x.CustomerLatitude,
                    CustomerLongitude = x.CustomerLongitude,
                    CustomerPhoto = x.CustomerPhoto,
                    Contacts = (x.Contacts == null ? null : x.Contacts.Select(y=> new ContactModel
                    {
                        ContactName = y.ContactName,
                        ContactNumber = y.ContactNumber,
                        CustomerCode = x.CustomerCode,
                    }).ToList())
                }).ToList();

                return dbResult;
                #endregion
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CustomerModel Get(string id)
        {
            try
            {
                repoCustomer.Condition = PredicateBuilder.True<Customer>().And(x => x.CustomerId == id && x.IsActive == 1 && x.IsDeleted == 0);
                var customer = repoCustomer.Find<CustomerModel>(x => new CustomerModel
                {
                    CustomerId = x.CustomerId,
                    CustomerName = x.CustomerName,
                    CustomerAddress = x.CustomerAddress,
                    CustomerCity = x.CustomerCity,
                    CustomerCode = x.CustomerCode,
                    CustomerEmail = x.CustomerEmail,
                    CustomerLatitude = x.CustomerLatitude,
                    CustomerLongitude = x.CustomerLongitude,
                    CustomerPhoneNumber = x.CustomerPhoneNumber,
                    CustomerPhoto = x.CustomerPhoto,
                }).FirstOrDefault();

                if (customer == null)
                    throw new ApplicationException("not found");

                return customer;
            }catch(ApplicationException ex)
            {
                throw ex;
            }catch(Exception ex)
            {
                throw ex;
            }
        }



        public List<CustomerModel> GetCustomers(string keyword, int limit = 10, int offset = 0, string orderColumn = "CreatedDt", string orderType = "desc", DateTime? startDate = null, DateTime? endDate = null, UserAuthenticated user = null)
        {
            try
            {
                repoCustomer.Condition = PredicateBuilder.True<Customer>().And(x => x.IsActive == 1 && x.IsDeleted == 0);
                repoCustomer.Limit = limit;
                repoCustomer.Offset = offset;

                if (!string.IsNullOrEmpty(orderColumn) && !string.IsNullOrEmpty(orderType))
                {
                    repoCustomer.OrderBy = new Trisatech.AspNet.Common.Models.SqlOrderBy { Type = orderType, Column = orderColumn };
                }

                if (!string.IsNullOrEmpty(keyword))
                {
                    repoCustomer.Condition = repoCustomer.Condition.And(x => x.CustomerName.Contains(keyword) || x.CustomerEmail.Contains(keyword));
                }

                if (startDate != null && endDate != null)
                {
                    repoCustomer.Condition = repoCustomer.Condition.And(x => (x.CreatedDt >= startDate.Value && x.CreatedDt <= endDate.Value));
                }

                if (user == null)
                {
                    #region Find based on custom model
                    var dbResult = repoCustomer.Find<CustomerModel>(x => new CustomerModel
                    {
                        CustomerId = x.CustomerId,
                        CustomerName = x.CustomerName,
                        CustomerCode = x.CustomerCode,
                        CustomerPhoneNumber = x.CustomerPhoneNumber,
                        CustomerEmail = x.CustomerEmail,
                        CustomerAddress = x.CustomerAddress,
                        CustomerCity = x.CustomerCity,
                        CustomerLatitude = x.CustomerLatitude,
                        CustomerLongitude = x.CustomerLongitude,
                        CustomerPhoto = x.CustomerPhoto
                    }).ToList();

                    return dbResult;
                } else
                {
                    var result = (
                                  from a in dbContext.Accounts
                                  join b in dbContext.Users on a.AccountId equals b.AccountId
                                  //join e in dbContext.Customers on b.UserId equals e.CreatedBy
                                  join c in dbContext.UserTerritories on b.UserId equals c.UserId
                                  join d in dbContext.UserTerritories on c.TerritoryId equals d.TerritoryId
                                  join e in dbContext.Customers on d.UserId equals e.CreatedBy
                                  where a.AccountId == user.AccountId

                                  select new CustomerModel
                                  {
                                      CustomerId = e.CustomerId,
                                      CustomerName = e.CustomerName,
                                      CustomerCode = e.CustomerCode,
                                      CustomerPhoneNumber = e.CustomerPhoneNumber,
                                      CustomerEmail = e.CustomerEmail,
                                      CustomerAddress = e.CustomerAddress,
                                      CustomerCity = e.CustomerCity,
                                      CustomerLatitude = e.CustomerLatitude,
                                      CustomerLongitude = e.CustomerLongitude,
                                      CustomerPhoto = e.CustomerPhoto
                                  }).ToList();
                    return result;
                }
                #endregion

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public int TotalCustomers(string keyword, int limit = 10, int offset = 0, string orderColumn = "CreatedDt", string orderType = "desc", UserAuthenticated user = null)
        {
            try
            {
                repoCustomer.Condition = PredicateBuilder.True<Customer>().And(x => x.IsActive == 1 && x.IsDeleted == 0);
                repoCustomer.Limit = limit;
                repoCustomer.Offset = offset;

                if (!string.IsNullOrEmpty(orderColumn) && !string.IsNullOrEmpty(orderType))
                {
                    repoCustomer.OrderBy = new Trisatech.AspNet.Common.Models.SqlOrderBy { Type = orderType, Column = orderColumn };
                }

                if (!string.IsNullOrEmpty(keyword))
                {
                    repoCustomer.Condition = repoCustomer.Condition.And(x => x.CustomerName.Contains(keyword) || x.CustomerEmail.Contains(keyword));
                }
                
                if (user == null)
                {
                    var count = repoCustomer.Count();
                    if (count == null)
                        return 0;
                    return (int)count;
                } else
                {
                    var result = (
                                  from a in dbContext.Accounts
                                  join b in dbContext.Users on a.AccountId equals b.AccountId
                                  //join e in dbContext.Customers on b.UserId equals e.CreatedBy
                                  join c in dbContext.UserTerritories on b.UserId equals c.UserId
                                  join d in dbContext.UserTerritories on c.TerritoryId equals d.TerritoryId
                                  join e in dbContext.Customers on d.UserId equals e.CreatedBy
                                  where a.AccountId == user.AccountId

                                  select new CustomerModel
                                  {
                                  }).ToList();
                    return result.Count();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CustomerDetailModel detailCust(string id)
        {
            try
            {
                repoCustDetail.Includes = new string[] { "Customer" };
                repoCustDetail.Condition = PredicateBuilder.True<CustomerDetail>().And(x => x.CustomerId == id );

                var result = repoCustDetail.Find<CustomerDetailModel>(x => new CustomerDetailModel
                {
                    StoreName = x.BrandName,
                    StoreAddress = x.BrandAddress,
                    StoreAge = x.BrandAge,
                    StoreCity = x.BrandCity,
                    StoreDistrict = x.BrandDistrict,
                    StoreLatitude = x.BrandLatitude,
                    StoreLongitude = x.BrandLongitude,
                    StorePhotoBlobId = x.BrandPhotoBlobId,
                    StorePhotoUrl = x.BrandPhotoUrl,
                    StoreStatus = x.BrandStatus,
                    StoreType = x.BrandType,
                    StoreVillage = x.BrandVillage,
                    Note = x.Desc,
                    OwnerName = x.Customer.CustomerName,
                    OwnerAddress = x.Customer.CustomerAddress,
                    OwnerCity = x.Customer.CustomerCity,
                    OwnerDistrict = x.Customer.CustomerDistrict,
                    OwnerPhoneNumber = x.Customer.CustomerPhoneNumber,
                    OwnerVillage = x.Customer.CustomerVillage,
                    PhotoIdCardBlobId = x.Customer.CustomerPhotoIdBlobId,
                    BrandingPhotoBlobId = x.BrandPhotoBlobId,
                    BrandingPhotoUrl = x.BrandPhotoUrl,
                    PhotoNPWPBlobId = x.Customer.CustomerPhotoNPWPBlobId,
                    PhotoNPWPUrl = x.Customer.CustomerPhotoNPWP,
                    PhotoIdCardUrl = x.Customer.CustomerPhotoId,
                    CustomerDetailId = x.CustomerDetailId,
                    CustomerId = x.CustomerId
                }).FirstOrDefault();

                //if (result == null)
                //    Debug.WriteLine("Data Not Found");

                return result;
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CustomerModel> GetOutlets(string keyword, int limit = 10, int offset = 0, string orderColumn = "CreatedDt", string orderType = "desc", DateTime? startDate = null, DateTime? endDate = null, UserAuthenticated user = null)
        {
            try
            {
                var condition = PredicateBuilder.True<Customer>().And(x => x.IsActive == 1 && x.IsDeleted == 0);

                if (!string.IsNullOrEmpty(keyword))
                {
                    condition = condition.And(x => x.CustomerName.Contains(keyword) || x.CustomerEmail.Contains(keyword));
                }

                if (startDate != null && endDate != null)
                {
                    condition = condition.And(x => (x.CreatedDt >= startDate.Value && x.CreatedDt <= endDate.Value));
                }

                if (user == null)
                {
                    #region Find based on custom model

                    IQueryable<Customer> query = (from b in dbContext.CustomerDetails
                                                  join a in dbContext.Customers on b.CustomerId equals a.CustomerId
                                                  select a);

                    var result = query.Where(condition).Select(e => new CustomerModel{
                        CustomerId = e.CustomerId,
                        CustomerName = e.CustomerName,
                        CustomerCode = e.CustomerCode,
                        CustomerPhoneNumber = e.CustomerPhoneNumber,
                        CustomerEmail = e.CustomerEmail,
                        CustomerAddress = e.CustomerAddress,
                        CustomerCity = e.CustomerCity,
                        CustomerLatitude = e.CustomerLatitude,
                        CustomerLongitude = e.CustomerLongitude,
                        CustomerPhoto = e.CustomerPhoto
                    }).Skip(offset).Take(limit).ToList();
                    return result;
                    #endregion
                }
                else
                {
                    #region Find based on custom model
                    
                    IQueryable<Customer> query = (from a in dbContext.Accounts
                                                  join b in dbContext.Users on a.AccountId equals b.AccountId
                                                  //join e in dbContext.Customers on b.UserId equals e.CreatedBy
                                                  join c in dbContext.UserTerritories on b.UserId equals c.UserId
                                                  join d in dbContext.UserTerritories on c.TerritoryId equals d.TerritoryId
                                                  join e in dbContext.Customers on d.UserId equals e.CreatedBy
                                                  where a.AccountId == user.AccountId
                                                  select e);
                    #region testing not dynamic query
                    /*var result = (
                                  from a in dbContext.Accounts
                                  join b in dbContext.Users on a.AccountId equals b.AccountId
                                  //join e in dbContext.Customers on b.UserId equals e.CreatedBy
                                  join c in dbContext.UserTerritories on b.UserId equals c.UserId
                                  join d in dbContext.UserTerritories on c.TerritoryId equals d.TerritoryId
                                  join e in dbContext.Customers on d.UserId equals e.CreatedBy
                                  where a.AccountId == user.AccountId

                                  select new CustomerModel
                                  {
                                      CustomerId = e.CustomerId,
                                      CustomerName = e.CustomerName,
                                      CustomerCode = e.CustomerCode,
                                      CustomerPhoneNumber = e.CustomerPhoneNumber,
                                      CustomerEmail = e.CustomerEmail,
                                      CustomerAddress = e.CustomerAddress,
                                      CustomerCity = e.CustomerCity,
                                      CustomerLatitude = e.CustomerLatitude,
                                      CustomerLongitude = e.CustomerLongitude,
                                      CustomerPhoto = e.CustomerPhoto
                                  }).ToList();*/
                    #endregion
                    var result = query.Where(condition).Select(e => new CustomerModel{
                        CustomerId = e.CustomerId,
                        CustomerName = e.CustomerName,
                        CustomerCode = e.CustomerCode,
                        CustomerPhoneNumber = e.CustomerPhoneNumber,
                        CustomerEmail = e.CustomerEmail,
                        CustomerAddress = e.CustomerAddress,
                        CustomerCity = e.CustomerCity,
                        CustomerLatitude = e.CustomerLatitude,
                        CustomerLongitude = e.CustomerLongitude,
                        CustomerPhoto = e.CustomerPhoto
                    }).Skip(offset).Take(limit).ToList();
                    return result;
                }
            }
                    #endregion

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int TotalOutlet(string keyword, int limit = 10, int offset = 0, string orderColumn = "CreatedDt", string orderType = "desc", UserAuthenticated user = null)
        {
            try
            {
                //repoCustomer.Includes = new string[] { "CustomerDetail" };
                //repoCustomer.Condition = PredicateBuilder.True<Customer>().And(x => x.IsActive == 1 && x.IsDeleted == 0);
                //repoCustomer.Limit = limit;
                //repoCustomer.Offset = offset;

                //if (!string.IsNullOrEmpty(orderColumn) && !string.IsNullOrEmpty(orderType))
                //{
                //    repoCustomer.OrderBy = new Trisatech.AspNet.Common.Models.SqlOrderBy { Type = orderType, Column = orderColumn };
                //}

                //if (!string.IsNullOrEmpty(keyword))
                //{
                //    repoCustomer.Condition = repoCustomer.Condition.And(x => x.CustomerName.Contains(keyword) || x.CustomerEmail.Contains(keyword));
                //}

                //var test = repoCustomer.Find().ToList();

                if (user == null)
                {
                    var result = (
                                  from a in dbContext.Customers
                                  join b in dbContext.CustomerDetails on a.CustomerId equals b.CustomerId
                                  select b.CustomerDetailId).Count();
                    return result;
                }
                else
                {
                    var result = (
                                  from a in dbContext.Accounts
                                  join b in dbContext.Users on a.AccountId equals b.AccountId
                                  //join e in dbContext.Customers on b.UserId equals e.CreatedBy
                                  join c in dbContext.UserTerritories on b.UserId equals c.UserId
                                  join d in dbContext.UserTerritories on c.TerritoryId equals d.TerritoryId
                                  join e in dbContext.Customers on d.UserId equals e.CreatedBy
                                  where a.AccountId == user.AccountId
                                  select e.CustomerId).Count();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void EditOutlet(CustomerDetailModel customerModel, string updatedBy)
        {
            try
            {
                repoCustDetail.Condition = PredicateBuilder.True<CustomerDetail>();
                var customerDetailExist = repoCustDetail.Find().FirstOrDefault();

                if (customerDetailExist != null && customerDetailExist.CustomerId != customerModel.CustomerId)
                    //throw new ApplicationException(customerModel.OwnerName + " already exist.");

                repoCustDetail.Condition = PredicateBuilder.True<CustomerDetail>().And(x => x.CustomerId == customerModel.CustomerId);
                var customer = repoCustDetail.Find().FirstOrDefault();

                if (customer == null)
                    throw new ApplicationException("not found");

                customer.BrandAddress = customerModel.StoreAddress;
                customer.BrandAge = customerModel.StoreAge;
                customer.BrandCity = customerModel.StoreCity;
                customer.BrandDistrict = customerModel.StoreDistrict;
                customer.BrandingName = customerModel.BrandingName;
                customer.BrandLatitude = customerModel.StoreLatitude;
                customer.BrandLongitude = customerModel.StoreLongitude;
                customer.BrandName = customerModel.StoreName;
                customer.BrandingName = customerModel.BrandingName;
                customer.BrandPhotoUrl = customerModel.StorePhotoUrl;
                customer.BrandStatus = customerModel.StoreStatus;
                customer.BrandType = customerModel.StoreType;
                customer.BrandVillage = customerModel.StoreVillage;
                customer.Desc = customerModel.Note;

                //customer.UpdatedBy = updatedBy;
                //customer.UpdatedDt = DateTime.UtcNow;
                //repoCustomer.Condition = PredicateBuilder.True<Customer>().And(x => x.CustomerId == customerModel.CustomerId);
                //var owner = repoCustomer.Find().FirstOrDefault();

                //if (customer == null)
                //    throw new ApplicationException("not found");

                //owner.CustomerAddress = customerModel.OwnerAddress;
                //owner.CustomerCity = customerModel.OwnerCity;
                //owner.CustomerDistrict = customerModel.OwnerDistrict;

                repoCustDetail.Update(customer, true);
                //repoCustomer.Update(owner, true);
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteOutlet(string id, string deletedBy)
        {
            try
            {
                repoCustDetail.Condition = PredicateBuilder.True<CustomerDetail>().And(x => x.CustomerDetailId == Convert.ToInt32(id) );
                var customer = repoCustDetail.Find().FirstOrDefault();

                if (customer == null)
                    throw new ApplicationException("not found");
                //customer.IsDeleted = 1;
                //customer.UpdatedBy = deletedBy;
                //customer.UpdatedDt = DateTime.UtcNow;

                //var resp = repoCustomer.Update(customer, true);
                //if (!string.IsNullOrEmpty(resp))
                //    throw new ApplicationException(resp);
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddCustDetail(CustomerDetailModel customerModel, string createdBy)
        {
            try
            {
                unitOfWork.Strategy().Execute(() =>
                {
                    unitOfWork.BeginTransaction();

                    repoCustDetail.Condition = PredicateBuilder.True<CustomerDetail>().And(x => x.BrandAddress == customerModel.StoreAddress);

                    var outletExist = repoCustDetail.Find().FirstOrDefault();
                    if (outletExist != null)
                        throw new ApplicationException("Outlet already exist.");

                    outletExist = null;

                    CustomerDetail customer = new CustomerDetail();
                    CopyProperty.CopyPropertiesTo(customerModel, customer);

                    customer.BrandAddress = customerModel.StoreAddress;
                    customer.BrandAge = customerModel.StoreAge;
                    customer.BrandCity = customerModel.StoreCity;
                    customer.BrandDistrict = customerModel.StoreDistrict;
                    customer.BrandingName = customerModel.BrandingName;
                    customer.BrandLatitude = customerModel.StoreLatitude;
                    customer.BrandLongitude = customerModel.StoreLongitude;
                    customer.BrandName = customerModel.StoreName;
                    customer.BrandingName = customerModel.BrandingName;
                    customer.BrandPhotoUrl = customerModel.StorePhotoUrl;
                    customer.BrandStatus = customerModel.StoreStatus;
                    customer.BrandType = customerModel.StoreType;
                    customer.BrandVillage = customerModel.StoreVillage;
                    customer.Desc = customerModel.Note;
                    customer.CustomerId = customerModel.CustomerId;

                    repoCustDetail.Insert(customer);

                    unitOfWork.Commit();
                });
 
            } catch (Exception ex)
            {
                unitOfWork.Rollback();
                Debug.Write(ex);
                throw;
            }
        }
        #region Contact
        public void AddContact(ContactModel contactModel, string createdBy)
        {
            try
            {
                Contact contact = new Contact();
                CopyProperty.CopyPropertiesTo(contactModel, contact);
                contact.IsActive = 1;
                contact.IsDeleted = 0;
                contact.CreatedBy = createdBy;
                contact.CreatedDt = DateTime.UtcNow;

                var resp = repoContact.Insert(contact, true);
                if (!string.IsNullOrEmpty(resp))
                    throw new ApplicationException(resp);   
            }
            catch (ApplicationException appEx)
            {
                throw appEx;
            }catch(Exception ex)
            {
                throw ex;
            }
        }
        public void EditContact(ContactModel contactModel, string updatedBy)
        {
            try
            {
                repoContact.Condition = PredicateBuilder.True<Contact>().And(x => x.ContactId == contactModel.ContactId && x.IsDeleted == 0 && x.IsActive == 1);
                var contact = repoContact.Find().FirstOrDefault();
                if (contact == null)
                    throw new ApplicationException("not found");

                contact.ContactName = contactModel.ContactName;
                contact.ContactNumber = contactModel.ContactNumber;
                contact.Email = contactModel.Email;
                contact.SecondaryEmail = contactModel.SecondaryEmail;
                contact.Position = contactModel.Position;
                contact.Remarks = contactModel.Remarks;
                contact.ContactPhoto = contactModel.ContactPhoto;
                contact.UpdatedBy = updatedBy;
                contact.UpdatedDt = DateTime.UtcNow;

                var resp = repoContact.Update(contact, true);
                if (!string.IsNullOrEmpty(resp))
                    throw new ApplicationException(resp);
            }
            catch (ApplicationException appEx)
            {
                throw appEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteContact(string id, string updatedBy)
        {
            try
            {
                repoContact.Condition = PredicateBuilder.True<Contact>().And(x => x.ContactId == id && x.IsDeleted == 0 && x.IsActive == 1);
                var contact = repoContact.Find().FirstOrDefault();
                if (contact == null)
                    throw new ApplicationException("not found");


                contact.IsDeleted = 1;
                contact.UpdatedBy = updatedBy;
                contact.UpdatedDt = DateTime.UtcNow;

                var resp = repoContact.Update(contact, true);
                if (!string.IsNullOrEmpty(resp))
                    throw new ApplicationException(resp);
            }
            catch (ApplicationException appEx)
            {
                throw appEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }       
        public List<ContactModel> GetContact(string keyword = "", int limit = 20, int offset = 0, string orderColumn = "", string orderType = "", IEnumerable<string> territories = null, string userid = "")
        {
            try
            {
                if (!string.IsNullOrEmpty(userid))
                {
                    return (from a in dbContext.Contacts
                     join b in dbContext.Customers on a.CustomerId equals b.CustomerId
                     join c in dbContext.AssignmentDetails on a.ContactId equals c.ContactId
                     where c.UserId == userid
                     select new ContactModel
                     {
                         ContactId = a.ContactId,
                         ContactName = a.ContactName,
                         Email = a.Email,
                         SecondaryEmail = a.SecondaryEmail,
                         ContactPhoto = a.ContactPhoto,
                         Position = a.Position,
                         ContactNumber = a.ContactNumber,
                         Remarks = a.Remarks
                     }
                    ).ToList();
                }
                

                var repoContact = unitOfWork.GetRepository<Contact>();
                repoContact.Includes = new string[] { "Customer" };

                repoContact.Condition = PredicateBuilder.True<Contact>().And(x => x.IsActive == 1 && x.IsDeleted == 0);
                if(limit > 0)
                {
                    repoContact.Limit = limit;
                    repoContact.Offset = offset;
                }

                if (!string.IsNullOrEmpty(orderColumn) && !string.IsNullOrEmpty(orderType))
                {
                    repoContact.OrderBy = new Trisatech.AspNet.Common.Models.SqlOrderBy { Type = orderType, Column = orderColumn };
                }
                else
                {
                    repoContact.OrderBy = new Trisatech.AspNet.Common.Models.SqlOrderBy { Type = "asc", Column = "ContactName" };
                }

                if (!string.IsNullOrEmpty(keyword))
                {
                    repoContact.Condition = repoContact.Condition.And(x => x.ContactName.Contains(keyword) || x.Email.Contains(keyword) ||
                    x.ContactNumber.Contains(keyword) || x.Position.Contains(keyword));
                }

                #region Find based on custom model
                var dbResult = repoContact.Find<ContactModel>(x => new ContactModel
                {
                    ContactId = x.ContactId,
                    ContactName = x.ContactName,
                    Email = x.Email,
                    SecondaryEmail = x.SecondaryEmail,
                    ContactPhoto = x.ContactPhoto,
                    Position = x.Position,
                    ContactNumber = x.ContactNumber,
                    Remarks = x.Remarks
                }).ToList();

                return dbResult;
                #endregion

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ContactModel GetContact(string id)
        {
            try
            {
                var repoContact = unitOfWork.GetRepository<Contact>();
                repoContact.Includes = new string[]{ "Customer" };

                repoContact.Condition = PredicateBuilder.True<Contact>()
                .And(x => x.IsActive == 1 && x.IsDeleted == 0 && x.ContactId == id);
                
                #region Find based on custom model
                var dbResult = repoContact.Find<ContactModel>(x => new ContactModel
                {
                    ContactId = x.ContactId,
                    ContactName = x.ContactName,
                    Email = x.Email,
                    SecondaryEmail = x.SecondaryEmail,
                    ContactPhoto = x.ContactPhoto,
                    Position = x.Position,
                    CustomerCode = x.Customer.CustomerCode,
                    ContactNumber = x.ContactNumber,
                    Remarks = x.Remarks
                }).FirstOrDefault();

                return dbResult;
                #endregion

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int TotalContact(string[] regions, string keyword, int limit = 10, int offset = 0, string orderColumn = "CreatedDt", string orderType = "desc")
        {
            try
            {
                var repoContact = unitOfWork.GetRepository<Contact>();

                repoContact.Condition = PredicateBuilder.True<Contact>().And(x => x.IsActive == 1 && x.IsDeleted == 0);
                repoContact.Limit = limit;
                repoContact.Offset = offset;
                
                if (!string.IsNullOrEmpty(orderColumn) && !string.IsNullOrEmpty(orderType))
                {
                    repoContact.OrderBy = new Trisatech.AspNet.Common.Models.SqlOrderBy { Type = orderType, Column = orderColumn };
                }
                else
                {
                    repoContact.OrderBy = new Trisatech.AspNet.Common.Models.SqlOrderBy { Type = "asc", Column = "ContactName" };
                }

                if (!string.IsNullOrEmpty(keyword))
                {
                    repoContact.Condition = repoContact.Condition.And(x => x.ContactName.Contains(keyword) || x.Email.Contains(keyword) || 
                    x.ContactNumber.Contains(keyword) || x.Position.Contains(keyword));
                }

                var dbResult = repoContact.Count();
                if (dbResult == null)
                    return 0;
                return (int)dbResult;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ContactModel> GetContact(string keyword = "", int limit = 20, int offset = 0, string orderColumn = "CreatedDt", string orderType = "desc", IEnumerable<string> sales = null)
        {
            if (sales == null)
                return new List<ContactModel>();

            var result = (from a in dbContext.AssignmentDetails
                          join b in dbContext.Contacts on a.ContactId equals b.ContactId
                          where sales.Contains(a.UserId)
                          group b by new { b.ContactId, b.ContactName, b.ContactNumber } into newContact 
                          orderby newContact.Key.ContactId descending
                          select new ContactModel
                          {
                              ContactId = newContact.Key.ContactId,
                              ContactName = newContact.Key.ContactName,
                              ContactNumber = newContact.Key.ContactNumber
                          }).Skip(offset).Take(limit).ToList();
            
            return result;
        }

        public List<ContactModel> GetWebContact(string[] regions, string keyword = "", int limit = 20, int offset = 0, string orderColumn = "CreatedDt", string orderType = "desc", IEnumerable<string> territories = null, string userid = "")
        {
            try
            {
                if (!string.IsNullOrEmpty(userid))
                {
                    return (from a in dbContext.Contacts
                            join b in dbContext.Customers on a.CustomerId equals b.CustomerId
                            join c in dbContext.AssignmentDetails on a.ContactId equals c.ContactId
                            where c.UserId == userid
                            select new ContactModel
                            {
                                ContactId = a.ContactId,
                                ContactName = a.ContactName,
                                Email = a.Email,
                                SecondaryEmail = a.SecondaryEmail,
                                ContactPhoto = a.ContactPhoto,
                                Position = a.Position,
                                ContactNumber = a.ContactNumber,
                                Remarks = a.Remarks
                            }
                    ).ToList();
                }
                
                var repoContact = unitOfWork.GetRepository<Contact>();
                repoContact.Includes = new string[] { "Customer" };

                repoContact.Condition = PredicateBuilder.True<Contact>().And(x => x.IsActive == 1 && x.IsDeleted == 0);
                if (limit > 0)
                {
                    repoContact.Limit = limit;
                    repoContact.Offset = offset;
                }

                if (!string.IsNullOrEmpty(orderColumn) && !string.IsNullOrEmpty(orderType))
                {
                    repoContact.OrderBy = new Trisatech.AspNet.Common.Models.SqlOrderBy { Type = orderType, Column = orderColumn };
                }
                else
                {
                    repoContact.OrderBy = new Trisatech.AspNet.Common.Models.SqlOrderBy { Type = "asc", Column = "ContactName" };
                }

                if (!string.IsNullOrEmpty(keyword))
                {
                    repoContact.Condition = repoContact.Condition.And(x => x.ContactName.Contains(keyword) || x.Email.Contains(keyword) ||
                    x.ContactNumber.Contains(keyword) || x.Position.Contains(keyword));
                }

                #region Find based on custom model
                var dbResult = repoContact.Find<ContactModel>(x => new ContactModel
                {
                    ContactId = x.ContactId,
                    ContactName = x.ContactName,
                    Email = x.Email,
                    SecondaryEmail = x.SecondaryEmail,
                    ContactPhoto = x.ContactPhoto,
                    Position = x.Position,
                    ContactNumber = x.ContactNumber,
                    Remarks = x.Remarks
                }).ToList();

                return dbResult;
                #endregion

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ContactModel> GetAllContact()
        {
            return (from a in dbContext.Contacts
                    select new ContactModel
                    {
                        ContactId = a.ContactId,
                        ContactName = a.ContactName,
                        Email = a.Email,
                        SecondaryEmail = a.SecondaryEmail,
                        ContactPhoto = a.ContactPhoto,
                        Position = a.Position,
                        ContactNumber = a.ContactNumber,
                        Remarks = a.Remarks
                    }
                    ).ToList();
        }


        #endregion
    }
}
