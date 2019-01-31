using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOBCore.Models
{
    public enum APIResultStatus
    {
        Success,
        Failure,
        TokenFailure
    }
    /// <summary>
    /// 呼叫 API 回傳的制式格式
    /// </summary>
    public class APIResult
    {
        /// <summary>
        /// 此次呼叫 API 是否成功
        /// </summary>
        public APIResultStatus Status { get; set; } = APIResultStatus.Success;
        /// <summary>
        /// 存取權杖
        /// </summary>
        public string Token { get; set; } = "";
        /// <summary>
        /// 呼叫 API 失敗的錯誤訊息
        /// </summary>
        public string Message { get; set; } = "";
        /// <summary>
        /// 呼叫此API所得到的其他內容
        /// </summary>
        public object Payload { get; set; }
    }
}
