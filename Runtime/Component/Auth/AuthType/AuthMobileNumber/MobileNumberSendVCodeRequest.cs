using Cat.Net.Http;

namespace Cat.Auth
{
    /// <summary>
    /// 发送手机验证码
    /// </summary>
    public class MobileNumberSendVCodeRequest
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
        /// <summary>
        /// URL
        /// </summary>
        public string RequestUrl = URLConst.URL_MobileNumberSendVCodeRequest;
        

        HttpBaseService http;
        public delegate void OnMobileNumberSendCompleteHandle();
        public event OnMobileNumberSendCompleteHandle OnMobileNumberSendComplete;
        public event OnExceptionCatchedHandle OnExceptionCatched;
        public MobileNumberSendVCodeRequest(string _number,int _timeout = 5000)
        {
            http = new HttpBaseService();
            http.TimeOut = _timeout;
            this.Number = _number;
        }
        public MobileNumberSendVCodeRequest(int _timeout = 5000)
        {
            http = new HttpBaseService();
            http.TimeOut = _timeout;
        }
        public void GetResponse()
        {
            if (string.IsNullOrEmpty(Number))
                throw new System.ArgumentNullException("the phone number is invalid");
            System.Net.HttpWebResponse response = http.GetHttpResponse(RequestUrl + Number);
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
                System.Net.HttpWebResponse response = await http.GetHttpResponseAsync(RequestUrl + Number);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    OnMobileNumberSendComplete?.Invoke();
                }
                else
                {
                    throw new System.Exception(nameof(MobileNumberSendVCodeRequest) + "==>" + response.StatusDescription);
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