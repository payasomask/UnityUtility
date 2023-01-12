using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New RangeFloat", menuName = "ScriptableObjectData/RangeFloat")]
public class RangeFloatData : ScriptableObject
{
  [SerializeField]
  float Max = 1;
  [SerializeField]
  float Min = 0;
  [SerializeField]
  float _Value;
  public float Value {
    set { _Value = Mathf.Clamp(value, Min, Max); }
    get { return Mathf.Clamp(_Value,Min, Max); }
  }
  public void SetRange(float min, float max) {
    Max = max;
    Min = min;
    //setMax(max);
    //setMin(min);
  }
  public static implicit operator float(RangeFloatData fd) {
    if (fd == null)
      return float.NaN;
    return fd.Value;
  }
  public static implicit operator RangeFloatData(float f) {
    RangeFloatData fd = new RangeFloatData();
    fd.Value = f;
    return fd;
  }
  //void setMin(float min) {
  //  if (min > Max)
  //    min = Max;
  //  Min = min;
  //}
  //void setMax(float max) {
  //  if (max < Min)
  //    max = Min;
  //  Max = max;
  //}
}
