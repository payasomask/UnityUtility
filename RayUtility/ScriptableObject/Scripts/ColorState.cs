using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ScriptableObjectData {
  [CreateAssetMenu(fileName = "New ColorState", menuName = "ScriptableObjectData/ColorState")]
  public class ColorState : ScriptableObject {
    public UnityEngine.UI.ColorBlock colorBlock = new ColorBlock();
  }
}

