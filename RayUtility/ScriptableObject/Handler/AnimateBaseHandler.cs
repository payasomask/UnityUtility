using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimateBaseHandler : MonoBehaviour {
  [SerializeField]
  bool EasingOnStart = false;
  protected bool easing = false;
  CommonAction onDone;
  public void OnStart(CommonAction onDone = null) {
    if (isEasing())
      return;
    this.onDone = onDone;
    easing = true;
    OnEasingStart();
  }
  public void OnEnd(CommonAction onDone = null) {
    if (isEasing())
      return;
    this.onDone = onDone;
    easing = true;
    OnEasingEnd();
  }
  public bool isEasing() {
    return easing;
  }
  protected virtual void OnEasingStart() { }
  protected virtual void OnEasingEnd() { }

  public virtual DG.Tweening.Tween[] getTween() { return null; }

  protected void OnDone(bool loop) {
    //if loop, never trigger onDone delegate
    //and dont stop easing forever
    if (loop) {
      OnEasingStart();
      return;
    }

    if (onDone != null)
      onDone();
    onDone = null;
    easing = false;
  }
  private void Start() {
    if (EasingOnStart)
      OnStart(null);
  }
}
