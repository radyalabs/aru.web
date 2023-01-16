using Trisatech.MWorkforce.Business.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Trisatech.MWorkforce.Business.Interfaces
{
    public interface ICustomerService
    {
        List<CustomerModel> Get(string keyword = "", int limit = 20, int offset = 0);
        void Add(CustomerModel customerModel, string createdBy, bool addAsContact = false);
        void AddCustomer(CustomerDetailModel model, string createdBy);
        void Edit(CustomerModel customerModel, string updatedBy);
        void Delete(string id, string deletedBy);
        CustomerModel Get(string id);
        List<CustomerModel> GetOutlets(string keyword, int limit = 10, int offset = 0, string orderColumn = "CreatedDt", string orderType = "desc", DateTime? startDate = null, DateTime? endDate = null, UserAuthenticated user = null);
        int TotalOutlet(string keyword, int limit = 10, int offset = 0, string orderColumn = "CreatedDt", string orderType = "desc", UserAuthenticated user = null);
        List<CustomerModel> GetCustomers(string keyword, int limit = 10, int offset = 0, string orderColumn = "CreatedDt", string orderType = "desc", DateTime? startDate = null, DateTime? endDate = null, UserAuthenticated user = null);
        int TotalCustomers(string keyword, int limit = 10, int offset = 0, string orderColumn = "CreatedDt", string orderType = "desc", UserAuthenticated user = null);
        CustomerDetailModel detailCust(string id);
        void EditOutlet(CustomerDetailModel model, string updatedBy);
        void DeleteOutlet(string id, string deletedBy);
        void AddCustDetail(CustomerDetailModel outletModel, string createdBy);

        ContactModel GetContact(string id);
        List<ContactModel> GetAllContact();

        void AddContact(ContactModel contactModel, string createdBy);
        void EditContact(ContactModel contactModel, string updatedBy);
        void DeleteContact(string id, string updatedBy);

        List<ContactModel> GetContact(string keyword = "", int limit = 20, int offset = 0, string orderColumn = "CreatedDt", string orderType = "desc", IEnumerable<string> territories = null, string userid = "");
        List<ContactModel> GetWebContact(string[] regions, string keyword = "", int limit = 20, int offset = 0, string orderColumn = "CreatedDt", string orderType = "desc", IEnumerable<string> territories = null, string userid = "");
        int TotalContact(string[] regions, string keyword, int limit = 10, int offset = 0, string orderColumn = "CreatedDt", string orderType = "desc");

        List<ContactModel> GetContact(string keyword = "", int limit = 20, int offset = 0, string orderColumn = "CreatedDt", string orderType = "desc", IEnumerable<string> sales = null);
    }
}
