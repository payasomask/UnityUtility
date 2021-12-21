using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScaleAnimateHandler : AnimateBaseHandler {
  [SerializeField]
  bool StartFromStartVector;
  [SerializeField]
  Transform target;
  [SerializeField]
  Vector3AnimateData vectorAnimateData;
  bool Forward = true;
  protected override void OnEasingStart() {
    base.OnEasingStart();
    if (target == null)
      return;

    //set start
    //ApplyStartVector();

    //set easing
    getTween();
  }

  Vector3 getStartVector() {
    switch (vectorAnimateData.mode) {
      case AnimateBaseData.Mode.Reverse:
        return vectorAnimateData.endVector;
      case AnimateBaseData.Mode.Pingpong:
        return Forward ? vectorAnimateData.startVector : vectorAnimateData.endVector;
      default:
        return vectorAnimateData.startVector;
    }
  }
  Vector3 getEndVector() {
    switch (vectorAnimateData.mode) {
      case AnimateBaseData.Mode.Reverse:
        return vectorAnimateData.startVector;
      case AnimateBaseData.Mode.Pingpong:
        return Forward ? vectorAnimateData.endVector : vectorAnimateData.startVector;
      default:
        return vectorAnimateData.endVector;
    }
  }
  void ForwardReverse() {
    Forward = !Forward;
  }

  void easingDone() {
    ForwardReverse();
    OnDone(getLoop());
  }

  bool getLoop() {
    switch (vectorAnimateData.mode) {
      case AnimateBaseData.Mode.Pingpong:
        return Forward ? vectorAnimateData.loop : true;
      default:
        return vectorAnimateData.loop;
    }
  }

  void ApplyStartVector() {
    if (StartFromStartVector) {
      if (vectorAnimateData.ValueType == Vector3AnimateData.VectorType.XY) {
        Vector3 tmp = target.localScale;
        tmp.x = getStartVector().x;
        tmp.y = getStartVector().y;
        target.localScale = tmp;
      } else {
        target.localScale = getStartVector();
      }
    }

  }

  public override Tween[] getTween() {
    ApplyStartVector();
    List<Tween> list = new List<Tween>();
    switch (vectorAnimateData.ValueType) {
      case Vector3AnimateData.VectorType.XY:
          list.Add(target.DOScaleX(getEndVector().x, vectorAnimateData.duration)
     .SetEase(vectorAnimateData.easingType));
          list.Add(target.DOScaleY(getEndVector().y, vectorAnimateData.duration)
     .SetEase(vectorAnimateData.easingType).OnComplete(easingDone));
        break;
      default:
          list.Add(target.DOScale(getEndVector(), vectorAnimateData.duration)
     .SetEase(vectorAnimateData.easingType).OnComplete(easingDone));
        break;
    }
    return list.ToArray();
  }
}
