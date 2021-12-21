using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Float", menuName = "ScriptableObjectData/Float")]
public class FloatData : ScriptableObject
{
  [SerializeField]
  public float Value;
  public static implicit operator float(FloatData fd) {
    if (fd == null)
      return float.NaN;
    return fd.Value;
  }
  public static implicit operator FloatData(float f) {
    FloatData fd = new FloatData();
    fd.Value = f;
    return fd;
  }
}
