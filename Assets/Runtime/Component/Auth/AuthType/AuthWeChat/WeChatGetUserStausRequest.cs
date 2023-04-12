using Cat.Net.Http;
using Newtonsoft.Json;
namespace Cat.Auth
{
    /// <summary>
    /// 获取微信用户扫码状态
    /// </summary>
    public class WeChatGetUserStausRequest
    {
        int timeout;
        /// <summary>
        /// milliseconds
        /// </summary>
        public int Timeout
        {
            get { return timeout; }
            set
            {
                http.TimeOut = value;
                timeout = value;
            }
        }
        /// <summary>
        /// 微信二维码给的唯一标示二维码的ID
        /// </summary>
        public string ImgID = "";
        /// <summary>
        /// URL
        /// </summary>
        public string RequestUrl = URLConst.URL_WeChatGetUserStausRequest;

        HttpBaseService http;
        public delegate void OnGetUserStatusCompleteHandle(Result result);
        public event OnGetUserStatusCompleteHandle OnGetUserStatusComplete;
        public event OnExceptionCatchedHandle OnExceptionCatched;
        public WeChatGetUserStausRequest(int _timeout = 5000)
        {
            http = new HttpBaseService();
            http.TimeOut = _timeout;
        }
        public WeChatGetUserStausRequest(string _imgID,int _timeout = 5000)
        {
            http = new HttpBaseService();
            http.TimeOut = _timeout;
            this.ImgID = _imgID;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Result GetResponse()
        {
            try
            {
                string resultstring = http.GetResponse(RequestUrl + ImgID);
                var result = JsonConvert.DeserializeObject<Result>(resultstring);
                return result;
            }
            catch (System.Exception e)
            {
                OnExceptionCatched?.Invoke(e);
                throw e;
            }
        }
        /// <summary>
        /// 开始获取用户微信操作状态
        /// </summary>
        public async void GetResponseAsync()
        {
            try
            {
                string resultstring = await http.GetResponseAsync(RequestUrl + ImgID);
                var result = JsonConvert.DeserializeObject<Result>(resultstring);
                OnGetUserStatusComplete?.Invoke(result);
            }
            catch (System.Exception e)
            {
                OnExceptionCatched.Invoke(e);
            }
            
        }
    }

}