using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotateAnimateHandler : AnimateBaseHandler {
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
    ApplyStartVector();

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
      switch (vectorAnimateData.LoacationType) {
        case Vector3AnimateData.TransformType.World:
          if (vectorAnimateData.ValueType == Vector3AnimateData.VectorType.XY) {
            Vector3 tmp = target.rotation.eulerAngles;
            tmp.x = getStartVector().x;
            tmp.y = getStartVector().y;
            target.rotation = Quaternion.Euler(tmp);
          } else {
            target.rotation = Quaternion.Euler(getStartVector());
          }
          break;
        default:
          if (vectorAnimateData.ValueType == Vector3AnimateData.VectorType.XY) {
            Vector3 tmp = target.localPosition;
            tmp.x = getStartVector().x;
            tmp.y = getStartVector().y;
            target.localRotation = Quaternion.Euler(tmp);
          } else {
            target.localRotation = Quaternion.Euler(getStartVector());
          }
          break;
      }
    }

  }

  public override Tween[] getTween() {
    ApplyStartVector();
    List<Tween> list = new List<Tween>();
    if (vectorAnimateData.LoacationType == Vector3AnimateData.TransformType.World) {
      list.Add(target.DORotate(getEndVector(), vectorAnimateData.duration)
 .SetEase(vectorAnimateData.easingType));
    } else {
      list.Add(target.DOLocalRotate(getEndVector(), vectorAnimateData.duration)
 .SetEase(vectorAnimateData.easingType));
    }
    return list.ToArray();
  }
}
