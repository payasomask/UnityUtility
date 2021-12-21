using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="New EventData",menuName = "ScriptableObjectData/EventBaseData")]
public class EventBaseData : ScriptableObject
{
  List<EventBaseHandler> ListenerList = new List<EventBaseHandler>();
  public void invoke() {
    for(int i = ListenerList.Count -1; i>=0; i--) {
      ListenerList[i].invoke();
    }
  }
  public virtual void invoke(string dataString) {
    for (int i = ListenerList.Count - 1; i >= 0; i--) {
      ListenerList[i].invoke(dataString);
    }
  }
  public void Register(EventBaseHandler e) {
    if (ListenerList.Contains(e))
      return;
    ListenerList.Add(e);
  }
  public void Unregister(EventBaseHandler e) {
    if (!ListenerList.Contains(e))
      return;
    ListenerList.Remove(e);
  }
}
