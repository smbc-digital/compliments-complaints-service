using System.Collections.Generic;
using compliments_complaints_service.Controllers.Models;
using compliments_complaints_service.Models;

namespace compliments_complaints_service.Mappers
{
    public static class FeedbackAndComplimentModelMapper
    {
        public static FeedbackAndComplimentDetailsFormBuilder MapAnswers(List<Answer> data)
        {
            var model = new FeedbackAndComplimentDetailsFormBuilder();

            foreach (var answer in data)
            {
                var propertyName = $"{answer.QuestionId[0].ToString().ToUpper()}{answer.QuestionId.Substring(1)}";

                var property = model?
                    .GetType()?
                    .GetProperty(propertyName);

                property?.SetValue(model, answer.Response, null);

            }

            model.CouncilDepartmentSub = CouncilDepartmentSubMapper.SetComplaintCouncilDepartmentSub(model.RevsBensDept, model.EnvironmentDept, model.PlanningDept);

            return model;
        }
    }
}
