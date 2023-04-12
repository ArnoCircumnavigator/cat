using System.Text;
using System.Net;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;

namespace Cat.Net.Http
{
    public class HttpBaseService
    {
        public int TimeOut = 10;
        List<HttpWebRequest> AsyncRequests = new List<HttpWebRequest>();//所有的异步请求
        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GetResponse(string url)
        {
            HttpWebResponse response = null;
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            AsyncRequests.Add(request);
            request.Method = "GET";
            request.AllowWriteStreamBuffering = false;
            request.KeepAlive = true;
            request.ContentType = "application/x-www-form-urlencoded";

            // 接收返回的页面
            response = request.GetResponse() as HttpWebResponse;
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new System.IO.StreamReader(responseStream, Encoding.UTF8);
            string strResult = reader.ReadToEnd();
            reader.Close();
            response.Close();
            return strResult;
        }
        public HttpWebResponse GetHttpResponse(string url)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            AsyncRequests.Add(request);
            request.Method = "GET";
            request.AllowWriteStreamBuffering = false;
            request.KeepAlive = true;
            request.ContentType = "application/x-www-form-urlencoded";
            // 接收返回的页面
            return request.GetResponse() as HttpWebResponse;
        }
        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<string> GetResponseAsync(string url)
        {
            HttpWebResponse response = null;
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            AsyncRequests.Add(request);
            request.Method = "GET";
            request.AllowWriteStreamBuffering = false;
            request.KeepAlive = true;
            request.ContentType = "application/x-www-form-urlencoded";

            // 接收返回的页面
            var worktask = request.GetResponseAsync();
            var timeTask = Task.Delay(TimeOut);
            var task = await Task.WhenAny(worktask, timeTask);
            if (task == timeTask)
            {
                throw new System.TimeoutException(nameof(HttpBaseService));
            }
            //response = await request.GetResponseAsync() as HttpWebResponse;
            response = worktask.Result as HttpWebResponse;
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new System.IO.StreamReader(responseStream, Encoding.UTF8);
            string strResult = reader.ReadToEnd();
            reader.Close();
            response.Close();
            return strResult;
        }
        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<HttpWebResponse> GetHttpResponseAsync(string url)
        {
            HttpWebResponse response = null;
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            AsyncRequests.Add(request);
            request.Method = "GET";
            request.AllowWriteStreamBuffering = false;
            request.KeepAlive = true;
            request.ContentType = "application/x-www-form-urlencoded";

            // 接收返回的页面
            var worktask = request.GetResponseAsync();
            var timeTask = Task.Delay(TimeOut);
            var task = await Task.WhenAny(worktask, timeTask);
            if (task == timeTask)
            {
                throw new System.TimeoutException(nameof(HttpBaseService));
            }
            try
            {
                response = worktask.Result as HttpWebResponse;
            }
            catch (System.Exception e)
            {
                throw new System.Exception("请求异常" + e.Message);
            }
            return response;
        }
        /// <summary>
        /// 发送Post请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string PostResponse(string url, string param)
        {
            HttpWebResponse response = null;
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            AsyncRequests.Add(request);
            byte[] byteArray = Encoding.UTF8.GetBytes(param);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            //写入
            Stream newStream = request.GetRequestStream();//创建一个Stream,赋值是写入HttpWebRequest对象提供的一个stream里面
            newStream.Write(byteArray, 0, byteArray.Length);
            newStream.Close();
            // 接收返回的页面
            response = request.GetResponse() as HttpWebResponse;
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new System.IO.StreamReader(responseStream, Encoding.UTF8);
            string strResult = reader.ReadToEnd();
            reader.Close();
            response.Close();
            return strResult;
        }
        /// <summary>
        /// 发送Post请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<string> PostResponseAsync(string url, string param)
        {
            HttpWebResponse response = null;
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            AsyncRequests.Add(request);
            byte[] byteArray = Encoding.UTF8.GetBytes(param);
            request.Method = "POST";
            //表单
            //request.ContentType = "application/x-www-form-urlencoded";
            //Json
            request.ContentType = "application/json";
            
            request.ContentLength = byteArray.Length;
            //写入
            Stream newStream = request.GetRequestStream();//创建一个Stream,赋值是写入HttpWebRequest对象提供的一个stream里面
            newStream.Write(byteArray, 0, byteArray.Length);
            newStream.Close();
            // 接收返回的页面
            var worktask = request.GetResponseAsync();
            var timeTask = Task.Delay(TimeOut);
            var task = await Task.WhenAny(worktask, timeTask);
            if (task == timeTask)
            {
                throw new System.TimeoutException(nameof(HttpBaseService));
            }
            response = worktask.Result as HttpWebResponse;
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new System.IO.StreamReader(responseStream, Encoding.UTF8);
            string strResult = reader.ReadToEnd();
            reader.Close();
            response.Close();
            return strResult;
        }
        public Stream GetStreamReader(string url)
        {
            HttpWebResponse response = null;
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            AsyncRequests.Add(request);
            request.Method = "GET";
            request.AllowWriteStreamBuffering = false;
            request.KeepAlive = true;
            request.ContentType = "application/x-www-form-urlencoded";

            // 接收返回的页面
            response = request.GetResponse() as HttpWebResponse;
            Stream responseStream = response.GetResponseStream();
            return responseStream;
        }
        public async Task<Stream> GetStreamReaderAsync(string url)
        {
            HttpWebResponse response = null;
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            AsyncRequests.Add(request);
            request.Method = "GET";
            request.AllowWriteStreamBuffering = false;
            request.KeepAlive = true;
            request.ContentType = "application/x-www-form-urlencoded";

            // 接收返回的页面
            response = await request.GetResponseAsync() as HttpWebResponse;
            Stream responseStream = response.GetResponseStream();
            return responseStream;
        }
        /// <summary>
        /// 终止web请求
        /// </summary>
        public void Abort()
        {
            foreach (var t in AsyncRequests)
            {
                t.Abort();
                AsyncRequests.Remove(t);
            }
        }
        public static Result ConvertToClass_Xml(string xml)
        {
            if (string.IsNullOrEmpty(xml)) return default(Result);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Result));
            MemoryStream reader = new MemoryStream(Encoding.UTF8.GetBytes(xml));
            return xmlSerializer.Deserialize(reader) as Result;
        }
        public static JToken ConvertToClass_Json(string json)
        {
            return JToken.Parse(json);
        }
    }

}