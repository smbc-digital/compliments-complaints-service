using System.Threading.Tasks;
using compliments_complaints_service.Models;

namespace compliments_complaints_service.Services
{
    public interface IComplimentsService
    {
        Task<string> CreateComplimentCaseFormBuilder(FeedbackAndComplimentDetailsFormBuilder data);
    }
}
