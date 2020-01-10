using StockportGovUK.NetStandard.Models.Models.ComplimentsComplaints;
using System.Threading.Tasks;
using compliments_complaints_service.Controllers.Models;
using compliments_complaints_service.Models;
using StockportGovUK.AspNetCore.Gateways.Response;

namespace compliments_complaints_service.Services
{
    public interface IFeedbackService
    {
        Task<string> CreateFeedbackCase(FeedbackDetails data);

        Task<string> CreateFeedbackCaseFormBuilder(PostData data);
    }
}
