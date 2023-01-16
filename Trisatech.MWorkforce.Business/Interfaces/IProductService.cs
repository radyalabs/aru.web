using Trisatech.MWorkforce.Business.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Business.Interfaces
{
    public interface IProductService
    {
        ProductModel Detail(string id);
        List<ProductModel> Get(string keywords = "", int limit = 10, int offset = 0, string orderByColumn = "CreatedDt", string orderType = "desc");
        void Add(ProductModel model, string createdBy);
        void Edit(ProductModel model, string updatedBy);
        void Delete(string id, string deletedBy);
        Task Order(List<OrderModel> model, string createdBy);
        int TotalRow(string search = "", int length = 10, int start = 0, string v = "CreatedDt", string orderType = "desc");
    }
}
