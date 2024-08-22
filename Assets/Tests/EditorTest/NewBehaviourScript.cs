using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace memo.EditorTests
{
    public class TestIMeshCreater
    {
        // A Test behaves as an ordinary method
        // SetUp : some action before entry this Testing
        [SetUp]
        public void TestSetup()
        {
            
        }

        // TearDown : some action after every test
        [TearDown]
        public void TestTerDown()
        {
            Assert.IsTrue(true);
        }

        [UnityTest]
        public IEnumerator TestRuning()
        {
            yield return null;
            //static void action()
            //{
            //    Debug.Log("Runing1");
            //}
            //var ie = new IncrementalIEnumerator(KeyCode.S, KeyCode.P, new WaitForFixedUpdate(), action);
            //yield return ie.Execute();
            //yield return ie.Execute();
        }
    }
}
