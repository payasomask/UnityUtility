using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PositionAnimateHandler : AnimateBaseHandler {
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
            Vector3 tmp = target.position;
            tmp.x = getStartVector().x;
            tmp.y = getStartVector().y;
            target.position = tmp;
          } else {
            target.position = getStartVector();
          }
          break;
        default:
          if (vectorAnimateData.ValueType == Vector3AnimateData.VectorType.XY) {
            Vector3 tmp = target.localPosition;
            tmp.x = getStartVector().x;
            tmp.y = getStartVector().y;
            target.localPosition = tmp;
          } else {
            target.localPosition = getStartVector();
          }
          break;
      }
    }

  }

  public override Tween[] getTween() {
    ApplyStartVector();
    List<Tween> list = new List<Tween>();
    switch (vectorAnimateData.ValueType) {
      case Vector3AnimateData.VectorType.XY:
        if (vectorAnimateData.LoacationType == Vector3AnimateData.TransformType.World) {
          list.Add(target.DOMoveX(getEndVector().x, vectorAnimateData.duration)
     .SetEase(vectorAnimateData.easingType));
          list.Add(target.DOMoveY(getEndVector().y, vectorAnimateData.duration)
     .SetEase(vectorAnimateData.easingType).OnComplete(easingDone));
        } else {
          list.Add(target.DOLocalMoveX(getEndVector().x, vectorAnimateData.duration)
     .SetEase(vectorAnimateData.easingType));
          list.Add(target.DOLocalMoveY(getEndVector().y, vectorAnimateData.duration)
     .SetEase(vectorAnimateData.easingType).OnComplete(easingDone));
        }
        break;
      default:
        if (vectorAnimateData.LoacationType == Vector3AnimateData.TransformType.World) {
          list.Add(target.DOMove(getEndVector(), vectorAnimateData.duration)
     .SetEase(vectorAnimateData.easingType).OnComplete(easingDone));
        } else {
          list.Add(target.DOLocalMove(getEndVector(), vectorAnimateData.duration)
     .SetEase(vectorAnimateData.easingType).OnComplete(easingDone));
        }
        break;
    }
    return list.ToArray();
  }
}
