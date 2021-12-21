using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "New Vector3Animate", menuName = "ScriptableObjectData/Vector3Animate")]

public class Vector3AnimateData : AnimateBaseData {
  public enum VectorType {
    XY,
    XYZ
  }
  public enum TransformType {
    Local,
    World
  }
  [SerializeField]
  public VectorType ValueType;
  [SerializeField]
  public TransformType LoacationType;
  [SerializeField]
  public Vector3 startVector;
  [SerializeField]
  public Vector3 endVector;
  [SerializeField]
  public FloatData duration;
  [SerializeField]
  public Ease easingType;
}
