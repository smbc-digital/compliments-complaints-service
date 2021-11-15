using System.Threading.Tasks;
using compliments_complaints_service.Models;

namespace compliments_complaints_service.Services
{
    public interface IFeedbackService
    {
        Task<string> CreateFeedbackCaseFormBuilder(FeedbackAndComplimentDetailsFormBuilder data);
    }
}
