using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  Cat.Net.Http
{
    public enum AuthCode
    {
        success = 0,
        faild = -1,//用户拒绝
        invalid = -2,//正常请求，但是没有token
        idle = -3//用户没有扫
    }
    public class Result
    {
        public AuthCode Code { get; set; }
        public string Massage
        {
            get; set;
        }
        public Token Token
        {
            get; set;
        }
    }
    public class Token
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}