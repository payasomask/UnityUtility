using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ColorAnimate", menuName = "ScriptableObjectData/ColorAnimate")]
public class ColorAnimateData : AnimateBaseData {
  [SerializeField]
  public Color startColor;
  [SerializeField]
  public Color EndColor;
  [SerializeField]
  public FloatData duration;
  [SerializeField]
  public DG.Tweening.Ease easingType;
}
