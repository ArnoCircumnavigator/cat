using Cat.Net.Http;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cat.Auth
{
    /// <summary>
    /// 发送邮箱验证码
    /// </summary>
    public class EmailSendVCodeRequest : MonoBehaviour
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
        public string EmailNumber = string.Empty;
        /// <summary>
        /// URL
        /// </summary>
        public string RequestUrl = URLConst.URL_EmailSendVCodeRequest;

        HttpBaseService http;
        public delegate void OnMobileNumberSendCompleteHandle();
        public event OnMobileNumberSendCompleteHandle OnMobileNumberSendComplete;
        public event OnExceptionCatchedHandle OnExceptionCatched;
        public EmailSendVCodeRequest(string _emailNumber,int _timeout = 5000)
        {
            http = new HttpBaseService();
            http.TimeOut = _timeout;
            EmailNumber = _emailNumber;
        }
        public EmailSendVCodeRequest(int _timeout = 5000)
        {
            http = new HttpBaseService();
            http.TimeOut = _timeout;
        }
        public void GetResponse()
        {
            if (string.IsNullOrEmpty(EmailNumber))
                throw new System.ArgumentNullException("the phone number is invalid");
            System.Net.HttpWebResponse response = http.GetHttpResponse(RequestUrl + EmailNumber);
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
                if (string.IsNullOrEmpty(EmailNumber))
                    throw new System.ArgumentNullException("the phone number is invalid");
                System.Net.HttpWebResponse response = await http.GetHttpResponseAsync(RequestUrl + EmailNumber);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    OnMobileNumberSendComplete?.Invoke();
                }
                else
                {
                    throw new System.Exception(nameof(MobileNumberSendVCodeRequest) + "==>" + response.StatusDescription);
                }
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