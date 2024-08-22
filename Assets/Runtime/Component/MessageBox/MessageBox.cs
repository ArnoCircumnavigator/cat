using System;
using System.Threading.Tasks;

namespace Cat
{
    /// <summary>
    /// 提示框
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class MessageBox
    {
        /// <summary>
        /// 打开回话窗
        /// </summary>
        /// <param name="style">窗口类型</param>
        /// <param name="title">标题</param>
        /// <param name="caption">描述</param>
        /// <param name="buttons">可交互的按钮</param>
        public static async Task<DialogResult> ShowAsync(WindowStyle style, string title, string caption, Buttons buttons)
        {
            var window = Show(style, title, caption, buttons);
            return await new WindowTask<MessageBoxWindow>(window);
        }

        /// <summary>
        /// 打开回话窗
        /// </summary>
        /// <param name="style">窗口类型</param>
        /// <param name="title">标题</param>
        /// <param name="caption">描述</param>
        /// <param name="buttons">可交互的按钮</param>
        public static MessageBoxWindow Show(WindowStyle style, string title, string caption, Buttons buttons)
        {
            MessageBoxWindow window = new MessageBoxWindow();
            window.Show(style, title, caption, buttons);
            return window;
        }
    }

    public interface IConfirmation
    {
        event Action<DialogResult> OnConfirmation;
    }

    public interface IMessageBoxWindow
    {
        /// <summary>
        /// 打开回话窗
        /// </summary>
        /// <param name="style">窗口类型</param>
        /// <param name="title">标题</param>
        /// <param name="caption">描述</param>
        /// <param name="buttons">可交互的按钮</param>
        void Show(WindowStyle style, string title, string caption, Buttons buttons);
    }

    public enum WindowStyle
    {
        Info,
        Warning,
        Error,
    }

    /// <summary>
    /// 按钮类型
    /// </summary>
    public enum Buttons
    {
        //
        // Summary:
        //     The message box contains an OK button.
        OK = 0,
        //
        // Summary:
        //     The message box contains OK and Cancel buttons.
        OKCancel = 1,
        //
        // Summary:
        //     The message box contains Abort, Retry, and Ignore buttons.
        AbortRetryIgnore = 2,
        //
        // Summary:
        //     The message box contains Yes, No, and Cancel buttons.
        YesNoCancel = 3,
        //
        // Summary:
        //     The message box contains Yes and No buttons.
        YesNo = 4,
        //
        // Summary:
        //     The message box contains Retry and Cancel buttons.
        RetryCancel = 5,
    }

    /// <summary>
    /// 结果类型
    /// </summary>
    public enum DialogResult
    {
        //
        // Summary:
        //     Nothing is returned from the dialog box. This means that the modal dialog continues
        //     running.
        None = 0,
        //
        // Summary:
        //     The dialog box return value is OK (usually sent from a button labeled OK).
        OK = 1,
        //
        // Summary:
        //     The dialog box return value is Cancel (usually sent from a button labeled Cancel).
        Cancel = 2,
        //
        // Summary:
        //     The dialog box return value is Abort (usually sent from a button labeled Abort).
        Abort = 3,
        //
        // Summary:
        //     The dialog box return value is Retry (usually sent from a button labeled Retry).
        Retry = 4,
        //
        // Summary:
        //     The dialog box return value is Ignore (usually sent from a button labeled Ignore).
        Ignore = 5,
        //
        // Summary:
        //     The dialog box return value is Yes (usually sent from a button labeled Yes).
        Yes = 6,
        //
        // Summary:
        //     The dialog box return value is No (usually sent from a button labeled No).
        No = 7,
        //
        // Summary:
        //     The dialog box return value is Try Again (usually sent from a button labeled
        //     Try Again).
        TryAgain = 10,
        //
        // Summary:
        //     The dialog box return value is Continue (usually sent from a button labeled Continue).
        Continue = 11
    }
}