using LOBCore.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOBCore.BusinessObjects.Factories
{
    public static class APIResultFactory
    {
        public static APIResult Build(bool aPIResultStatus)
        {
            APIResult foo = new APIResult()
            {
                 Status = aPIResultStatus
            };
            return foo;
        }
    }
}
