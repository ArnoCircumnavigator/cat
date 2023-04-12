using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// 逐步式迭代器
/// </summary>
public class IncrementalIEnumerator
{
    bool startFlag = false;
    bool abortFlag = false;
    readonly YieldInstruction yieldInstruction;
    readonly KeyCode startKeyCode;
    readonly KeyCode endKeyCode;
    readonly Action action;
    /// <summary>
    /// 逐步式迭代器
    /// </summary>
    /// <param name="startKeyCode">启动键</param>
    /// <param name="endKeyCode">终止键</param>
    /// <param name="yieldInstruction"></param>
    /// <param name="action"></param>
    public IncrementalIEnumerator(KeyCode startKeyCode, KeyCode endKeyCode, YieldInstruction yieldInstruction, Action action)
    {
        this.startKeyCode = startKeyCode;
        this.endKeyCode = endKeyCode;
        this.yieldInstruction = yieldInstruction;
        this.action = action;
    }

    public IEnumerator Execute()
    {
        while (true)
        {
            //由于UTF的环境并不像真实的Playing状态
            //如果使用KeyDown,或者KeyUp,是存在不触发的可能的

            if (Input.GetKey(startKeyCode))
            {
                startFlag = true;
            }

            if (!startFlag)
            {
                yield return yieldInstruction;
                continue;
            }

            if (Input.GetKey(endKeyCode))
            {
                abortFlag = true;
            }

            if (abortFlag)
            {
                startFlag = false;
                abortFlag = false;
                break;
            }

            //Todo...
            action?.Invoke();

            yield return yieldInstruction;
        }
    }
}
