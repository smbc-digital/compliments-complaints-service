using compliments_complaints_service.Config;
using compliments_complaints_service.Models;
using Microsoft.Extensions.Options;
using StockportGovUK.NetStandard.Gateways.Models.Verint;

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

            var eventTitle = $"Complaint - {model.ComplaintAbout}";

            var crmCase = new Case
            {
                EventCode = (int)eventCode,
                EventTitle = eventTitle,
                Description = GenerateDescription(model.ComplaintAbout, model.ComplaintAboutDetails),
                Customer = new Customer
                {
                    Forename = model.FirstName,
                    Surname = model.LastName,
                    Email = model.EmailAddress,
                    Telephone = model.PhoneNumber,
                }
            };

            if (!string.IsNullOrEmpty(model.CustomersAddress.PlaceRef))
            {
                crmCase.Customer.Address = new Address
                {
                    Postcode = model.CustomersAddress.Postcode,
                    Reference = model.CustomersAddress.PlaceRef,
                    UPRN = model.CustomersAddress.PlaceRef,
                    Description = model.CustomersAddress.ToString()
                };
            }
            else
            {
                crmCase.Customer.Address = new Address
                {
                    AddressLine1 = model.CustomersAddress.AddressLine1,
                    AddressLine2 = model.CustomersAddress.AddressLine2,
                    AddressLine3 = model.CustomersAddress.Town,
                    Postcode = model.CustomersAddress.Postcode,
                    Description = model.CustomersAddress.ToString()
                };
            }


            return crmCase;
        }


        public static string GenerateDescription(string complaintAbout, string complaintAboutDetails)
        {
            return $"{complaintAboutDetails}";
        }
    }
}
