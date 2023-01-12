using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventBaseHandler : MonoBehaviour
{
  [SerializeField]
  EventBaseData _TargetEvent;
  [SerializeField]
  public UnityEvent Actions;
  List<EventBaseClass> EventList = new List<EventBaseClass>();
  private void OnEnable() {
    _TargetEvent.Register(this);
  }
  
  private void OnDisable() {
    _TargetEvent.Unregister(this);
  }

  private void OnDestroy() {
    _TargetEvent.Unregister(this);
  }
  public void invoke(string dataString) {
    Actions.Invoke();
    invokeEvent(dataString);
  }
  void invokeEvent(string dataString) {
    for (int i = EventList.Count - 1; i >= 0; i--) {
      EventList[i].invoke(dataString);
    }
  }

  public void invoke() {
    Actions.Invoke();
    invokeEvent();
  }
  void invokeEvent() {
    for (int i = EventList.Count - 1; i >= 0; i--) {
      EventList[i].invoke();
    }
  }

  public void addEvent(EventBaseClass e) {
    if (e == null)
      return;
    if (EventList.Contains(e))
      return;
    EventList.Add(e);
  }
  public void RemoveEvent(EventBaseClass e) {
    if (e == null)
      return;
    if (!EventList.Contains(e))
      return;
    EventList.Remove(e);
  }
}

public delegate void EventDelegate(object[] o);
public class EventBaseClass {
  public EventDelegate OnEvent;
  public object[] o;
  public void invoke() {
    if (OnEvent != null)
      OnEvent(o);
  }
  public void invoke(string dataString) {
    //re make object[] with dataString at 0;
    if (o != null) {
      List<object> tmp = new List<object>(o.Length + 1);
      tmp.Add(dataString);
      foreach (var v in o) {
        tmp.Add(v);
      }
      o = tmp.ToArray();
    } else {
      o = new object[] { dataString };
    }

    if (OnEvent != null)
      OnEvent(o);
  }
}
