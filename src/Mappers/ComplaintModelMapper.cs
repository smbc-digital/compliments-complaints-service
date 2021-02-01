using compliments_complaints_service.Models;
using compliments_complaints_service.Config;
using Microsoft.Extensions.Options;
using System.Linq;
using StockportGovUK.NetStandard.Models.Verint;

namespace compliments_complaints_service.Mappers
{
    public static class ComplaintModelMapper
    {
        public static Case ToCrmCase(ComplaintDetailsFormBuilder model, IOptions<ComplaintsListConfiguration> complaintsConfig)
        {

            model.CouncilDepartmentSub = CouncilDepartmentSubMapper.SetComplaintCouncilDepartmentSub(model.RevsBensDept, model.EnvironmentDept, model.PlanningDept);
            var events = complaintsConfig.Value.ComplaintsConfigurations;

            var eventCode = string.IsNullOrEmpty(model.CouncilDepartmentSub)
                ? events.FirstOrDefault(_ => _.EventName == model.CouncilDepartment)?.EventCode ?? events.FirstOrDefault(_ => _.EventName == "none")?.EventCode
                : events.FirstOrDefault(_ => _.EventName == model.CouncilDepartmentSub)?.EventCode ?? events.FirstOrDefault(_ => _.EventName == "none")?.EventCode;

            var crmCase = new Case
            {
                EventCode = (int)eventCode,
                EventTitle = string.IsNullOrEmpty(model.CouncilDepartmentSub) ? $"Complaint - {model.CouncilDepartment}" : $"Complaint - {model.CouncilDepartment} - {model.CouncilDepartmentSub}",
                Description = GenerateDescription(model.ComplaintAbout, model.ComplaintAboutDetails),
                Customer = new Customer
                {
                    Forename = model.FirstName,
                    Surname = model.LastName,
                    Email = model.EmailAddress,
                    Telephone = model.PhoneNumber,
                    Address = new Address
                    {
                        AddressLine1 = model.CustomersAddress.AddressLine1,
                        AddressLine2 = model.CustomersAddress.AddressLine2,
                        AddressLine3 = model.CustomersAddress.Town,
                        Postcode = model.CustomersAddress.Postcode,
                        Reference = model.CustomersAddress.PlaceRef,
                        Description = model.CustomersAddress.ToString()
                    }
                }
            };

            return crmCase;
        }


        public static string GenerateDescription(string complaintAbout, string complaintAboutDetails)
        {
            return $"{complaintAbout}\n\n{complaintAboutDetails}";
        }
    }
}
