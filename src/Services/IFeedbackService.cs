using System.Threading.Tasks;
using compliments_complaints_service.Models;
using StockportGovUK.NetStandard.Models.ComplimentsComplaints;

namespace compliments_complaints_service.Services
{
    public interface IFeedbackService
    {
        Task<string> CreateFeedbackCase(FeedbackDetails data);

        Task<string> CreateFeedbackCaseFormBuilder(FeedbackAndComplimentDetailsFormBuilder data);
    }
}
