using Cat.Net.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace Cat.Auth
{
    public enum AuthType
    {
        Null = -1,
        MobileNumber = 0,
        WeChat =1,
        Email = 2
    }

    public class AuthUGUI : MonoBehaviour
    {
        AuthType authType = AuthType.Null;
        public AuthType AuthType
        {
            get { return this.authType; }
            set
            {
                if (this.authType != value)
                {
                    OnAuthTypeChange(value);
                }
                this.authType = value;
            }
        }
        /// <summary>
        /// 上面那个输入框
        /// </summary>
        public InputField ipf_id;
        /// <summary>
        /// 下面那个输入框
        /// </summary>
        public InputField ipf_Code;
        /// <summary>
        /// 微信的整个组件
        /// </summary>
        public GameObject WeChatPlane;
        /// <summary>
        /// 微信二维码
        /// </summary>
        public Image QRCodeImage;
        /// <summary>
        /// 刷新微信二维码的按钮
        /// </summary>
        public Button bt_ReflashWeChatCode;
        /// <summary>
        /// 微信登录页面返回（扫码页面回到一开始）
        /// </summary>
        public Button bt_WeChatReturn;
        /// <summary>
        /// 获取验证码按钮
        /// </summary>
        public Button bt_Verify;
        /// <summary>
        /// 登录按钮
        /// </summary>
        public Button bt_Login;
        /// <summary>
        /// 微信登录
        /// </summary>
        public Button bt_WeChat;
        /// <summary>
        /// Email登录
        /// </summary>
        public Button bt_Email;
        /// <summary>
        /// 手机短信登录
        /// </summary>
        public Button bt_MobileNumber;
        /// <summary>
        /// 登录成功后那个示例页面的返回按钮
        /// </summary>
        public Button bt_Return;
        /// <summary>
        /// 用来提示信息的UILogText
        /// </summary>
        public Text UILogText;
        /// <summary>
        /// 登录成功示例
        /// </summary>
        public GameObject LoginSuccessPanel;
        /// <summary>
        /// 验证等待间隔
        /// </summary>
        public int WaitVerifyInterval = 3;

        Color ipf_id_color;
        Color ipf_Code_color;
        void Start()
        {
            AuthType = AuthType.MobileNumber;
            #region --
            bt_Return.onClick.AddListener(() =>
                {
                    this.ipf_Code.text = string.Empty;
                    LoginSuccessPanel.SetActive(false);
                });
            bt_WeChat.onClick.AddListener(() =>
                {
                    this.AuthType = AuthType.WeChat;
                });
            bt_Email.onClick.AddListener(() =>
                {
                    this.AuthType = AuthType.Email;
                });
            bt_MobileNumber.onClick.AddListener(() =>
                {
                    this.AuthType = AuthType.MobileNumber;
                });
            bt_WeChat.onClick.AddListener(() =>
                {
                    //打开微信的请求页面
                    SetWeChatPlaneActivate();
                    //像微信服务器请求一个码
                    WeChatQRCodeRequest();
                });
            bt_ReflashWeChatCode.onClick.AddListener(()=>
                {
                    SetWeChatPlaneActivate();
                    WeChatQRCodeRequest();
                }
                );
            bt_WeChatReturn.onClick.AddListener(() =>
                {
                    //关闭微信的页面
                    SetWeChatPlaneUnActivate();
                });
            #endregion
            SetWeChatPlaneUnActivate();
            LoginSuccessPanel.SetActive(false);
            UILogText.gameObject.SetActive(false);
            ipf_id_color = ipf_id.GetComponent<Image>().color;
            ipf_Code_color = ipf_Code.GetComponent<Image>().color;
        }
        private void OnAuthTypeChange(AuthType newValue)
        {
            switch (newValue)
            {
                case AuthType.MobileNumber:
                    ipf_id.placeholder.GetComponent<Text>().text = "电话号码";
                    ipf_Code.placeholder.GetComponent<Text>().text = "短信验证码";
                    bt_Verify.onClick.RemoveAllListeners();
                    bt_Verify.onClick.AddListener(MobileNumberSendVCodeRequest);
                    bt_Login.onClick.RemoveAllListeners();
                    bt_Login.onClick.AddListener(MobileNumberVerifyCodeRequest);
                    break;
                case AuthType.Email:
                    ipf_id.placeholder.GetComponent<Text>().text = "邮箱号";
                    ipf_Code.placeholder.GetComponent<Text>().text = "邮箱验证码";
                    bt_Verify.onClick.RemoveAllListeners();
                    bt_Verify.onClick.AddListener(EmailSendVCodeRequest);
                    bt_Login.onClick.RemoveAllListeners();
                    bt_Login.onClick.AddListener(EmailVerifyCodeRequest);
                    break;
            }
        }
        /// <summary>
        /// 递减验证码按钮上的倒计时,到0后，启用发送验证码按钮
        /// </summary>
        /// <returns></returns>
        IEnumerator DecreaseProgressively()
        {
            int _delayTime = WaitVerifyInterval;//验证码获取按钮的等待周期
            Text VerifyText = bt_Verify.GetComponentInChildren<Text>();
            do
            {
                bt_Verify.GetComponentInChildren<Text>().text = _delayTime + "秒后重试";
                yield return new WaitForSeconds(1);
                _delayTime--;
            }
            while (_delayTime > 0);
            VerifyText.text = "获取验证码";
            bt_Verify.enabled = true;
        }
        /// <summary>
        /// 微信扫码界面-》等待用户刷新的状态
        /// </summary>
        void SetWeChatPlaneWaitReflash()
        {
            //开启刷新微信二维码的按钮
            bt_ReflashWeChatCode.gameObject.SetActive(true);
        }
        /// <summary>
        /// 开启微信登录的起始页面
        /// </summary>
        void SetWeChatPlaneActivate()
        {
            WeChatPlane.gameObject.SetActive(true);
            bt_ReflashWeChatCode.gameObject.SetActive(false);
        }
        void SetWeChatPlaneUnActivate()
        {
            WeChatPlane.gameObject.SetActive(false);
            QRCodeImage.sprite = null;
            bt_ReflashWeChatCode.gameObject.SetActive(false);
        }
        /// <summary>
        /// 让发送验证码按钮保持Hold状态
        /// </summary>
        void SetBtVerifyHold()
        {
            bt_Verify.GetComponentInChildren<Text>().text = "......";//发送期间是...
            bt_Verify.enabled = false;
        }
        void SetBtVerifyActivate()
        {
            bt_Verify.GetComponentInChildren<Text>().text = "发送验证码";//发送期间是...
            bt_Verify.enabled = true;
        }
        /// <summary>
        /// 设置登录按钮为Hold状态
        /// </summary>
        void SetBtLoginHold()
        {
            bt_Login.GetComponentInChildren<Text>().text = "正在登录...";
            bt_Login.enabled = false;
        }
        /// <summary>
        /// 设置登录按钮为活跃可用状态
        /// </summary>
        void SetBtLoginActivate()
        {
            bt_Login.GetComponentInChildren<Text>().text = "登录";
            bt_Login.enabled = true;
        }
        /// <summary>
        /// 延迟设置id输入框的颜色，一秒后又变回去
        /// </summary>
        /// <param name="color"></param>
        /// <param name="newColor"></param>
        /// <returns></returns>
        IEnumerator DelaySetIpf_id_Color(Color newColor)
        {
            Image image = ipf_id.GetComponent<Image>();
            image.color = newColor;//变个色
            yield return new WaitForSeconds(1);
            image.color = ipf_id_color;//变回去
        }
        /// <summary>
        /// 延迟设置id输入框的颜色，一秒后又变回去
        /// </summary>
        /// <param name="color"></param>
        /// <param name="newColor"></param>
        /// <returns></returns>
        IEnumerator DelaySetIpf_Code_Color(Color newColor)
        {
            Image image = ipf_Code.GetComponent<Image>();
            image.color = newColor;//变个色
            yield return new WaitForSeconds(1);
            image.color = ipf_Code_color;//变回去
        }
        IEnumerator DelayHideUILog()
        {
            yield return new WaitForSeconds(1);
            UILogText.gameObject.SetActive(false);
        }
        void UILog(Color color,string _text)
        {
            StartCoroutine(DelayHideUILog());
            UILogText.gameObject.SetActive(true);
            UILogText.color = color;
            UILogText.text = _text;
        }
        #region 短信验证操作
        /// <summary>
        /// 尝试发送短信验证码
        /// </summary>
        void MobileNumberSendVCodeRequest()
        {
            //检查输入，筛掉不必要的请求
            string phoneNum = ipf_id.text.Trim();
            if(! System.Text.RegularExpressions.Regex.IsMatch(phoneNum, @"^1[34578]\d{9}$"))
            {
                Debug.LogError("The phone number you entered is not valid !");
                StartCoroutine(DelaySetIpf_id_Color(Color.red));
                UILog(Color.red,"电话号码无效!");
                return;
            }
            //发送一个“发短信”的请求
            try
            {
                MobileNumberSendVCodeRequest request = new MobileNumberSendVCodeRequest(phoneNum);
                //MobileNumberSendVCodeRequest request = new MobileNumberSendVCodeRequest("15025113436");
                request.GetResponseAsync();
                request.OnMobileNumberSendComplete += Request_OnMobileNumberSendComplete;

                UILog(Color.green, "正在发送....");
                //处理发送验证码按钮
                SetBtVerifyHold();
            }
            catch (Exception e)
            {
                Debug.LogError("Unable to send SMS verification code," + e.Message);
                UILog(Color.red, "无法发送" + e.Message);
                SetBtVerifyActivate();
            }
        }
        /// <summary>
        /// 短信验证码已发送
        /// </summary>
        private void Request_OnMobileNumberSendComplete()
        {
            //验证码间隔
            StartCoroutine(DecreaseProgressively());
            UILog(Color.green, "已发送");
        }
        /// <summary>
        /// 尝试发送验证短信验证码是否正确的请求
        /// </summary>
        void MobileNumberVerifyCodeRequest()
        {
            string phoneNum = ipf_id.text.Trim();
            if (!Regex.IsMatch(phoneNum, @"^1[34578]\d{9}$"))
            {
                Debug.Log("The phone number you entered is not valid !");
                StartCoroutine(DelaySetIpf_id_Color(Color.red));
                UILog(Color.red, "电话号码无效!");
                return;
            }
            string code = ipf_Code.text.Trim();
            if (!Regex.IsMatch(code, @"^\d{6}$"))
            {
                Debug.Log("The captcha you entered is not a six-digit number !");
                StartCoroutine(DelaySetIpf_Code_Color(Color.red));
                UILog(Color.red, "验证码无效!");
                return;
            }
            try
            {
                MobileNumberVerifyCodeRequest request = new MobileNumberVerifyCodeRequest(phoneNum, code);
                request.GetResponseAsync();
                request.OnMobileNumberVerifyCodeRequestComplete += Request_OnMobileNumberVerifyCodeRequestComplete;
                //处理登录按钮
                SetBtLoginHold();
            }
            catch (Exception e)
            {
                Debug.LogError("Unable to verify SMS verification code," + e.Message);
                UILog(Color.red, "无法验证" + e.Message);
                SetBtLoginActivate();
            }
        }
        /// <summary>
        /// 得到短信验证后的结果
        /// </summary>
        /// <param name="isVerify"></param>
        private void Request_OnMobileNumberVerifyCodeRequestComplete(bool isVerify)
        {
            if(isVerify)
            {
                Debug.Log("phone number isVerify success!");
                UILog(Color.green, "跳转中");
                LoginSuccessPanel.SetActive(true);
            }
            else
            {
                Debug.Log("phone number isVerify Failed!");
                UILog(Color.red, "验证码错误!");
            }
            //验证完成，启用登录按钮
            SetBtLoginActivate();
        }
        #endregion

        #region 邮箱验证操作相关
        /// <summary>
        /// 向服务器发起，发送邮箱的请求
        /// </summary>
        void EmailSendVCodeRequest()
        {
            string email = ipf_id.text.Trim();
            if (!Regex.IsMatch(email, @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"))
            {
                Debug.Log("The E_mail number you entered is not valid !");
                StartCoroutine(DelaySetIpf_id_Color(Color.red));
                UILog(Color.red, "邮箱号无效!");
                return;
            }
            EmailSendVCodeRequest request = new EmailSendVCodeRequest(email, 30000);
            request.GetResponseAsync();
            request.OnMobileNumberSendComplete += Request_OnMobileNumberSendComplete1;

            UILog(Color.green, "正在发送....");
            SetBtVerifyHold();
        }
        /// <summary>
        /// 发送邮箱请求已发送完成
        /// </summary>
        private void Request_OnMobileNumberSendComplete1()
        {
            //验证码间隔
            StartCoroutine(DecreaseProgressively());
            UILog(Color.green, "已发送");
        }
        /// <summary>
        /// 发送，验证邮箱验证码是否正确 的请求
        /// </summary>
        void EmailVerifyCodeRequest()
        {
            //check
            string email = ipf_id.text.Trim();
            if (!Regex.IsMatch(email, @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"))
            {
                Debug.Log("The E_mail number you entered is not valid !");
                StartCoroutine(DelaySetIpf_id_Color(Color.red));
                UILog(Color.red, "邮箱号无效!");
                return;
            }
            string code = ipf_Code.text.Trim();
            if (!Regex.IsMatch(code, @"^\d{6}$"))
            {
                Debug.Log("The captcha you entered is not a six-digit number !");
                StartCoroutine(DelaySetIpf_Code_Color(Color.red));
                UILog(Color.red, "验证码无效!");
                return;
            }
            //try send
            EmailVerifyCodeRequest request = new EmailVerifyCodeRequest(email, code);
            request.GetResponseAsync();
            request.OnExceptionCatched += Request_OnExceptionCatched;
            request.OnMobileNumberVerifyCodeRequestComplete += Request_OnMobileNumberVerifyCodeRequestComplete1;
            SetBtLoginHold();
        }
        /// <summary>
        /// 获取验证码时异常
        /// </summary>
        /// <param name="e"></param>
        private void Request_OnExceptionCatched(Exception e)
        {
            Debug.LogError("Unable to verify Email verification code," + e.ToString());
            UILog(Color.red, "无法验证！");
            SetBtLoginActivate();
        }

        /// <summary>
        /// 获得验证结果
        /// </summary>
        /// <param name="isVerify"></param>
        private void Request_OnMobileNumberVerifyCodeRequestComplete1(bool isVerify)
        {
            if (isVerify)
            {
                Debug.Log("phone number isVerify success!");
                UILog(Color.green, "跳转中");
                LoginSuccessPanel.SetActive(true);
            }
            else
            {
                Debug.Log("phone number isVerify Failed!");
                UILog(Color.red, "验证码错误!");
            }
            SetBtLoginActivate();
        }
        #endregion

        #region 微信登录相关
        string imgID = string.Empty;
        /// <summary>
        /// 像微信服务器请求一个二维码和二维码唯一ID
        /// </summary>
        void WeChatQRCodeRequest()
        {
            WeChatQRCodeRequest request = new WeChatQRCodeRequest(10000);
            request.GetResponseAsync();
            request.OnExceptionCatched += Request_OnExceptionCatched1;
            request.OnGetQRCodeAsyncComplete += Request_OnGetQRCodeComplete;
            Debug.Log("开始请求微信二维码");
        }

        private void Request_OnExceptionCatched1(Exception e)
        {
            Debug.LogError("请求二维码时出错" + e.ToString());
        }

        /// <summary>
        /// 获得二维码以后
        /// </summary>
        /// <param name="QRCodePair"></param>
        private void Request_OnGetQRCodeComplete(KeyValuePair<string, Sprite> QRCodePair)
        {
            //获得码以后，把码显示
            imgID = QRCodePair.Key;
            QRCodeImage.sprite = QRCodePair.Value;
            //拿到码以后开始检测用户状态
            WeChatGetUserStausRequest();
        }
        /// <summary>
        /// 发起验证微信用户状态的请求
        /// </summary>
        void WeChatGetUserStausRequest()
        {
            WeChatGetUserStausRequest request = new WeChatGetUserStausRequest(imgID, 20000);
            request.GetResponseAsync();
            request.OnExceptionCatched += Request_OnExceptionCatched2;
            request.OnGetUserStatusComplete += Request_OnGetUserStatusComplete;
        }

        private void Request_OnExceptionCatched2(Exception e)
        {
            Debug.LogError("出错"+e.ToString());
        }

        /// <summary>
        /// 得到微信用户的状态
        /// </summary>
        /// <param name="result"></param>
        private void Request_OnGetUserStatusComplete(Result result)
        {
            Debug.Log(result.Code);
            if(result.Code == AuthCode.success)
            {
                Debug.Log("WeChat isVerify success!");
                UILog(Color.green, "跳转中");
                SetWeChatPlaneUnActivate();
                LoginSuccessPanel.SetActive(true);
            }
            else if (result.Code == AuthCode.faild)//用户自己取消的
            {
                Debug.Log("WeChat isVerify faild!");
                SetWeChatPlaneUnActivate();
            }
            else if (result.Code == AuthCode.invalid)//过期
            {
                SetWeChatPlaneWaitReflash();//打开刷新
            }
            else if(result.Code == AuthCode.idle)
            {
                WeChatGetUserStausRequest();//接着请求！这里如果出问题，递归不会结束，需要谨慎处理
            }
        }
        #endregion
    }
}
