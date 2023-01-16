using Trisatech.MWorkforce.Business.Entities;
using Trisatech.MWorkforce.Business.Interfaces;
using Trisatech.MWorkforce.Domain;
using Trisatech.MWorkforce.Domain.Entities;
using Trisatech.AspNet.Common.Interfaces;
using Trisatech.AspNet.Common.Repository;
using Trisatech.AspNet.Common.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Trisatech.MWorkforce.Business.Services
{
    public class ProductService : IProductService
    {
        private IUnitOfWork unitOfWork;
        private IRepository<Product> repoProduct;
        private MobileForceContext dbContext;
        public ProductService(MobileForceContext context)
        {
            dbContext = context;
            unitOfWork = new UnitOfWork<MobileForceContext>(context);
            repoProduct = this.unitOfWork.GetRepository<Product>();
        }

        public void Add(ProductModel model, string createdBy)
        {
            try
            {
                Product product = new Product();
                CopyProperty.CopyPropertiesTo(model, product);
                product.CreatedBy = createdBy;
                product.ProductModel = model.ProductType;
                product.CreatedDt = DateTime.UtcNow;
                product.IsActive = 1;
                product.IsDeleted = 0;

                string resp = repoProduct.Insert(product, true);
                if (!string.IsNullOrEmpty(resp))
                    throw new ApplicationException(resp);

            }catch(DbUpdateException sqlEx)
            {
                throw new Exception("database connection error, please try again.");
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
                repoProduct.Condition = PredicateBuilder.True<Product>().And(x => x.IsDeleted == 0 && x.IsActive == 1 && x.ProductId == id);

                var result = repoProduct.Find().FirstOrDefault();
                if (result == null)
                    throw new ApplicationException("product not found");

                result.IsDeleted = 1;
                result.UpdatedBy = deletedBy;
                result.UpdatedDt = DateTime.UtcNow;

                string resp = repoProduct.Update(result, true);
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

        public ProductModel Detail(string id)
        {
            try
            {
                repoProduct.Condition = PredicateBuilder.True<Product>().And(x => x.IsDeleted == 0 && x.IsActive == 1 && x.ProductId == id);
                
                var result = repoProduct.Find<ProductModel>(x => new ProductModel
                {
                    ProductId = x.ProductId,
                    ProductCode = x.ProductCode,
                    ProductImage = x.ProductImage,
                    ProductName = x.ProductName,
                    ProductPrice = x.ProductPrice,
                    ProductType = x.ProductModel
                }).FirstOrDefault();

                return result;
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

        public void Edit(ProductModel model, string updatedBy)
        {
            try
            {
                repoProduct.Condition = PredicateBuilder.True<Product>().And(x => x.IsDeleted == 0 && x.IsActive == 1 && x.ProductId == model.ProductId);

                var result = repoProduct.Find().FirstOrDefault();
                if (result == null)
                    throw new ApplicationException("product not found");

                result.ProductCode = model.ProductCode;
                result.ProductName = model.ProductName;
                result.ProductModel = model.ProductType;
                result.ProductPrice = model.ProductPrice;
                result.ProductImage = model.ProductImage;
                result.UpdatedBy = updatedBy;
                result.UpdatedDt = DateTime.UtcNow;

                string resp = repoProduct.Update(result, true);
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

        public List<ProductModel> Get(string keywords = "", int limit = 10, int offset = 0, string orderByColumn = "CreatedDt", string orderType = "desc")
        {
            try
            {
                repoProduct.Condition = PredicateBuilder.True<Product>().And(x => x.IsDeleted == 0 && x.IsActive == 1);

                if (!string.IsNullOrEmpty(keywords))
                {
                    repoProduct.Condition.And(x => (x.ProductCode != null && x.ProductCode.Contains(keywords))
                    || (x.ProductName != null && x.ProductName.Contains(keywords))
                    || (x.ProductModel != null && x.ProductModel.Contains(keywords)));
                }

                repoProduct.Limit = limit;
                repoProduct.Offset = offset;
                repoProduct.OrderBy = new Trisatech.AspNet.Common.Models.SqlOrderBy
                {
                    Column = orderByColumn,
                    Type = orderType
                };

                var result = repoProduct.Find<ProductModel>(x => new ProductModel
                {
                    ProductId = x.ProductId,
                    ProductCode = x.ProductCode,
                    ProductImage = x.ProductImage,
                    ProductName = x.ProductName,
                    ProductPrice = x.ProductPrice,
                    ProductType = x.ProductModel
                }).ToList();

                return result;
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

        public async Task Order(List<OrderModel> model, string createdBy)
        {
            try
            {
                await unitOfWork.Strategy().ExecuteAsync(async () =>
                {
                    unitOfWork.BeginTransaction();

                    await Task.Run(() =>
                    {
                        var repoOrder = unitOfWork.GetRepository<Order>();
                        DateTime utcNow = DateTime.UtcNow;

                        foreach (var item in model)
                        {
                            Order order = new Order();
                            CopyProperty.CopyPropertiesTo(item, order);
                            order.CreatedBy = createdBy;
                            order.CreatedDt = utcNow;
                            order.IsActive = 1;
                            order.IsDeleted = 0;

                            repoOrder.Insert(order);
                        }
                    });

                    unitOfWork.Commit();
                });

            }catch(DbUpdateException sqlEx)
            {
                unitOfWork.Rollback();
                throw new Exception("connection data error, please try again.");
            }catch(ApplicationException appEx)
            {
                unitOfWork.Rollback();
                throw appEx;
            }catch(Exception ex)
            {
                unitOfWork.Rollback();
                throw ex;
            }
        }

        public int TotalRow(string search = "", int length = 10, int start = 0, string v = "CreatedDt", string orderType = "desc")
        {
            try
            {
                repoProduct.Condition = PredicateBuilder.True<Product>().And(x => x.IsDeleted == 0 && x.IsActive == 1);

                if (!string.IsNullOrEmpty(search))
                {
                    repoProduct.Condition.And(x => (x.ProductCode != null && x.ProductCode.Contains(search)) 
                    || (x.ProductName != null && x.ProductName.Contains(search)) 
                    || (x.ProductModel != null && x.ProductModel.Contains(search)));
                }

                repoProduct.Limit = length;
                repoProduct.Offset = start;
                repoProduct.OrderBy = new Trisatech.AspNet.Common.Models.SqlOrderBy
                {
                    Column = v,
                    Type = orderType
                };

                long? totalRow = repoProduct.Count();

                return (int)totalRow;
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
    }
}
