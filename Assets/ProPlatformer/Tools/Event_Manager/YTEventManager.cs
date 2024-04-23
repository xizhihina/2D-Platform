using System;
using System.Collections.Generic;
using UnityEngine.Events;
/// <summary>
/// 无参事件：
/// > 发布消息 TriggerEvent(EventName);
/// > 订阅消息 AddEventListener(EventName,Function);
/// > 取消订阅 RemovEventListener(EventName,Function);
/// 有参事件：
/// > 发布消息 TriggerEvent<ArgumentsType>(EventName,Arguments);
/// > 订阅消息 AddEventListener<ArgumentsType>(EventName,Function);
/// > 取消订阅 RemovEventListener<ArgumentsType>(EventName,Function);
/// </summary>
public class YTEventManager : Singleton<YTEventManager>
{
    private Dictionary<string, Delegate> _eventDic = new();
    /// <summary>
    /// 订阅事件核对，预防订阅事件为空或方法为空的情况，正常情况下不执行
    /// </summary>
    /// <param name="event_type"></param>
    /// <param name="listener"></param>
    void CheckAddingEvent(string event_type, Delegate listener)
    {
        if (_eventDic.ContainsKey(event_type) == false)
        {
            _eventDic.Add(event_type, null);
        }

        Delegate @delegate = _eventDic[event_type];
        if (@delegate != null && @delegate.GetType() != listener.GetType())
        {
            throw new Exception(
                $"try to add incorrect eventType : {@delegate}, needed listener type is {@delegate.GetType()}, given listener type is {listener.GetType()}.");
        }
    }
    /// <summary>
    /// 移除事件核对，确保被移除的事件存在
    /// </summary>
    /// <param name="event_type"></param>
    /// <param name="listener"></param>
    /// <returns></returns>
    bool CheckRemovingEvent(string event_type, Delegate listener)
    {
        bool resuit = false;
        if (!_eventDic.ContainsKey(event_type))
        {
            resuit = false;
        }
        else
        {
            Delegate @delegate = _eventDic[event_type];
            if (@delegate != null && @delegate.GetType() != listener.GetType())
            {
                throw new Exception(
                    $"try to remov incorrect eventType : {@delegate}, needed listener type is {@delegate.GetType()}, given listener type is {listener.GetType()}.");
            }
            resuit = true;
        }
        return resuit;
    }
    /// <summary>
    /// 清空事件表，清掉有键无值的事件
    /// </summary>
    /// <param name="event_type"></param>
    void OnListenerRemoved(string event_type)
    {
        if (_eventDic[event_type] == null)
        {
            _eventDic.Remove(event_type);
        }
    }

