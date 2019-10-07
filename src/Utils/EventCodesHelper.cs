using compliments_complaints_service.Config;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace compliments_complaints_service.Utils
{
    public class EventCodesHelper : IEventCodesHelper 
    {
        public int getRealEventCode(string councilDepartment, string type)
        {
            string jsonCodes = System.IO.File.ReadAllText(@".\Config\"+type+".json");
            var listJsonCodes = JsonConvert.DeserializeObject<List<EventModel>>(jsonCodes);

            var singleCode = from e in listJsonCodes
                             where e.EventName == councilDepartment
                             select e.EventCode;
            return singleCode.FirstOrDefault();
        }
    }
}
