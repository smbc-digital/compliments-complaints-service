using compliments_complaints_service.Models;

namespace compliments_complaints_service.Services
{
    public interface IComplaintsService
    {
        Task<string> CreateComplaintCase(ComplaintDetailsFormBuilder model);
    }
}