    /// <summary>
    /// 订阅事件
    /// </summary>
    /// <param name="event_type">事件名称</param>
    /// <param name="action"></param>
    public void AddEventListener(string event_type, UnityAction action)
    {
        CheckAddingEvent(event_type, action);
        _eventDic[event_type] = (UnityAction)Delegate.Combine((UnityAction)_eventDic[event_type], action);
    }
    public void AddEventListener<T>(string event_type, UnityAction<T> action)
    {
        CheckAddingEvent(event_type, action);
        _eventDic[event_type] = (UnityAction<T>)Delegate.Combine((UnityAction<T>)_eventDic[event_type], action);
    }
    public void AddEventListener<T, Y>(string event_type, UnityAction<T, Y> action)
    {
        CheckAddingEvent(event_type, action);
        _eventDic[event_type] = (UnityAction<T, Y>)Delegate.Combine((UnityAction<T, Y>)_eventDic[event_type], action);
    }
    public void AddEventListener<T, Y, U>(string event_type, UnityAction<T, Y, U> action)
    {
        CheckAddingEvent(event_type, action);
        _eventDic[event_type] = (UnityAction<T, Y, U>)Delegate.Combine((UnityAction<T, Y, U>)_eventDic[event_type], action);
    }
    /// <summary>
    /// 移除事件
    /// </summary>
    /// <param name="event_type">事件名称</param>
    /// <param name="action"></param>
    public void RemoveEventListener(string event_type, UnityAction action)
    {
        if (CheckRemovingEvent(event_type, action) == true)
        {
            _eventDic[event_type] = (UnityAction)Delegate.Remove((UnityAction)_eventDic[event_type], action);
            OnListenerRemoved(event_type);
        }
    }
    public void RemoveEventListener<T>(string event_type, UnityAction<T> action)
    {
        if (CheckRemovingEvent(event_type, action) == true)
        {
            _eventDic[event_type] = (UnityAction<T>)Delegate.Remove((UnityAction<T>)_eventDic[event_type], action);
            OnListenerRemoved(event_type);
        }
    }
    public void RemoveEventListener<T, Y>(string event_type, UnityAction<T, Y> action)
    {
        if (CheckRemovingEvent(event_type, action) == true)
        {
            _eventDic[event_type] = (UnityAction<T, Y>)Delegate.Remove((UnityAction<T, Y>)_eventDic[event_type], action);
            OnListenerRemoved(event_type);
        }
    }
    public void RemoveEventListener<T, Y, U>(string event_type, UnityAction<T, Y, U> action)
    {
        if (CheckRemovingEvent(event_type, action) == true)
        {
            _eventDic[event_type] = (UnityAction<T, Y, U>)Delegate.Remove((UnityAction<T, Y, U>)_eventDic[event_type], action);
            OnListenerRemoved(event_type);
        }
    }
    /// <summary>
    /// 广播事件
    /// </summary>
    /// <param name="event_type">事件名称</param>
    public void TriggerEvent(string event_type)
    {
        if (_eventDic.TryGetValue(event_type, out Delegate targetDelegate))
        {   //先判断是否有键
            if (targetDelegate == null)//再判断是否有值(委托引用的方法)
            {
                return;
            }

            if (targetDelegate.GetType() != typeof(UnityAction))
            {
                throw new Exception($"TriggerEvent {event_type} error : type sof parameters are not match.");
            }//防止类型错误

            UnityAction action = (UnityAction)targetDelegate;//为什么要用UnityAction装Delegate：自带多播方法，能直接传委托链
            try
            {
                action();
            }
            catch (Exception e)
            {
                throw new Exception($"TriggerEvent {event_type} error : {e}.");
            }
        }
    }
    public void TriggerEvent<T>(string event_type, T t)
    {
        if (_eventDic.TryGetValue(event_type, out Delegate targetDelegate))
        {
            if (targetDelegate == null)
            {
                return;
            }

            if (targetDelegate.GetType() != typeof(UnityAction<T>))
            {
                throw new Exception($"TriggerEvent {event_type} error : type sof parameters are not match.");
            }

            UnityAction<T> action = (UnityAction<T>)targetDelegate;

            try
            {
                action(t);
            }
            catch (Exception e)
            {
                throw new Exception($"TriggerEvent {event_type} error : {e}.");
            }
        }
    }
    public void TriggerEvent<T, Y>(string event_type, T t, Y y)
    {
        if (_eventDic.TryGetValue(event_type, out Delegate targetDelegate))
        {
            if (targetDelegate == null)
            {
                return;
            }

            if (targetDelegate.GetType() != typeof(UnityAction<T, Y>))
            {
                throw new Exception($"TriggerEvent {event_type} error : type sof parameters are not match.");
            }

            UnityAction<T, Y> action = (UnityAction<T, Y>)targetDelegate;

            try
            {
                action(t, y);
            }
            catch (Exception e)
            {
                throw new Exception($"TriggerEvent {event_type} error : {e}.");
            }
        }
    }
    public void TriggerEvent<T, Y, U>(string event_type, T t, Y y, U u)
    {
        if (_eventDic.TryGetValue(event_type, out Delegate targetDelegate))
        {
            if (targetDelegate == null)
            {
                return;
            }

            if (targetDelegate.GetType() != typeof(UnityAction<T, Y, U>))
            {
                throw new Exception($"TriggerEvent {event_type} error : type sof parameters are not match.");
            }

            UnityAction<T, Y, U> action = (UnityAction<T, Y, U>)targetDelegate;

            try
            {
                action(t, y, u);
            }
            catch (Exception e)
            {
                throw new Exception($"TriggerEvent {event_type} error : {e}.");
            }
        }
    }
}