using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchEventHandlerQueue : MonoBehaviour {
  public List<TouchEventHandler> queue = new List<TouchEventHandler>();
  bool dirty = false;

  GameObject focusing = null;
  GameObject highLightgo = null;
  bool focusing_broadcased = false;
  bool _HighLightEnable = false;
  public bool HighLightEnable {
    get { return _HighLightEnable; }
    set { _HighLightEnable = value; }
  }

  void reset_focusing() {
    focusing = null;
    focusing_broadcased = false;
  }

  void set_focusing(GameObject obj) {
    if (focusing == null) {
      if (obj != null) {
        focusing = obj;
        focusing_broadcased = false;
      }
    }
  }

  void broadcast_focusing() {
    if (focusing_broadcased == true)
      return;

    //broadcast focusing
    for (int i = 0; i < queue.Count; ++i) {
      queue[i].broadcast_focusing(focusing);
    }
    focusing_broadcased = true;
  }

  public TouchEventHandler getSpecifyPrioityHandler(int prioity) {
    for (int i = 0; i < queue.Count; i++) {
      if (queue[i].priority == prioity)
        return queue[i];
    }
    return null;
  }

  public void HighLight(Vector3 mousePostion) {
    if (!HighLightEnable)
      return;
    Vector3 pos = Camera.main.ScreenToWorldPoint(mousePostion);
    Vector2 touchPos = new Vector2(pos.x, pos.y);

    Collider2D hitObj = Physics2D.OverlapPoint(touchPos);
    ITouchEventReceiver currentReceiver = null;
    if (hitObj == null) {
      if (highLightgo == null)
        return;
      currentReceiver = highLightgo.GetComponent<ITouchEventReceiver>();
      currentReceiver.onHighLight(false);
      highLightgo = null;
      return;
    }

    currentReceiver = hitObj.gameObject.GetComponent<ITouchEventReceiver>();

    if (highLightgo != null) {
      ITouchEventReceiver preReceiver = highLightgo.GetComponent<ITouchEventReceiver>();
      if (preReceiver == currentReceiver) {
        //same ITouchEventReceiver return
        return;
      }
      preReceiver.onHighLight(false);
    }

    if (currentReceiver == null)
      return;
    highLightgo = hitObj.gameObject;
    currentReceiver.onHighLight(true);
  }

  // Use this for initialization
  void Start() {

  }

  // Update is called once per frame
  void Update() {

    HighLight(Input.mousePosition);

    if (Input.GetMouseButton(0)) {
      dirty = true;

      if (Input.GetMouseButtonDown(0)) {

        bool click_event_emitted = false;
        for (int i = 0; i < queue.Count; ++i) {
          click_event_emitted = queue[i].clearTouch(Input.mousePosition, click_event_emitted);
        }
        reset_focusing();

        bool touch_event_taken = false;
        for (int i = 0; i < queue.Count; ++i) {
          GameObject tmpFocusing = null;
          touch_event_taken = queue[i].checkTouch(Input.mousePosition, touch_event_taken, out tmpFocusing);
          set_focusing(tmpFocusing);
        }
      }

      for (int i = 0; i < queue.Count; ++i) {
        GameObject tmpFocusing = null;
        if (queue[i].deliverTouch(Input.mousePosition, out tmpFocusing)) {
          set_focusing(tmpFocusing);
          break;
        }
        set_focusing(tmpFocusing);
      }

      for (int i = 0; i < queue.Count; ++i) {
        GameObject tmpFocusing = null;
        if (queue[i].checkLongPress(Input.mousePosition, out tmpFocusing)) {
          set_focusing(tmpFocusing);
          break;
        }
        set_focusing(tmpFocusing);
      }
    } else {

      if (dirty) {
        bool click_event_emitted = false;
        for (int i = 0; i < queue.Count; ++i) {
          click_event_emitted = queue[i].clearTouch(Input.mousePosition, click_event_emitted);
        }
        reset_focusing();

        dirty = false;
      }
    }
    broadcast_focusing();
  }
}

public enum UIEventType {
  BUTTON,
  BUTTON_LONG_PRESS,
  TOUCH_ENTER,
  TOUCH_LEAVE,
  PAGEBUTTON,
  VERTICAL_SLIDER
}
