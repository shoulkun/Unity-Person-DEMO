using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 局内时间管理器
/// </summary>
public class TimerManager : SingletonMonoBase<TimerManager>
{
    /// <summary>
    /// 局内定时器列表
    /// </summary>
    public List<Timer> _timers;
    private Dictionary<string, Timer> _timerDict;
    new void Awake()
    {
        base.Awake();
        _timers = new List<Timer>();
        _timerDict = new Dictionary<string, Timer>();
    }
    private void Update()
    {
        for (int i = 0; i < _timers.Count; i++)
        {
            _timers[i].OnUpdate(Time.deltaTime);
        }
    }
    public Timer AddTimer(string str,Timer timer)
    {
        if (_timerDict.ContainsKey(str))
        {
            _timerDict[str].LeftTime += _timerDict[str].Duration;
        }
        else
        {
            _timerDict.Add(str, timer);
            _timers.Add(timer);
        }
        return timer;
    }
    public void RemoveTimer(string str)
    {
        var timer = _timerDict[str];
        if (timer != null)
        {
            _timers.Remove(timer);
            _timerDict.Remove(str);
        }
    }
}
