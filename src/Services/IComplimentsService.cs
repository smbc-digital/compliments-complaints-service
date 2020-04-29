using System.Threading.Tasks;
using compliments_complaints_service.Controllers.Models;
using StockportGovUK.NetStandard.Models.ComplimentsComplaints;

namespace compliments_complaints_service.Services
{
    public interface IComplimentsService
    {
        Task<string> CreateComplimentCase(ComplimentDetails data);

        Task<string> CreateComplimentCaseFormBuilder(PostData data);
    }
}
