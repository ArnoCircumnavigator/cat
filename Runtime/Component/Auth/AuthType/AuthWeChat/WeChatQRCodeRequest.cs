using Cat.Net.Http;
using System.Collections.Generic;
using UnityEngine;

namespace Cat.Auth
{
    /// <summary>
    /// 获取微信二维码请求
    /// </summary>
    public class WeChatQRCodeRequest
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
        /// URL
        /// </summary>
        public string RequestUrl = URLConst.URL_WeChatQRCodeRequest;
        

        HttpBaseService http;
        public delegate void OnGetQRCodeAsyncCompleteHandle(KeyValuePair<string, Sprite> QRCodePair);
        public event OnGetQRCodeAsyncCompleteHandle OnGetQRCodeAsyncComplete;
        public event OnExceptionCatchedHandle OnExceptionCatched;
        public WeChatQRCodeRequest(int _timeout = 5000)
        {
            http = new HttpBaseService();
            http.TimeOut = _timeout;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>一张二维码，和微信给的唯一标示这张码的id</returns>
        public KeyValuePair<string, Sprite> GetResponse()
        {
            try
            {
                string result = http.GetResponse(RequestUrl);
                var t = HttpBaseService.ConvertToClass_Json(result);
                string imgID = t.Value<string>("imgID");
                string imgBase64 = t.Value<string>("imgBase64");
                byte[] bytes = System.Convert.FromBase64String(imgBase64);
                Texture2D tex2D = new Texture2D(470, 470);//微信官方给的base64固有尺寸
                tex2D.LoadImage(bytes);
                Sprite _sprite = Sprite.Create(tex2D, new Rect(0, 0, tex2D.width, tex2D.height), new Vector2(0.5f, 0.5f));
                return new KeyValuePair<string, Sprite>(imgID, _sprite);
            }
            catch (System.Exception e)
            {
                OnExceptionCatched.Invoke(e);
                throw e;
            }
        }
        /// <summary>
        /// 开始获取二维码
        /// </summary>
        public async void GetResponseAsync()
        {
            try
            {
                string result = await http.GetResponseAsync(RequestUrl);
                var t = HttpBaseService.ConvertToClass_Json(result);
                string imgID = t.Value<string>("imgID");
                string imgBase64 = t.Value<string>("imgBase64");
                byte[] bytes = System.Convert.FromBase64String(imgBase64);
                Texture2D tex2D = new Texture2D(470, 470);//微信官方给的base64固有尺寸
                tex2D.LoadImage(bytes);
                Sprite _sprite = Sprite.Create(tex2D, new Rect(0, 0, tex2D.width, tex2D.height), new Vector2(0.5f, 0.5f));
                OnGetQRCodeAsyncComplete?.Invoke(new KeyValuePair<string, Sprite>(imgID, _sprite));
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