using Trisatech.MWorkforce.Business.Entities;
using Trisatech.MWorkforce.Business.Interfaces;
using Trisatech.MWorkforce.Domain;
using Trisatech.MWorkforce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Trisatech.AspNet.Common.Helpers;
using Trisatech.AspNet.Common.Interfaces;
using Trisatech.AspNet.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trisatech.MWorkforce.Business.Services
{
    public class ContentManagementService : IContentManagementService
    {
            private IUnitOfWork unitOfWork;
            private IRepository<News> repoNews;
            private MobileForceContext dbContext;

            public ContentManagementService(MobileForceContext dbContext)
            {
                this.dbContext = dbContext;
                unitOfWork = new UnitOfWork<DbContext>(dbContext);
                repoNews = unitOfWork.GetRepository<News>();
            }

        #region News
        public void Add(NewsModel model, string createdBy, bool addAsContact = false)
        {
            try
            {
                News news = new News();
                CopyProperty.CopyPropertiesTo(model, news);

                news.CreatedBy = createdBy;
                news.CreatedDt = DateTime.UtcNow;
                news.IsActive = 1;
                news.IsDeleted = 0;

                string resp = repoNews.Insert(news, true);
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

        public void Delete(string id, string deletedBy)
        {
            try
            {
                repoNews.Condition = PredicateBuilder.True<News>().And(x => x.NewsId == id && x.IsDeleted == 0 && x.IsActive == 1);

                var news = repoNews.Find().FirstOrDefault();
                if (news == null)
                    throw new ApplicationException("News not found");

                news.IsDeleted = 1;
                news.UpdatedBy = deletedBy;
                news.UpdatedDt = DateTime.UtcNow;

                string resp = repoNews.Update(news, true);
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

        public void Edit(NewsModel model, string updatedBy)
        {
            try
            {
                repoNews.Condition = PredicateBuilder.True<News>().And(x => x.NewsId == model.NewsId && x.IsDeleted == 0 && x.IsActive == 1);

                var news = repoNews.Find().FirstOrDefault();
                if (news == null)
                    throw new ApplicationException("News not found");

                news.Title = model.Title;
                news.Desc = model.Desc;
                news.Content = model.Content;
                news.IsPublish = model.IsPublish;
                news.PublishedDate = model.PublishedDate;
                news.UpdatedBy = updatedBy;
                news.UpdatedDt = DateTime.UtcNow;

                string resp = repoNews.Update(news, true);
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

        public List<NewsModel> Get(string keyword = "", int limit = 20, int offset = 0, string orderColumn = "CreatedDt", string orderType = "desc", bool isPublish = false)
        {
            try
            {
                repoNews.Condition = PredicateBuilder.True<News>().And(x => x.IsDeleted == 0 && x.IsActive == 1);

                if (!string.IsNullOrEmpty(keyword))
                {
                    repoNews.Condition.And(x => (x.Title != null && x.Title.Contains(keyword)) || (x.Desc != null && x.Desc.Contains(keyword)) || (x.Content != null && x.Content.Contains(keyword)));
                }

                if (isPublish)
                {
                    repoNews.Condition.And(x => x.IsPublish == true);
                }
                repoNews.Limit = limit;

                repoNews.Offset = offset;
                repoNews.OrderBy = new Trisatech.AspNet.Common.Models.SqlOrderBy
                {
                    Column = orderColumn,
                    Type = orderType
                };

                #region Find based on custom model
                var dbResult = repoNews.Find<NewsModel>(x => new NewsModel
                {
                    NewsId = x.NewsId,
                    Title = x.Title,
                    Desc = x.Content,
                    Content = x.Desc,
                    IsPublish = x.IsPublish,
                    PublishedDate = x.PublishedDate
                }).ToList();

                if (dbResult == null)
                    throw new ApplicationException("News not found");

                return dbResult;
                #endregion
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

        public NewsModel Get(string id)
        {
            try
            {
                repoNews.Condition = PredicateBuilder.True<News>().And(x => x.NewsId == id && x.IsDeleted == 0 && x.IsActive == 1);

                var news = repoNews.Find().FirstOrDefault();
                if (news == null)
                    throw new ApplicationException("News not found");

                NewsModel model = new NewsModel();
                CopyProperty.CopyPropertiesTo(news, model);

                return model;
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

        public int TotalNews(string keyword, int limit = 10, int offset = 0, string orderColumn = "CreatedDt", string orderType = "desc")
        {
            try
            {
                repoNews.Condition = PredicateBuilder.True<News>().And(x => x.IsDeleted == 0 && x.IsActive == 1);

                if (!string.IsNullOrEmpty(keyword))
                {
                    repoNews.Condition.And(x => (x.Title != null && x.Title.Contains(keyword)) || (x.Desc != null && x.Desc.Contains(keyword)) || (x.Content != null && x.Content.Contains(keyword)));
                }

                repoNews.Limit = limit;
                repoNews.Offset = offset;
                repoNews.OrderBy = new Trisatech.AspNet.Common.Models.SqlOrderBy
                {
                    Column = orderColumn,
                    Type = orderType
                };
                
                return (int)repoNews.Count();
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
        #endregion


    }
}
