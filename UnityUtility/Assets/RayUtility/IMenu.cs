using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class IMenu : MonoBehaviour
{
  public virtual void init() {}
  public virtual void init(object[] _params) { }

  public abstract void OnUIEvent(string name, UIEventType type, object[] _params);

  //if IMenu use canvas, init it
  public virtual void initCanvas(string canvasPath = "Canvas") {
    //set canvas
    Transform t = transform.Find(canvasPath);
    if (t == null)
      return;
    UtilityHelper.setupCameraCanvas(t);
    return;
  }
  public virtual void initWorldCanvas(Vector3 defaultLocalPostion, string canvasPath = "Canvas") {
    //set canvas
    Transform t = transform.Find(canvasPath);
    if (t == null)
      return;
    UtilityHelper.setupWorldCanvas(t, defaultLocalPostion);
    return;
  }

  public virtual void dismiss() {
    gameObject.SetActive(false);
  }

  public virtual void show() {
    gameObject.SetActive(true);
  }
}
