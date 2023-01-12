using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateHandler : AnimateBaseHandler
{
  [SerializeField]
  AnimateBaseHandler[] AnimateInHandlers;
  [SerializeField]
  AnimateBaseHandler[] AnimateOutHandlers;
  //目前正在處理表演的handlers數量
  int handlerCount = 0;
  enum State {
    IN,
    IDLE,
    OUT,
  }
  State mState = State.IDLE;
  protected override void OnEasingStart() {
    base.OnEasingStart();
    In();
  }
  protected override void OnEasingEnd() {
    base.OnEasingEnd();
    Out();
  }
  void In() {
    if (mState == State.IN)
      return;
    if (AnimateInHandlers == null) {
      InDone();
      return;
    }
    if (AnimateInHandlers.Length == 0) {
      InDone();
      return;
    }
    handlerCount = AnimateInHandlers.Length;
    foreach (var v in AnimateInHandlers) {
      v.OnStart(OnHandlerDone);
    }
    mState = State.IN;
  }
  void Out() {
    if (mState == State.OUT)
      return;
    if (AnimateOutHandlers == null) {
      OutDone();
      return;
    }
    if (AnimateOutHandlers.Length == 0) {
      OutDone();
      return;
    }
    handlerCount = AnimateOutHandlers.Length;
    foreach (var v in AnimateOutHandlers) {
      v.OnStart(OnHandlerDone);
    }
    mState = State.OUT;
  }

  void InDone() {
    mState = State.IDLE;
  }
  void OutDone() {
    mState = State.IDLE;
  }

  private void OnHandlerDone() {
    handlerCount--;
    if (handlerCount <= 0) {
      Debug.Log("872 - AnimateHandler on : " + gameObject.name + "all handlers done");
      OnDone(false);
      mState = State.IDLE;
    }
  }

  public DG.Tweening.Tween[] getInTweens() {
    if (AnimateInHandlers == null)
      return new DG.Tweening.Tween[] { };
    List<DG.Tweening.Tween> tmp = new List<DG.Tweening.Tween>();
    foreach(var v in AnimateInHandlers) {
      tmp.AddRange(v.getTween());
    }
    return tmp.ToArray();
  }
  public DG.Tweening.Tween[] getOutTweens() {
    if (AnimateOutHandlers == null)
      return new DG.Tweening.Tween[] { };
    List<DG.Tweening.Tween> tmp = new List<DG.Tweening.Tween>();
    foreach (var v in AnimateOutHandlers) {
      tmp.AddRange(v.getTween());
    }
    return tmp.ToArray();
  }
  //private void Update() {
  //  if (Input.GetKeyUp(KeyCode.S)) {
  //    this.OnStart();
  //  }
  //  if (Input.GetKeyUp(KeyCode.A)) {
  //    this.OnEnd();
  //  }
  //}
}
