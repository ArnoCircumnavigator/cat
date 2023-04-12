using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.TestRunner;

[assembly: TestRunCallback(typeof(MyTestRunCallback))]

public class MyTestRunCallback : ITestRunCallback
{
    public void RunStarted(ITest testsToRun)
    {

    }

    public void RunFinished(ITestResult testResults)
    {
        // 所有测试都已经完成
        //Debug.Log("RuningFinish");
    }

    public void TestStarted(ITest test)
    {

    }

    public void TestFinished(ITestResult result)
    {
        if (result.Test.IsSuite)
            return;

        if (result.ResultState.Status == TestStatus.Passed)
            return;

        Debug.Log($"Result of {result.Name}: {result.ResultState.Status}");
    }
}