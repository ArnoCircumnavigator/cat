using NUnit.Framework;
using System.Threading.Tasks;

namespace Cat.RuntimeTests
{
    public class Test_MessageBox
    {
        [SetUp]
        public void SetUp()
        {

        }

        [TearDown]
        public void TearDown()
        {

        }

        private static Loger loger = new Loger("Test_MessageBox");

        [Test, Order(0)]
        public void Test_Sync()
        {
            MessageBoxWindow window = MessageBox.Show(WindowStyle.Warning, "小心", "请确保了解业务后再操作", Buttons.YesNo);
            window.OnConfirmation += (result) =>
            {
                if (result == DialogResult.Yes)
                {

                }
                else if (result == DialogResult.No)
                {

                }
            };
        }

        [Test, Order(1)]
        public async Task Test_ASync()
        {
            DialogResult result = await MessageBox.ShowAsync(WindowStyle.Warning, "小心", "请确保了解业务后再操作", Buttons.YesNo);
            if (result == DialogResult.Yes)
            {
                loger.Log("yes");
            }
            else if (result == DialogResult.No)
            {
                loger.Log("no");
            }
        }
    }
}