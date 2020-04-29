using System.Threading.Tasks;
using compliments_complaints_service.Controllers.Models;
using StockportGovUK.NetStandard.Models.ComplimentsComplaints;

namespace compliments_complaints_service.Services
{
    public interface IFeedbackService
    {
        Task<string> CreateFeedbackCase(FeedbackDetails data);

        Task<string> CreateFeedbackCaseFormBuilder(PostData data);
    }
}
