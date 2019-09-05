using System;
using System.Collections.Generic;
using StockportGovUK.NetStandard.Models.Models.ComplimentsComplaints;
using System.Threading.Tasks;
using compliments_complaints_service.Models;

namespace compliments_complaints_service.Services
{
    public interface ICaseService
    {
        Task<bool> CreateComplimentCase(ComplimentDetails data);
    }


}
