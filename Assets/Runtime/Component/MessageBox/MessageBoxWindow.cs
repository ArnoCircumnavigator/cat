using System;
using UnityEngine;
using UnityEngine.UI;

namespace Cat
{
    /// <summary>
    /// messageBox的窗体
    /// 如果有改UI样式的需求，提供新的UI资源，重新实现这个类
    /// </summary>
    public class MessageBoxWindow : IMessageBoxWindow, IConfirmation
    {
        public event Action<DialogResult> OnConfirmation;

        bool uiObject = false;
        public void Show(WindowStyle style, string title, string caption, Buttons buttons)
        {
            if (!uiObject)
            {
                LoadUIAssets();
                uiObject = true;
            }

            bt1.onClick.RemoveAllListeners();
            bt2.onClick.RemoveAllListeners();
            bt3.onClick.RemoveAllListeners();

            SetWindowStyle(style);
            SetTitle(title);
            SetCaption(caption);
            SetButtons(buttons);
        }

        void SetWindowStyle(WindowStyle style)
        {
            switch (style)
            {
                case WindowStyle.Info:
                    iconImage.sprite = infoSprite;
                    break;
                case WindowStyle.Warning:
                    iconImage.sprite = warningSprite;
                    break;
                case WindowStyle.Error:
                    iconImage.sprite = errorSprite;
                    break;
            }
        }

        void SetTitle(string title)
        {
            titleText.text = title;
        }

        void SetCaption(string caption)
        {
            captionText.text = caption;
        }

        void SetButtons(Buttons buttons)
        {
            switch (buttons)
            {
                case Buttons.OK:
                    bt1.gameObject.SetActive(true);
                    bt2.gameObject.SetActive(false);
                    bt3.gameObject.SetActive(false);
                    bt1Text.text = "明白";
                    bt1.onClick.AddListener(() => { OnConfirmation?.Invoke(DialogResult.OK); });
                    break;
                case Buttons.OKCancel:
                    bt1.gameObject.SetActive(true);
                    bt2.gameObject.SetActive(true);
                    bt3.gameObject.SetActive(false);
                    bt1Text.text = "确认";
                    bt1.onClick.AddListener(() => { OnConfirmation?.Invoke(DialogResult.OK); });
                    bt2Text.text = "取消";
                    bt2.onClick.AddListener(() => { OnConfirmation?.Invoke(DialogResult.Cancel); });
                    break;
                case Buttons.AbortRetryIgnore:
                    bt1.gameObject.SetActive(true);
                    bt2.gameObject.SetActive(true);
                    bt3.gameObject.SetActive(true);
                    bt1Text.text = "终止";
                    bt1.onClick.AddListener(() => { OnConfirmation?.Invoke(DialogResult.Abort); });
                    bt2Text.text = "重试";
                    bt2.onClick.AddListener(() => { OnConfirmation?.Invoke(DialogResult.Retry); });
                    bt3Text.text = "忽略";
                    bt3.onClick.AddListener(() => { OnConfirmation?.Invoke(DialogResult.Ignore); });
                    break;
                case Buttons.YesNoCancel:
                    bt1.gameObject.SetActive(true);
                    bt2.gameObject.SetActive(true);
                    bt3.gameObject.SetActive(true);
                    bt1Text.text = "是";
                    bt1.onClick.AddListener(() => { OnConfirmation?.Invoke(DialogResult.Yes); });
                    bt2Text.text = "否";
                    bt2.onClick.AddListener(() => { OnConfirmation?.Invoke(DialogResult.No); });
                    bt3Text.text = "取消";
                    bt3.onClick.AddListener(() => { OnConfirmation?.Invoke(DialogResult.Cancel); });
                    break;
                case Buttons.YesNo:
                    bt1.gameObject.SetActive(true);
                    bt2.gameObject.SetActive(true);
                    bt3.gameObject.SetActive(false);
                    bt1Text.text = "是";
                    bt1.onClick.AddListener(() => { OnConfirmation?.Invoke(DialogResult.Yes); });
                    bt2Text.text = "否";
                    bt2.onClick.AddListener(() => { OnConfirmation?.Invoke(DialogResult.No); });
                    break;
                case Buttons.RetryCancel:
                    bt1.gameObject.SetActive(true);
                    bt2.gameObject.SetActive(true);
                    bt3.gameObject.SetActive(false);
                    bt1Text.text = "重试";
                    bt1.onClick.AddListener(() => { OnConfirmation?.Invoke(DialogResult.Retry); });
                    bt2Text.text = "取消";
                    bt2.onClick.AddListener(() => { OnConfirmation?.Invoke(DialogResult.Cancel); });
                    break;
            }
        }

        /*
         * 控件
         */
        GameObject uiGo = null;
        Image iconImage = null;
        Text titleText = null;
        Text captionText = null;
        Button bt1 = null;
        Text bt1Text = null;
        Button bt2 = null;
        Text bt2Text = null;
        Button bt3 = null;
        Text bt3Text = null;

        /*
         * 素材
         */
        Sprite infoSprite = null;
        Sprite warningSprite = null;
        Sprite errorSprite = null;

        /// <summary>
        /// 加载UI
        /// </summary>
        /// <returns></returns>
        public void LoadUIAssets()
        {
            var prefab = Resources.Load<GameObject>("MessageBoxWindow");
            uiGo = UnityEngine.Object.Instantiate(prefab);

            iconImage = FindChild(uiGo.transform, "icon").GetComponent<Image>();
            titleText = FindChild(uiGo.transform, "title").GetComponent<Text>();
            captionText = FindChild(uiGo.transform, "caption").GetComponent<Text>();
            bt1 = FindChild(uiGo.transform, "bt1").GetComponent<Button>();
            bt1Text = bt1.transform.GetComponentInChildren<Text>();
            bt2 = FindChild(uiGo.transform, "bt2").GetComponent<Button>();
            bt2Text = bt2.transform.GetComponentInChildren<Text>();
            bt3 = FindChild(uiGo.transform, "bt3").GetComponent<Button>();
            bt3Text = bt3.transform.GetComponentInChildren<Text>();

            infoSprite = Resources.Load<Sprite>("Testure/info");
            warningSprite = Resources.Load<Sprite>("Testure/warning");
            errorSprite = Resources.Load<Sprite>("Testure/error");
        }

        Transform FindChild(Transform parent, string name)
        {
            foreach (Transform child in parent)
            {
                if (child.name == name)
                {
                    return child;
                }

                Transform foundChild = FindChild(child, name);
                if (foundChild != null)
                {
                    return foundChild;
                }
            }
            return null;
        }
    }
}