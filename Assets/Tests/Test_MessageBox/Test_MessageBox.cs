using Cysharp.Threading.Tasks;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TestTools;

namespace Cat.RuntimeTests
{
    public class Test_MessageBox
    {
        private static Loger loger = new Loger("Test_MessageBox");

        [Test, Order(1)]
        public void Test_Sync()
        {
            MessageBoxWindow window = MessageBox.Show(WindowStyle.Warning, "小心", "请确保了解业务后再操作", Buttons.YesNo);
            DialogResult result = default;
            window.OnConfirmation += (result) =>
            {
                if (result == DialogResult.Yes)
                {
                    loger.Log("yes");
                }
                else if (result == DialogResult.No)
                {
                    loger.Log("no");
                }
            };

            //Keyboard keyboard = InputSystem.AddDevice<Keyboard>();
            //Mouse mouse = InputSystem.AddDevice<Mouse>();
            //1、鼠标放在合适的位置上
            //我想模拟鼠标在某个位置（不论是哪种坐标系统的位置），查不到资料，无法实现

            //2、按下左键
            //Press(mouse.leftButton);

            //3、根据鼠标位置获得结果，进行断言
            //Assert.IsTrue(result == DialogResult.Yes || result == DialogResult.No);
        }

        [UnityTest]
        [Explicit, Category("interactive")]
        public IEnumerator Test_Sync_Interactive()
        {
            //创建一个EventSystem，来监听正常的UI事件
            var eventSystemObject = new GameObject("BasicElement")
                .AddComponent<EventSystem>().gameObject
                .AddComponent<StandaloneInputModule>().gameObject
                .AddComponent<Camera>();

            //创建窗口
            Task<DialogResult> task = MessageBox.ShowAsync(WindowStyle.Warning, "小心", "请确保了解业务后再操作", Buttons.YesNo);

            while (!task.IsCompleted)
            {
                yield return null;
            }
            loger.Log(task.Result);
        }

        [Test, Order(2)]
        public async void Test_Async_2()
        {
            var dialogResult = await MessageBox.ShowAsync(WindowStyle.Warning, "小心", "请确保了解业务后再操作", Buttons.YesNo);
            Assert.IsTrue(dialogResult == DialogResult.Yes || dialogResult == DialogResult.No);
        }

        [UnityTest, Order(3)]
        public IEnumerator Test_ASync()
        {
            UniTask<Task<DialogResult>> task = UniTask.FromResult(MessageBox.ShowAsync(WindowStyle.Warning, "小心", "请确保了解业务后再操作", Buttons.YesNo));
            yield return task;
        }
    }
}