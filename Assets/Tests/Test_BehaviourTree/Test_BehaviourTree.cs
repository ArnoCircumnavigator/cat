using BehaviourTreeGeneric;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.TestTools;

namespace Cat.RuntimeTests
{
    public class Test_BehaviourTree
    {
        // SetUp : some action before entry every test
        [SetUp]
        public void TestSetup()
        {

        }

        // TearDown : some action after every test
        [TearDown]
        public void TestTerDown()
        {
            //#if UNITY_EDITOR
            //            Assembly assembly = Assembly.GetAssembly(typeof(SceneView));
            //            System.Type type = assembly.GetType("UnityEditor.LogEntries");
            //            MethodInfo method = type.GetMethod("Clear");
            //            method.Invoke(new object(), null);
            //#endif
        }

        [Test, Order(0)]
        public void Test_BuildTree_From_jsontxt()
        {
            string path = Path.Combine(Application.dataPath, "Tests", "Test_BehaviourTree", "bt1.json");
            if (!File.Exists(path))
                Assert.Inconclusive();
            string jsonString = File.ReadAllText(path);
            JObject json = JObject.Parse(jsonString);
            BTree btree = new BTree();
            btree.Load(json);

            Debug.Log(btree);
        }

        [Test, Order(1)]
        public void Test_SerializeTree()
        {
            string path = Path.Combine(Application.dataPath, "Tests", "Test_BehaviourTree", "bt1.json");
            if (!File.Exists(path))
                Assert.Inconclusive();
            string jsonString = File.ReadAllText(path);
            JObject json = JObject.Parse(jsonString);
            BTree btree = new BTree();
            btree.Load(json);

            Debug.Log(btree);

            string copy_path = Path.Combine(Application.dataPath, "Tests", "Test_BehaviourTree", "bt1_copy.json");
            if (File.Exists(copy_path))
                File.Delete(copy_path);
            JToken serializeResult = btree.Serialize();
            File.WriteAllText(copy_path, serializeResult.ToString());
            Debug.Log(serializeResult);
        }

        [UnityTest, Order(1)]
        public IEnumerator Test_Run1()
        {
            string path = Path.Combine(Application.dataPath, "Tests", "Test_BehaviourTree", "bt1.json");
            if (!File.Exists(path))
                Assert.Inconclusive();
            string jsonString = File.ReadAllText(path);
            JObject json = JObject.Parse(jsonString);
            BTree btree = new BTree();
            btree.Load(json);
            while (true)
            {
                BContext input = new BContext();
                ActionResult res = btree.Run(input);
                if (res != ActionResult.RUNNING)
                    break;
                yield return new WaitForSeconds(1);
            }
        }

        [UnityTest, Order(2)]
        public IEnumerator Test_Run2()
        {
            string path = Path.Combine(Application.dataPath, "Tests", "Test_BehaviourTree", "bt2.json");
            if (!File.Exists(path))
                Assert.Inconclusive();
            string jsonString = File.ReadAllText(path);
            JObject json = JObject.Parse(jsonString);
            BTree btree = new BTree();
            btree.Load(json);
            Debug.Log(btree);

            while (true)
            {
                Debug.Log("----------------------------");
                //Debug.Log("trick");
                BContext input = new BContext();
                ActionResult res = btree.Run(input);
                if(res != ActionResult.RUNNING)
                    break;
                yield return new WaitForSeconds(1);
            }
        }
    }
}
