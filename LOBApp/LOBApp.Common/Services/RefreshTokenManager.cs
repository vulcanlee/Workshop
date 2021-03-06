﻿using LOBApp.Common.DTOs;
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
    public class RefreshTokenManager : BaseWebAPI<LoginResponseDTO>
    {
        private readonly AppStatus appStatus;

        public RefreshTokenManager(AppStatus appStatus)
            : base()
        {
            //資料檔案名稱 = "SampleRepository.txt";
            //this.url = "/webapplication/ntuhwebadminapi/webadministration/T0/searchDoctor";
            this.url = "/api/Login/RefreshToken";
            this.host = "https://lobworkshop.azurewebsites.net";
            this.appStatus = appStatus;
            isCollection = false;
        }

        public async Task<APIResult> GetAsync()
        {
            token = appStatus.SystemStatus.RefreshToken;
            encodingType = EnctypeMethod.JSON;
            needSave = true;

            #region 要傳遞的參數
            //Dictionary<string, string> dic = new Dictionary<string, string>();
            WebQueryDictionary dic = new WebQueryDictionary();

            // ---------------------------- 另外兩種建立 QueryString的方式
            //dic.Add(Global.getName(() => memberSignIn_QS.app), memberSignIn_QS.app);
            //dic.AddItem<string>(() => 查詢資料QueryString.strHospCode);
            //dic.Add("Price", SetMemberSignUpVM.Price.ToString());
            //dic.Add(LOBGlobal.JSONDataKeyName, JsonConvert.SerializeObject(loginRequestDTO));
            #endregion

            var mr = await this.SendAsync(dic, HttpMethod.Get, CancellationToken.None);

            //mr.Success = false;
            //mr.Message = "測試用的錯誤訊息";
            return mr;
        }
    }
}
