﻿using StockportGovUK.NetStandard.Models.Addresses;
using System.ComponentModel.DataAnnotations;

namespace compliments_complaints_service.Models
{
    public class ComplaintDetailsFormBuilder
    {
        public int? EventCode { get; set; }

        [Required]
        public string CouncilDepartment { get; set; }

        public string CouncilDepartmentSub { get; set; }

        public string RevsBensDept { get; set; }

        public string EnvironmentDept { get; set; }

        public string PlanningDept { get; set; }

        [Required]
        public string ComplaintAbout { get; set; }

        [Required]
        public string ComplaintAboutDetails { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string EmailAddress { get; set; }

        [Required]
        public Address CustomersAddress { get; set; }
    }
}
