namespace Cat
{
    /// <summary>
    /// 包装窗体的“提交”作为可等待的
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class WindowTask<T> : UniversalAwaiter<DialogResult> where T : IConfirmation
    {
        private T window;

        public WindowTask(T window)
        {
            this.window = window;
            window.OnConfirmation += Window_OnConfirmation;
        }

        private void Window_OnConfirmation(DialogResult result)
        {
            if (window != null)
                window.OnConfirmation -= Window_OnConfirmation;

            SetResult(result);

            window = default;
        }
    }
}