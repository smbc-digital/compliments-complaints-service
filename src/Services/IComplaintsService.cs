using System.Threading.Tasks;
using compliments_complaints_service.Models;

namespace compliments_complaints_service.Services
{
    public interface IComplaintsService
    {
        Task<string> CreateComplaintCaseFormBuilder(ComplaintDetailsFormBuilder model);
    }
}
