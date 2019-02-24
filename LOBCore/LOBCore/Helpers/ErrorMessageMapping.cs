using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOBCore.Helpers
{
    public class ErrorMessageMapping
    {
        private Dictionary<ErrorMessageEnum, string> ErrorMessages { get; set; }
        private static ErrorMessageMapping instance;
        private ErrorMessageMapping()
        {
            BuildErrorMessages();
        }

        private void BuildErrorMessages()
        {
            ErrorMessages = new Dictionary<ErrorMessageEnum, string>();
            ErrorMessages.Add(ErrorMessageEnum.None, "");
            ErrorMessages.Add(ErrorMessageEnum.SecurityTokenExpiredException, "存取權杖可用期限已經逾期超過");
            ErrorMessages.Add(ErrorMessageEnum.權杖中沒有發現指定使用者ID, "權杖中沒有發現指定使用者ID");
            ErrorMessages.Add(ErrorMessageEnum.帳號或密碼不正確, "帳號或密碼不正確");
            ErrorMessages.Add(ErrorMessageEnum.沒有發現指定的該使用者資料, "沒有發現指定的該使用者資料");
            ErrorMessages.Add(ErrorMessageEnum.傳送過來的資料有問題, "傳送過來的資料有問題");
            ErrorMessages.Add(ErrorMessageEnum.沒有任何符合資料存在, "沒有任何符合資料存在");
            ErrorMessages.Add(ErrorMessageEnum.沒有發現指定的請假單, "沒有發現指定的請假單");
            ErrorMessages.Add(ErrorMessageEnum.權杖Token上標示的使用者與傳送過來的使用者不一致, "權杖 Token 上標示的使用者與傳送過來的使用者不一致");
            ErrorMessages.Add(ErrorMessageEnum.沒有發現指定的請假單類別, "沒有發現指定的請假單類別");
            ErrorMessages.Add(ErrorMessageEnum.要更新的紀錄_發生同時存取衝突_已經不存在資料庫上, "要更新的紀錄，發生同時存取衝突，已經不存在資料庫上");
            ErrorMessages.Add(ErrorMessageEnum.紀錄更新時_發生同時存取衝突, "紀錄更新時，發生同時存取衝突");
            ErrorMessages.Add(ErrorMessageEnum.紀錄更新所指定ID不一致, "紀錄更新所指定 ID 不一致");
            ErrorMessages.Add(ErrorMessageEnum.Exception, "發生例外異常：");
        }

        public static ErrorMessageMapping Instance
        {
            get
            {
                // 若 instance 並沒有持有一個單例物件，則需要在這個時候，進行產生出來
                // ?? 若這個 單例物件 需要能夠在多執行緒環境下正確執行，又該如何設計呢？
                if (instance == null)
                {
                    instance = new ErrorMessageMapping();
                }
                return instance;
            }
        }
        public string GetErrorMessage(ErrorMessageEnum errorMessageEnum)
        {
            string fooMsg = "";
            if (ErrorMessages.ContainsKey(errorMessageEnum) == true)
            {
                fooMsg = ErrorMessages[errorMessageEnum];
            }
            return fooMsg;
        }
    }
}
