using LOBApp.Common.DTOs;
using LOBApp.Common.Helpers;
using LOBApp.Common.Helpers.WebAPIs;
using LOBApp.Common.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LOBApp.Common.Services
{
    public class LeaveFormsManager : BaseWebAPI<LeaveFormResponseDTO>
    {
        private readonly AppStatus appStatus;

        public LeaveFormsManager(AppStatus appStatus)
            : base()
        {
            //資料檔案名稱 = "SampleRepository.txt";
            //this.url = "/webapplication/ntuhwebadminapi/webadministration/T0/searchDoctor";
            this.url = "/api/LeaveForms";
            this.host = "https://lobworkshop.azurewebsites.net";
            this.appStatus = appStatus;
        }

        public async Task<APIResult> GetAsync()
        {
            token = appStatus.SystemStatus.Token;
            encodingType = EnctypeMethod.JSON;
            needSave = true;
            isCollection = true;
            routeUrl = $"";

            #region 要傳遞的參數
            //Dictionary<string, string> dic = new Dictionary<string, string>();
            WebQueryDictionary dic = new WebQueryDictionary();

            // ---------------------------- 另外兩種建立 QueryString的方式
            //dic.Add(Global.getName(() => memberSignIn_QS.app), memberSignIn_QS.app);
            //dic.AddItem<string>(() => 查詢資料QueryString.strHospCode);
            //dic.Add("Price", SetMemberSignUpVM.Price.ToString());
            //dic.Add(LOBGlobal.JSONDataKeyName, JsonConvert.SerializeObject(leaveFormRequestDTO));
            #endregion

            var mr = await this.SendAsync(dic, HttpMethod.Get, CancellationToken.None);

            //mr.Success = false;
            //mr.Message = "測試用的錯誤訊息";
            return mr;
        }

        public async Task<APIResult> PostAsync(LeaveFormRequestDTO leaveFormRequestDTO)
        {
            token = appStatus.SystemStatus.Token;
            encodingType = EnctypeMethod.JSON;
            needSave = false;
            isCollection = false;
            routeUrl = $"";

            #region 要傳遞的參數
            //Dictionary<string, string> dic = new Dictionary<string, string>();
            WebQueryDictionary dic = new WebQueryDictionary();

            // ---------------------------- 另外兩種建立 QueryString的方式
            //dic.Add(Global.getName(() => memberSignIn_QS.app), memberSignIn_QS.app);
            //dic.AddItem<string>(() => 查詢資料QueryString.strHospCode);
            //dic.Add("Price", SetMemberSignUpVM.Price.ToString());
            dic.Add(LOBGlobal.JSONDataKeyName, JsonConvert.SerializeObject(leaveFormRequestDTO));
            #endregion

            var mr = await this.SendAsync(dic, HttpMethod.Post, CancellationToken.None);

            //mr.Success = false;
            //mr.Message = "測試用的錯誤訊息";
            return mr;
        }

        public async Task<APIResult> PutAsync(int id, LeaveFormRequestDTO leaveFormRequestDTO)
        {
            token = appStatus.SystemStatus.Token;
            encodingType = EnctypeMethod.JSON;
            needSave = false;
            isCollection = false;
            routeUrl = $"/{id}";

            #region 要傳遞的參數
            //Dictionary<string, string> dic = new Dictionary<string, string>();
            WebQueryDictionary dic = new WebQueryDictionary();

            // ---------------------------- 另外兩種建立 QueryString的方式
            //dic.Add(Global.getName(() => memberSignIn_QS.app), memberSignIn_QS.app);
            //dic.AddItem<string>(() => 查詢資料QueryString.strHospCode);
            //dic.Add("Price", SetMemberSignUpVM.Price.ToString());
            dic.Add(LOBGlobal.JSONDataKeyName, JsonConvert.SerializeObject(leaveFormRequestDTO));
            #endregion

            var mr = await this.SendAsync(dic, HttpMethod.Put, CancellationToken.None);

            //mr.Success = false;
            //mr.Message = "測試用的錯誤訊息";
            return mr;
        }

        public async Task<APIResult> DeleteAsync(int id)
        {
            token = appStatus.SystemStatus.Token;
            encodingType = EnctypeMethod.JSON;
            needSave = false;
            routeUrl = $"/{id}";
            isCollection = false;

            #region 要傳遞的參數
            //Dictionary<string, string> dic = new Dictionary<string, string>();
            WebQueryDictionary dic = new WebQueryDictionary();

            // ---------------------------- 另外兩種建立 QueryString的方式
            //dic.Add(Global.getName(() => memberSignIn_QS.app), memberSignIn_QS.app);
            //dic.AddItem<string>(() => 查詢資料QueryString.strHospCode);
            //dic.Add("Price", SetMemberSignUpVM.Price.ToString());
            //dic.Add(LOBGlobal.JSONDataKeyName, JsonConvert.SerializeObject(leaveFormRequestDTO));
            #endregion

            var mr = await this.SendAsync(dic, HttpMethod.Delete, CancellationToken.None);

            //mr.Success = false;
            //mr.Message = "測試用的錯誤訊息";
            return mr;
        }

        public override async Task ReadFromFileAsync()
        {
            needSave = true;
            isCollection = true;
            await base.ReadFromFileAsync();
        }

        /// <summary>
        /// 將物件資料寫入到檔案中
        /// </summary>
        public override async Task WriteToFileAsync()
        {
            needSave = true;
            isCollection = true;
            await base.WriteToFileAsync();
        }
    }
}
