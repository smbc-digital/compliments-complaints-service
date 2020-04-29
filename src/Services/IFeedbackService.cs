using StockportGovUK.NetStandard.Models.ComplimentsComplaints;
using System.Threading.Tasks;
using compliments_complaints_service.Controllers.Models;

namespace compliments_complaints_service.Services
{
    public interface IFeedbackService
    {
        Task<string> CreateFeedbackCase(FeedbackDetails data);

        Task<string> CreateFeedbackCaseFormBuilder(PostData data);
    }
}
