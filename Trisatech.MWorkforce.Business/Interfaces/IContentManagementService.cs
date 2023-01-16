using Trisatech.MWorkforce.Business.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Trisatech.MWorkforce.Business.Interfaces
{
    public interface IContentManagementService
    {
        #region News
        List<NewsModel> Get(string keyword = "", int limit = 20, int offset = 0, string orderColumn = "CreatedDt", string orderType = "desc", bool isPublish = false);
        void Add(NewsModel model, string createdBy, bool addAsContact = false);
        void Edit(NewsModel model, string updatedBy);
        void Delete(string id, string deletedBy);
        NewsModel Get(string id);
        int TotalNews(string keyword, int limit = 10, int offset = 0, string orderColumn = "CreatedDt", string orderType = "desc");
        #endregion
    }
}
