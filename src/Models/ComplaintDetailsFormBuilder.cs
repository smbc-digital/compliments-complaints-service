using StockportGovUK.NetStandard.Models.Addresses;
using System.ComponentModel.DataAnnotations;

namespace compliments_complaints_service.Models
{
    public class ComplaintDetailsFormBuilder
    {
        public int? EventCode { get; set; }

        public string CouncilDepartment { get; set; }

        public string CouncilDepartmentSub { get; set; }

        public string RevsBensDept { get; set; }

        public string EnvironmentDept { get; set; }

        public string PlanningDept { get; set; }
        
        public string ComplaintAbout { get; set; }

        public string ComplaintAboutDetails { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string EmailAddress { get; set; }

        public Address CustomersAddress { get; set; }
    }
}
