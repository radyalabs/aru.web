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
using System.Threading.Tasks;
using Trisatech.AspNet.Common.Helpers;

namespace Trisatech.MWorkforce.Business.Services
{
    public class SurveyService : ISurveyService
    {
        private IUnitOfWork unitOfWork;
        private IRepository<Survey> repoSurvey;
        private MobileForceContext dbContext;
        public SurveyService(MobileForceContext context)
        {
            dbContext = context;
            unitOfWork = new UnitOfWork<MobileForceContext>(context);
            repoSurvey = this.unitOfWork.GetRepository<Survey>();
        }

        public void Add(SurveyModel surveyModel, string createdBy)
        {
            try
            {
                Survey survey = new Survey();
                CopyProperty.CopyPropertiesTo(surveyModel, survey);
                
                survey.CreatedBy = createdBy;
                survey.CreatedDt = DateTime.UtcNow;
                survey.IsActive = 1;
                survey.IsDeleted = 0;

                string resp = repoSurvey.Insert(survey, true);
                if (!string.IsNullOrEmpty(resp))
                    throw new ApplicationException(resp);
            }catch(ApplicationException appEx)
            {
                throw appEx;
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public void Answer(AnswerModel answerModel, string createdBy)
        {
            try
            {
                var repoAnswer = unitOfWork.GetRepository<Answer>();

                Answer answer = new Answer();
                
                answer.AssignmentCode = answerModel.AssignmentCode;
                answer.AssignmentId = answerModel.AssignmentId;
                answer.UserId = answerModel.UserId;
                answer.SurveyId = answerModel.SurveyId;
                answer.CreatedBy = createdBy;
                answer.CreatedDt = answerModel.AnswerDate;
                answer.IsActive = 1;
                answer.IsDeleted = 0;

                string resp = repoAnswer.Insert(answer, true);
                if (!string.IsNullOrEmpty(resp))
                    throw new ApplicationException(resp);

            }catch(ApplicationException appEx)
            {
                throw appEx;
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(string id, string deletedBy)
        {
            try
            {
                repoSurvey.Condition = PredicateBuilder.True<Survey>().And(x => x.SurveyId == id && x.IsDeleted == 0 && x.IsActive == 1);

                var survey = repoSurvey.Find().FirstOrDefault();
                if (survey == null)
                    throw new ApplicationException("survey not found");

                survey.IsDeleted = 1;
                survey.UpdatedBy = deletedBy;
                survey.UpdatedDt = DateTime.UtcNow;

                string resp = repoSurvey.Update(survey, true);
                if (!string.IsNullOrEmpty(resp))
                    throw new ApplicationException(resp);
            }catch(ApplicationException appEx)
            {
                throw appEx;
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public void Edit(SurveyModel surveyModel, string updatedBy)
        {
            try
            {
                repoSurvey.Condition = PredicateBuilder.True<Survey>().And(x => x.SurveyId == surveyModel.SurveyId && x.IsDeleted == 0 && x.IsActive == 1);

                var survey = repoSurvey.Find().FirstOrDefault();
                if (survey == null)
                    throw new ApplicationException("survey not found");

                survey.SurveyName = surveyModel.SurveyName;
                survey.SurveyLink = surveyModel.SurveyLink;
                survey.StartDate = surveyModel.StartDate;
                survey.EndDate = surveyModel.EndDate;
                survey.UpdatedBy = updatedBy;
                survey.UpdatedDt = DateTime.UtcNow;

                string resp = repoSurvey.Update(survey, true);
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
        public List<SurveyModel> Get(DateTime now)
        {
            try
            {
                try
                {
                    repoSurvey.Condition = PredicateBuilder.True<Survey>().And(x => (now >= x.StartDate && now <= x.EndDate) && x.IsDeleted == 0 && x.IsActive == 1);

                    #region Find based on custom model
                    var dbResult = repoSurvey.Find<SurveyModel>(x => new SurveyModel
                    {
                        SurveyId = x.SurveyId,
                        SurveyName = x.SurveyName,
                        SurveyLink = x.SurveyLink,
                        StartDate = x.StartDate,
                        EndDate = x.EndDate
                    }).ToList();

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
            catch(Exception ex)
            {
                return null;
            }
        }
        public SurveyModel Get(string id)
        {
            try
            {
                repoSurvey.Condition = PredicateBuilder.True<Survey>().And(x => x.IsDeleted == 0 && x.IsActive == 1 && x.SurveyId == id);

                #region Find based on custom model
                var dbResult = repoSurvey.Find<SurveyModel>(x => new SurveyModel
                {
                    SurveyId = x.SurveyId,
                    SurveyName = x.SurveyName,
                    SurveyLink = x.SurveyLink,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate
                }).FirstOrDefault();

                if (dbResult == null)
                    throw new ApplicationException("Survey not found");

                return dbResult;
                #endregion
            }catch(ApplicationException appEx)
            {
                throw appEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SurveyModel> Get(string keyword, int limit = 10, int offset = 0, string orderColumn = "CreatedDt", string orderType = "desc")
        {
            try
            {
                repoSurvey.Condition = PredicateBuilder.True<Survey>().And(x => x.IsDeleted == 0 && x.IsActive == 1);

                if (!string.IsNullOrEmpty(keyword))
                {
                    repoSurvey.Condition.And(x => (x.SurveyName != null && x.SurveyName.Contains(keyword)) || (x.SurveyLink != null && x.SurveyLink.Contains(keyword)));
                }

                repoSurvey.Limit = limit;
                repoSurvey.Offset = offset;
                repoSurvey.OrderBy = new Trisatech.AspNet.Common.Models.SqlOrderBy
                {
                    Column = orderColumn,
                    Type = orderType
                };

                #region Find based on custom model
                var dbResult = repoSurvey.Find<SurveyModel>(x => new SurveyModel
                {
                    SurveyId = x.SurveyId,
                    SurveyName = x.SurveyName,
                    SurveyLink = x.SurveyLink,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate
                }).ToList();

                if (dbResult == null)
                    throw new ApplicationException("Survey not found");

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

        public List<AnswerModel> GetAnswers(string id)
        {
            try
            {
                var repoAnswer = unitOfWork.GetRepository<Answer>();
                repoAnswer.Condition = PredicateBuilder.True<Answer>().And(x => x.IsDeleted == 0 && x.IsActive == 1 && x.SurveyId == id);
                repoAnswer.Includes = new string[] { "Survey", "User", "Assigment", "User.Account", "User.Account.Role" };
                repoSurvey.Condition = PredicateBuilder.True<Survey>().And(x => x.IsDeleted == 0 && x.IsActive == 1);
                #region Find based on custom model
                var dbResult = repoAnswer.Find<AnswerModel>(x => new AnswerModel
                {
                    AnswerId = x.AnswerId,
                    SurveyId = x.SurveyId,
                    SurveyName = x.Survey.SurveyName,
                    SurveyLink = x.Survey.SurveyLink,
                    StartDate = x.Survey.StartDate,
                    EndDate = x.Survey.EndDate,
                    UserId = x.UserId,
                    AgentName = x.User.UserName,
                    AgentRole = x.User.Account.RoleCode,
                    AgentRoleName = x.User.Account.Role.RoleName,
                    AnswerDate = x.CreatedDt,
                    AssignmentTitle = x.Assignment.AssignmentName,
                    AssignmentCode = x.Assignment.AssignmentCode,
                    AssignmentId = x.AssignmentId
                    
                }).ToList();

                if (dbResult == null)
                    throw new ApplicationException("Survey not found");

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

        public int TotalRow(string keyword, int limit = 10, int offset = 0, string orderColumn = "CreatedDt", string orderType = "desc")
        {
            try
            {
                repoSurvey.Condition = PredicateBuilder.True<Survey>().And(x => x.IsDeleted == 0 && x.IsActive == 1);

                if (!string.IsNullOrEmpty(keyword))
                {
                    repoSurvey.Condition.And(x => (x.SurveyName != null && x.SurveyName.Contains(keyword)) || (x.SurveyLink != null && x.SurveyLink.Contains(keyword)));
                }

                repoSurvey.Limit = limit;
                repoSurvey.Offset = offset;
                repoSurvey.OrderBy = new Trisatech.AspNet.Common.Models.SqlOrderBy
                {
                    Column = orderColumn,
                    Type = orderType
                };
                
                long? totalRow = repoSurvey.Count();

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
