using System.Collections.Generic;
using compliments_complaints_service.Controllers.Models;
using compliments_complaints_service.Models;

namespace compliments_complaints_service.Mappers
{
    public static class ComplaintModelMapper
    {
        public static ComplaintDetailsFormBuilder MapComplaint(List<Answer> data)
        {
            var model = new ComplaintDetailsFormBuilder();

            foreach (var answer in data)
            {
                var propertyName = $"{answer.QuestionId[0].ToString().ToUpper()}{answer.QuestionId.Substring(1)}";

                var property = model?
                    .GetType()?
                    .GetProperty(propertyName);

                property?.SetValue(model, answer.Response, null);

                if (answer.QuestionId.Equals("customers-address-address"))
                {
                    model.AddressRef = answer.Response;
                }

                if (answer.QuestionId.Equals("customers-address-address-description"))
                {
                    model.SelectedAddress = answer.Response;
                }

                if (answer.QuestionId.Equals("customers-address-AddressManualAddressLine1"))
                {
                    model.AddressLine1 = answer.Response;
                }

                if (answer.QuestionId.Equals("customers-address-AddressManualAddressLine2"))
                {
                    model.AddressLine2 = answer.Response ?? string.Empty;
                }

                if (answer.QuestionId.Equals("customers-address-AddressManualAddressTown"))
                {
                    model.Town = answer.Response;
                }

                if (answer.QuestionId.Equals("customers-address-AddressManualAddressPostcode"))
                {
                    model.Postcode = answer.Response;
                }
            }

            model.CouncilDepartmentSub = CouncilDepartmentSubMapper.SetComplaintCouncilDepartmentSub(model.RevsBensDept, model.EnvironmentDept, model.PlanningDept);

            return model;
        }
    }
}
