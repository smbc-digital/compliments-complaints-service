﻿using compliments_complaints_service.Config;
using compliments_complaints_service.Models;
using Microsoft.Extensions.Options;
using StockportGovUK.NetStandard.Gateways.Models.Verint;

namespace compliments_complaints_service.Mappers
{
    public static class FeedBackAndComplimentsodelMapper
    {


        public static Case FeedbackToCrmCase(FeedbackAndComplimentDetailsFormBuilder model, IOptions<FeedbackListConfiguration> _feedbackConfig)
        {

            model.CouncilDepartmentSub = CouncilDepartmentSubMapper.SetComplaintCouncilDepartmentSub(model.RevsBensDept, model.EnvironmentDept, model.PlanningDept);
            var events = _feedbackConfig.Value.FeedbackConfigurations;

            var eventCode = string.IsNullOrEmpty(model.CouncilDepartmentSub)
                ? events.FirstOrDefault(_ => _.EventName == model.CouncilDepartment)?.EventCode ?? events.FirstOrDefault(_ => _.EventName == "none")?.EventCode
                : events.FirstOrDefault(_ => _.EventName == model.CouncilDepartmentSub)?.EventCode ?? events.FirstOrDefault(_ => _.EventName == "none")?.EventCode;

            string name = string.IsNullOrEmpty(model.Name) ? "Not provided" : model.Name;
            string email = string.IsNullOrEmpty(model.EmailAddress) ? "Not provided" : model.EmailAddress;

            var crmCase = new Case
            {
                EventCode = (int)eventCode,
                EventTitle = $"Feedback",
                Description = $"Name: {name} \nEmail: {email}\n\nFeedback: {model.Description}"
            };

            return crmCase;
        }

        public static Case ComplimentToCrmCase(FeedbackAndComplimentDetailsFormBuilder model, IOptions<ComplimentsListConfiguration> _complimentConfig)
        {

            model.CouncilDepartmentSub = CouncilDepartmentSubMapper.SetComplaintCouncilDepartmentSub(model.RevsBensDept, model.EnvironmentDept, model.PlanningDept);
            var events = _complimentConfig.Value.ComplimentsConfigurations;

            var eventCode = string.IsNullOrEmpty(model.CouncilDepartmentSub)
                ? events.FirstOrDefault(_ => _.EventName == model.CouncilDepartment)?.EventCode ?? events.FirstOrDefault(_ => _.EventName == "none")?.EventCode
                : events.FirstOrDefault(_ => _.EventName == model.CouncilDepartmentSub)?.EventCode ?? events.FirstOrDefault(_ => _.EventName == "none")?.EventCode;

            if ((model.CouncilDepartment == "libraries" || model.CouncilDepartmentSub == "libraries") &&
                (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "prod" || Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "stage"))
            {
                eventCode = 2002782;
            }

            string name = string.IsNullOrEmpty(model.Name) ? "Not provided" : model.Name;

            var crmCase = new Case
            {
                EventCode = (int)eventCode,
                EventTitle = $"Compliment",
                Description = $"Name: {name} \n\n Feedback: {model.Description}"
            };

            return crmCase;
        }
    }
}
