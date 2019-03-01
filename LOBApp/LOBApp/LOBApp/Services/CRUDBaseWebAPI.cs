using LOBApp.DTOs;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LOBApp.Services
{
    public class CRUDBaseWebAPI<T> : BaseWebAPI<T>
    {
        public CRUDBaseWebAPI()
            : base()
        {
        }

        public async Task<APIResult> GetAsync(Dictionary<string, string> dic, CancellationToken ctoken = default(CancellationToken))
        {
            var mr = await this.SendAsync(dic, HttpMethod.Get, ctoken);
            return mr;
        }

        public async Task<APIResult> PostAsync(Dictionary<string, string> dic, CancellationToken ctoken = default(CancellationToken))
        {
            var mr = await this.SendAsync(dic, HttpMethod.Post, ctoken);
            return mr;
        }
    }
}
