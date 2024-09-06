using Cysharp.Threading.Tasks;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;


namespace Cat.RuntimeTests
{
    public class Test_MessageBox
    {
        private InputTestFixture input = new InputTestFixture();

        // NOTE: You have to manually call Setup() and TearDown() in this scenario.

        [SetUp]
        void SetUp()
        {
            input.Setup();
        }

        [TearDown]
        void TearDown()
        {
            input.TearDown();
        }

        private static Loger loger = new Loger("Test_MessageBox");

        [Test, Order(3)]
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
            Keyboard keyboard = InputSystem.AddDevice<Keyboard>();

            Mouse mouse = InputSystem.AddDevice<Mouse>();
            //mouse.rightButton.

            //Release(mouse.rightButton);
            Assert.IsTrue(result == DialogResult.Yes || result == DialogResult.No);
        }

        [Test, Order(2)]
        public async void Test_Async_2()
        {
            var dialogResult = await MessageBox.ShowAsync(WindowStyle.Warning, "小心", "请确保了解业务后再操作", Buttons.YesNo);
            Assert.IsTrue(dialogResult == DialogResult.Yes || dialogResult == DialogResult.No);
        }

        [UnityTest, Order(1)]
        public IEnumerator Test_ASync()
        {
            UniTask<Task<DialogResult>> task = UniTask.FromResult(MessageBox.ShowAsync(WindowStyle.Warning, "小心", "请确保了解业务后再操作", Buttons.YesNo));
            yield return task;
            //Assert.IsTrue(task.)

        }
    }
}