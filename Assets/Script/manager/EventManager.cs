using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XMWorkspace
{
public class EventManager : Singleton<EventManager> {
    private Dictionary<Event, List<System.Action<object[]>>> m_eventList = new Dictionary<Event, List<System.Action<object[]>>>();

    public void RegisterEvent(Event eventType,System.Action<object[]> func)
    {
        if (m_eventList.ContainsKey(eventType) == false)
        {
            m_eventList.Add(eventType, new List<System.Action<object[]>>());
        }
        m_eventList[eventType].Add(func);
    }


   public void RemoveEvent(Event eventType, System.Action<object[]> func=null)
    {
        if (func == null)
        {
            m_eventList.Remove(eventType);
        }
        for (int i = 0; i < m_eventList[eventType].Count; i++)
        {
            if (m_eventList[eventType][i] == func)
            {
                m_eventList[eventType].RemoveAt(i);
                return;
            }
        }
    }

   public void NotifyEvent(Event eventType,params object[] param)
    {
        if (m_eventList.ContainsKey(eventType))
        {
            for (int i = 0; i < m_eventList[eventType].Count; i++)
            {
                m_eventList[eventType][i](param);
            }
        }
    }
}
}
