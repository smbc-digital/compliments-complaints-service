using System.Threading.Tasks;
using compliments_complaints_service.Models;
using StockportGovUK.NetStandard.Models.ComplimentsComplaints;

namespace compliments_complaints_service.Services
{
    public interface IComplaintsService
    {
        Task<string> CreateComplaintCase(ComplaintDetails model);

        Task<string> CreateComplaintCaseFormBuilder(ComplaintDetailsFormBuilder model);
    }
}
