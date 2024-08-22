using System;

namespace Cat
{
    /// <summary>
    /// messageBox的窗体
    /// </summary>
    public class MessageBoxWindow : IMessageBoxWindow, IConfirmation
    {
        public event Action<DialogResult> OnConfirmation;

        public void Show(WindowStyle style, string title, string caption, Buttons buttons)
        {
            //实例化UI(池子，复用)
            //事件堵塞问题
            throw new NotImplementedException();
        }
    }
}