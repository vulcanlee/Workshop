using LOBApp.DTOs;
using LOBApp.Helpers;
using LOBApp.Helpers.WebAPIs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LOBApp.Services
{
    public class AppExceptionsManager : BaseWebAPI<List<ExceptionRecordResponseDTO>>
    {
        public AppExceptionsManager()
            : base()
        {
            //資料檔案名稱 = "SampleRepository.txt";
            //this.url = "/webapplication/ntuhwebadminapi/webadministration/T0/searchDoctor";
            //this.url = "/api/ExceptionRecords";
            //this.host = "https://lobworkshop.azurewebsites.net";
        }
    }
}
