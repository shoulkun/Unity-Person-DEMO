using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
/// <summary>
/// 计时器
/// </summary>
public class Timer
{
    /// <summary>
    /// 时长
    /// </summary>
    public float Duration;
    /// <summary>
    /// 过去的时间
    /// </summary>
    public float LeftTime;
    private Action _updateAction;
    private Action _callAction;
    private bool _isPause;
    public Timer(float duration, Action updateAction=null, Action callAction=null, Action intiAction = null)
    {
        LeftTime = duration;
        Duration = duration;
        if (intiAction != null) intiAction.Invoke();
        _updateAction = updateAction;
        _callAction = callAction;
        _isPause = false;
    }
    public void OnUpdate(float deltaTime)
    {
        LeftTime -= deltaTime;
        if (LeftTime <= 0)
        {
            if (_callAction != null)
                _callAction.Invoke();
        }
        else
        {
            if (_updateAction != null && !_isPause)
                _updateAction.Invoke();  
        }
    }
    
    public void SetTimerTrick(bool b)
    {
        _isPause = b;
    }
}

