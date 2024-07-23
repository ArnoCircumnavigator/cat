using Cat;
using NUnit.Framework;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.TestTools;
using Debug = UnityEngine.Debug;
using LogType = Cat.LogType;

namespace memo.RuntimeTests
{
    public class Test_Loger
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

        [Test, Order(1)]
        public void Test_logerLevel_None()
        {
            Loger.LogLevel = Cat.LogType.None;

            Loger loger = new Loger("prefix");
            loger.Log("loger.log");
            loger.LogWarning("loger.logWarning");
            loger.LogError("loger.logError");

            LogAssert.NoUnexpectedReceived();
        }

        [Test, Order(2)]
        public void Test_logerLevel_Warning()
        {
            Loger.LogLevel = Cat.LogType.Warning;

            Loger loger = new Loger("prefix");
            loger.Log("1");
            loger.LogWarning("2");
            loger.LogError("3");

            LogAssert.Expect(UnityEngine.LogType.Warning, "2");
            LogAssert.Expect(UnityEngine.LogType.Error, "3");
        }

        [Test, Order(3)]
        public void Test_logerLevel_Info()
        {
            Loger.LogLevel = LogType.Info;

            Loger loger = new Loger("prefix");
            loger.Log("1");
            loger.LogWarning("2");
            loger.LogError("3");

            LogAssert.Expect(UnityEngine.LogType.Log, "1");
            LogAssert.Expect(UnityEngine.LogType.Warning, "2");
            LogAssert.Expect(UnityEngine.LogType.Error, "3");
        }

        /// <summary>
        /// 写文件正常
        /// </summary>
        [Test, Order(4)]
        public void Test_logerFile_opend()
        {
            Loger.LogLevel = LogType.Info;
            Loger.FileLogSwitch = true;

            Loger loger = new Loger("prefix");
            loger.Log("1");
            loger.LogWarning("2");
            loger.LogError("3");
            LogAssert.Expect(UnityEngine.LogType.Error, "3");

            string path = Loger.FileLogPath + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt";
            Assert.IsTrue(File.Exists(path));
        }
    }
}
