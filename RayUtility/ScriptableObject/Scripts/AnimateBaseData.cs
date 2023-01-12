using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimateBaseData : ScriptableObject
{
  [SerializeField]
  public bool loop;
  public enum Mode {
    Default,
    Pingpong,
    Reverse,
  }
  [SerializeField]
  public Mode mode;
}
