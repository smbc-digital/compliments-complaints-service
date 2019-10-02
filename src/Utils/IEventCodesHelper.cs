using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace compliments_complaints_service.Utils
{
    public interface IEventCodesHelper
    {
        int getRealEventCode(string councilDepartment, string type);
    }
}
