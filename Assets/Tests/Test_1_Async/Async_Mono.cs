using Cat;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor.VersionControl;
using UnityEngine;

namespace Cat.RuntimeTests
{
    public class Async_Mono : MonoBehaviour
    {
        readonly Loger loger_Fixed = new Loger("FixedUpdate");
        readonly Loger loger = new Loger("Update");
        // Start is called before the first frame update
        async void Start()
        {
            Loger.FileLogSwitch = true;

            var op = new Op();
            await op;
        }
        public bool text;
        bool pre_lock = false;
        async void Update()
        {
            if (pre_lock)
                return;

            pre_lock = true;
            loger.Log($"s");
            Stopwatch sw = new Stopwatch();
            sw.Start();

            bool result = Tool.Action_100ms();
            Task<bool> task = System.Threading.Tasks.Task.Run(
                () => Tool.Action_100ms()
                );

            Doooo();

            await task;


            sw.Stop();
            loger.Log($"e");
            pre_lock = false;
        }

        public void Doooo()
        {

        }

        void accccc(Action onComp)
        {
            Tool.Action_100ms();
            onComp?.Invoke();
        }
    }

    public static class Tool
    {
        static readonly Loger loger = new Loger("Tool");
        /// <summary>
        /// 一个耗时2ms的无法加速的操作
        /// </summary>
        public static bool Action_2ms()
        {
            Guid acitonGuid = Guid.NewGuid();
            loger.Log($"start {acitonGuid}");
            Thread.Sleep(2);
            loger.Log($"end {acitonGuid}");
            return new System.Random().Next(2) == 1;
        }

        /// <summary>
        /// 一个耗时100ms的无法加速的操作
        /// </summary>
        public static bool Action_100ms()
        {
            Guid acitonGuid = Guid.NewGuid();
            loger.Log($"start {acitonGuid}");
            Thread.Sleep(100);
            loger.Log($"end {acitonGuid}");
            return new System.Random().Next(2) == 1;
        }
    }

    public class Op : UniversalAwaiter<bool>
    {
        public Op()
        {
            DoAction();

        }
        void DoAction()
        {
            Tool.Action_100ms();
            base.SetResult(false);//结束
        }
    }

    public class One_Operate : UniversalAwaiter<int>
    {
        public One_Operate()
        {
            DoAction();
        }

        void DoAction()
        {
            Thread.Sleep(2);
            base.SetResult(0);//结束
        }
    }

    public class WritePLC_Operate : UniversalAwaiter<int>
    {
        public WritePLC_Operate()
        {
            DoAction();
        }

        void DoAction()
        {
            //模拟一次IO
            Thread.Sleep(100);
            base.SetResult(0);
        }
    }
}
