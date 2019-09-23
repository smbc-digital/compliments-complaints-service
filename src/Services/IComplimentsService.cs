using System.Net.Http;
using StockportGovUK.NetStandard.Models.Models.ComplimentsComplaints;
using System.Threading.Tasks;
using StockportGovUK.AspNetCore.Gateways.Response;

namespace compliments_complaints_service.Services
{
    public interface IComplimentsService
    {
        Task<HttpResponse<string>> CreateComplimentCase(ComplimentDetails data);
    }


}
