using LOBCore.DataTransferObject.DTOs;
using LOBCore.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOBCore.BusinessObjects.Factories
{
    public static class APIResultFactory
    {
        public static APIResult Build(bool aPIResultStatus,
            int statusCodes = StatusCodes.Status200OK, ErrorMessageEnum errorMessageEnum = ErrorMessageEnum.None,
            object payload = null, string exceptionMessage = "")
        {
            APIResult apiResult = new APIResult()
            {
                Status = aPIResultStatus,
                ErrorCode = (int)errorMessageEnum,
                Message = (errorMessageEnum == ErrorMessageEnum.None) ? "" : $"錯誤代碼 {(int)errorMessageEnum}, {ErrorMessageMapping.Instance.GetErrorMessage(errorMessageEnum)}",
                HTTPStatus = statusCodes,
                Payload = payload,
            };
            if (apiResult.ErrorCode == (int)ErrorMessageEnum.Exception)
            {
                apiResult.Message = $"{apiResult.Message}{exceptionMessage}";
            }
            else if (string.IsNullOrEmpty(exceptionMessage) == false)
            {
                apiResult.Message = $"{exceptionMessage}";
            }
            return apiResult;
        }
    }
}
