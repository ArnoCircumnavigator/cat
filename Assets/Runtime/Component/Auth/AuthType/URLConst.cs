
public static class URLConst 
{
    const string BaseUrl = "http://www.celankeji.com/auth";
    //const string BaseUrl = "http://localhost:5010";
    /// <summary>
    /// 邮箱发送验证码
    /// </summary>
    public const string URL_EmailSendVCodeRequest = BaseUrl + "/Emails/GetCode?email=";
    /// <summary>
    /// 邮箱，验证，验证码
    /// </summary>
    public const string URL_EmailVerifyCodeRequest = BaseUrl+"/Emails/GetToken?";
    /// <summary>
    /// 短信验证，发送验证码
    /// </summary>
    public const string URL_MobileNumberSendVCodeRequest = BaseUrl + "/Mobiles/GetCode?number=";
    /// <summary>
    /// 短信验证，验证验证码
    /// </summary>
    public const string URL_MobileNumberVerifyCodeRequest = BaseUrl + "/Mobiles/GetToken?";
    /// <summary>
    /// 微信验证，获得登录二维码
    /// </summary>
    public const string URL_WeChatQRCodeRequest = BaseUrl + "/WeChat/GetQRCode";
    /// <summary>
    /// 微信验证，获得用户状态
    /// </summary>
    public const string URL_WeChatGetUserStausRequest = BaseUrl + "/WeChat/GetWeChatUserState?imgID=";
}
