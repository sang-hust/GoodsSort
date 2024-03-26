using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeGameManager : IInitialize
{
    private long totalSeconds;
    private Dictionary<string, Schedule> dicTimer;
    private List<string> _removeTag;

    public long GetTimeAfter(long seconds) => totalSeconds + seconds;
    /// <summary>
    /// Dùng khi lấy lại time từ server
    /// </summary>
    public void OverrideTimeServer()
    {
        WorldTimeAPI.Instance.GetTimeServer(() =>
        {
            totalSeconds = WorldTimeAPI.Instance.GetCurrentSecond();
            CoroutineChain.Stop(IETick);
            CoroutineChain.Start.Play(IETick);
        });
    }
    private IEnumerator IETick;
    private IEnumerator Tick()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(1f);
            foreach (var schedule in dicTimer.Values)
            {
                if (schedule.tickEnd < totalSeconds || schedule.tickEnd < 0)
                {
                    _removeTag.Add(schedule.key);
                    schedule.actionEnd?.Invoke();
                }
                else
                {
                    schedule.actionEverySecond?.Invoke(schedule.tickEnd - totalSeconds);
                }
            }
            totalSeconds++;

            if (_removeTag.Count == 0) continue;
            foreach (var tag in _removeTag)
                dicTimer.Remove(tag);
            _removeTag.Clear();
        }
    }
    public void OnInitialize()
    {
        IETick = Tick();
        dicTimer = new Dictionary<string, Schedule>();
        _removeTag = new List<string>();
        WorldTimeAPI.Instance.GetTimeServer(() =>
        {
            totalSeconds = WorldTimeAPI.Instance.GetCurrentSecond();
            CoroutineChain.Start.Play(IETick);
            timeManager = this;
        });
    }

    private void AddTick(string key, long tickEnd, Action<long> action, Action actionEnd)
    {
        if (_removeTag.Contains(key))
        {
            _removeTag.Remove(key);
        }
        if (dicTimer.ContainsKey(key))
        {
            dicTimer[key].tickEnd = tickEnd;
            dicTimer[key].actionEverySecond = action;
            dicTimer[key].actionEnd = actionEnd;
            return;
        }
        dicTimer.Add(key, new Schedule()
        {
            key = key,
            tickEnd = tickEnd,
            actionEverySecond = action,
            actionEnd = actionEnd,
        });
    }

    private static TimeGameManager timeManager;
    private void RemoveTick(string key)
    {
        if (dicTimer.ContainsKey(key)) dicTimer.Remove(key);
    }

    public static void Add(string key, long tickEnd, Action<long> action, Action actionEnd)
    {
        timeManager?.AddTick(key, tickEnd, action, actionEnd);
    }
    public static void Remove(string key)
    {
        timeManager._removeTag.Add(key);
    }
}

public class Schedule
{
    public string key;
    public Action<long> actionEverySecond;
    public Action actionEnd;
    public long tickEnd;
}

public static class KeySchedule
{
    public static string TimePlusHeart = "TimePlusHeart";
}