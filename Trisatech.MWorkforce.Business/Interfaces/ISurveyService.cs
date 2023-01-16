using Trisatech.MWorkforce.Business.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Business.Interfaces
{
    public interface ISurveyService
    {
        void Answer(AnswerModel answerModel, string createdBy);
        List<AnswerModel> GetAnswers(string id);
        void Add(SurveyModel surveyModel, string createdBy);
        void Edit(SurveyModel surveyModel, string updatedBy);
        SurveyModel Get(string id);
        List<SurveyModel> Get(DateTime now);
        void Delete(string id, string deletedBy);
        int TotalRow(string keyword, int limit = 10, int offset = 0, string orderColumn = "CreatedDt", string orderType = "desc");
        List<SurveyModel> Get(string keyword, int limit = 10, int offset = 0, string orderColumn = "CreatedDt", string orderType = "desc");
    }
}
