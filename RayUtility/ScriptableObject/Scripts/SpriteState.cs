using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ScriptableObjectData {
  [CreateAssetMenu(fileName = "New SrpiteState", menuName = "ScriptableObjectData/SpriteState")]
  public class SpriteState : ScriptableObject {
    public Sprite normalSpirte;
    public UnityEngine.UI.SpriteState spriteState;
  }
}

