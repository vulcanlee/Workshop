using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOBCore.Helpers
{
    public enum ErrorMessageEnum
    {
        None = 0,
        SecurityTokenExpiredException,
        帳號或密碼不正確 = 1000,
        權杖中沒有發現指定使用者ID,
        沒有發現指定的該使用者資料,
        傳送過來的資料有問題,
        沒有任何符合資料存在,
        沒有發現指定的請假單,
        權杖Token上標示的使用者與傳送過來的使用者不一致,
        沒有發現指定的請假單類別,
        要更新的紀錄_發生同時存取衝突_已經不存在資料庫上,
        紀錄更新時_發生同時存取衝突,
        紀錄更新所指定ID不一致,
        使用者需要強制登出並重新登入以便進行身分驗證,
        原有密碼不正確,
        新密碼不能為空白,
        沒有發現指定的發票,
        Exception = 2147483647,
    }
}
