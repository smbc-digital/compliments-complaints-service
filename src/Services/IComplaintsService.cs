using System.Threading.Tasks;
using compliments_complaints_service.Controllers.Models;
using StockportGovUK.NetStandard.Models.ComplimentsComplaints;

namespace compliments_complaints_service.Services
{
    public interface IComplaintsService
    {
        Task<string> CreateComplaintCase(ComplaintDetails model);

        Task<string> CreateComplaintCaseFormBuilder(PostData formData);
    }
}
