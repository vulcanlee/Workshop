using LOBCore.BusinessObjects.Factories;
using LOBCore.DataTransferObject.DTOs;
using LOBCore.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOBCore.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute, IActionFilter, IOrderedFilter
    {
        public int Order { get; set; } = 0;
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                string fooErrors = "";
                if(context.ModelState.ErrorCount > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    var fooConnectChart = $"{Environment.NewLine}";
                    foreach (var item in context.ModelState)
                    {
                        sb.Append(fooConnectChart);
                        sb.Append($"{item.Key} : ");
                        var fooErrorConnectChart = $" ";

                        foreach (var errorItem in item.Value.Errors)
                        {
                            sb.Append(fooErrorConnectChart);
                            if (errorItem.ErrorMessage != null)
                            {
                                sb.Append($"{errorItem.ErrorMessage}");
                            }else
                            {
                                sb.Append($"{errorItem.Exception.Message}");
                            }
                            fooErrorConnectChart = ", ";
                        }
                        fooConnectChart = ", ";
                    }
                    fooErrors = sb.ToString();
                }
                var apiResult = APIResultFactory.Build(false, StatusCodes.Status400BadRequest,
                  ErrorMessageEnum.傳送過來的資料有問題, 
                  exceptionMessage: fooErrors, replaceExceptionMessage:false);
                context.Result = new BadRequestObjectResult(apiResult);
            }
        }
    }
}
