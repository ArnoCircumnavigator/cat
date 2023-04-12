using Cat.Net.Http;

namespace Cat.Auth 
{
    /// <summary>
    /// 验证，手机验证码
    /// </summary>
    public class MobileNumberVerifyCodeRequest
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
        public string Number = string.Empty;
        public string Code = string.Empty;
        /// <summary>
        /// URL
        /// </summary>
        public string RequestUrl = URLConst.URL_MobileNumberVerifyCodeRequest;

        HttpBaseService http;
        public delegate void OnMobileNumberVerifyCodeRequestCompleteHandle(bool isVerify);
        public event OnMobileNumberVerifyCodeRequestCompleteHandle OnMobileNumberVerifyCodeRequestComplete;
        public event OnExceptionCatchedHandle OnExceptionCatched;
        public MobileNumberVerifyCodeRequest(int _timeout = 5000)
        {
            http = new HttpBaseService();
            http.TimeOut = _timeout;
        }
        public MobileNumberVerifyCodeRequest(string _number, string _code, int _timeout = 5000)
        {
            http = new HttpBaseService();
            http.TimeOut = _timeout;
            this.Number = _number;
            this.Code = _code;
        }
        public void GetResponse()
        {
            if (string.IsNullOrEmpty(Number))
                throw new System.ArgumentNullException("the phone number is invalid");
            System.Net.HttpWebResponse response = http.GetHttpResponse(RequestUrl + "number=" + Number + "&code=" + Code);
            response.Close();
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                OnExceptionCatched.Invoke(new System.Exception(nameof(MobileNumberSendVCodeRequest) + "==>" + response.StatusDescription));
                throw new System.Exception(nameof(MobileNumberSendVCodeRequest) + "==>" + response.StatusDescription);
            }
        }
        public async void GetResponseAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(Number))
                    throw new System.ArgumentNullException("the phone number is invalid");
                System.Net.HttpWebResponse response = await http.GetHttpResponseAsync(RequestUrl + "number=" + Number + "&code=" + Code);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    OnMobileNumberVerifyCodeRequestComplete?.Invoke(true);
                }
                else
                {
                    OnMobileNumberVerifyCodeRequestComplete?.Invoke(false);
                }
                response.Close();
            }
            catch (System.Exception e)
            {
                OnExceptionCatched.Invoke(e);
            }
        }
        public void Abort()
        {
            http.Abort();
        }
    }

}