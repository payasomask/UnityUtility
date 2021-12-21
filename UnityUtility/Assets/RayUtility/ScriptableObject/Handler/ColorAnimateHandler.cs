using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class ColorAnimateHandler : AnimateBaseHandler {
  [SerializeField]
  bool StartFromStartColor;
  [SerializeField]
  UnityEngine.Object target;
  [SerializeField]
  ColorAnimateData colorAnimateData;
  //是否是順向
  bool Forward = true;
  protected override void OnEasingStart() {
    base.OnEasingStart();
    if (target == null)
      return;
    getTween();
  }

  Color getStartColor() {
    switch (colorAnimateData.mode) {
      case AnimateBaseData.Mode.Reverse:
        return colorAnimateData.EndColor;
      case AnimateBaseData.Mode.Pingpong:
        return Forward ? colorAnimateData.startColor : colorAnimateData.EndColor;
      default:
        return colorAnimateData.startColor;
    }
  }
  Color getEndColor() {
    switch (colorAnimateData.mode) {
      case AnimateBaseData.Mode.Reverse:
        return colorAnimateData.startColor;
      case AnimateBaseData.Mode.Pingpong:
        return Forward ? colorAnimateData.EndColor : colorAnimateData.startColor;
      default:
        return colorAnimateData.EndColor;
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
    switch (colorAnimateData.mode) {
      case AnimateBaseData.Mode.Pingpong:
        return Forward ? colorAnimateData.loop : true;
      default:
        return colorAnimateData.loop;
    }
  }

  public override Tween[] getTween() {
    if (target.GetType() == typeof(SpriteRenderer)) {
      SpriteRenderer s = (SpriteRenderer)target;
      if (StartFromStartColor)
        s.color = getStartColor();
      return new Tween[] { s.DOColor(getEndColor(), colorAnimateData.duration)
        .SetEase(colorAnimateData.easingType)
        .OnComplete(easingDone)};
    } else if (target.GetType() == typeof(Image)) {
      Image i = (Image)target;
      if (StartFromStartColor)
        i.color = getStartColor();
      return new Tween[] { i.DOColor(getEndColor(), colorAnimateData.duration)
        .SetEase(colorAnimateData.easingType)
        .OnComplete(easingDone)};
    } else if (target.GetType() == typeof(CanvasGroup)) {
      CanvasGroup c = (CanvasGroup)target;
      return new Tween[] {
       DOTween.To(x => c.alpha = x, getStartColor().a, getEndColor().a, colorAnimateData.duration).
        SetEase(colorAnimateData.easingType).
        OnComplete(easingDone)};
    } else {
      Debug.Log("858 - ColorAnimateHandler, target is not support type : " + target.GetType());
      OnDone(false);
    }
    return null;
  }
}
