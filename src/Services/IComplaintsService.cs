using System.Threading.Tasks;
using StockportGovUK.AspNetCore.Gateways.Response;
using StockportGovUK.NetStandard.Models.Models.ComplimentsComplaints;

namespace compliments_complaints_service.Services
{
    public interface IComplaintsService
    {
        Task<string> CreateComplaintCase(ComplaintDetails model);
    }
}
