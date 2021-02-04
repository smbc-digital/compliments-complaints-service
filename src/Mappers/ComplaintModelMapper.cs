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


            var address = new Address();
            address.AddressLine1 = model.CustomersAddress.AddressLine1;
            address.AddressLine2 = model.CustomersAddress.AddressLine2;
            address.AddressLine3 = model.CustomersAddress.Town;
            address.Postcode = model.CustomersAddress.Postcode;
            address.Reference = model.CustomersAddress.PlaceRef;
            address.Description = model.CustomersAddress.ToString();
            


            var customer = new Customer();
            customer.Forename = model.FirstName;
            customer.Surname = model.LastName;
            customer.Email = model.EmailAddress;
            customer.Telephone = model.PhoneNumber;
            customer.Address = address;
            

            var eventTitle = string.IsNullOrEmpty(model.CouncilDepartmentSub) ? $"Complaint - {model.CouncilDepartment}" : $"Complaint - {model.CouncilDepartment} - {model.CouncilDepartmentSub}";


            var crmCase = new Case
            {
                EventCode = (int)eventCode,
                EventTitle = eventTitle,
                Description = GenerateDescription(model.ComplaintAbout, model.ComplaintAboutDetails),
                Customer =  customer
            };

            return crmCase;
        }


        public static string GenerateDescription(string complaintAbout, string complaintAboutDetails)
        {
            return $"{complaintAbout}\n\n{complaintAboutDetails}";
        }
    }
}
