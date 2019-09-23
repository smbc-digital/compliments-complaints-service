using StockportGovUK.NetStandard.Models.Models.ComplimentsComplaints;
using System.Threading.Tasks;
using StockportGovUK.AspNetCore.Gateways.Response;

namespace compliments_complaints_service.Services
{
    public interface IComplimentsService
    {
        Task<string> CreateComplimentCase(ComplimentDetails data);
    }


}
