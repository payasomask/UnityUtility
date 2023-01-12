using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorButtonHandler : ButtonBaseHandler
{
  [SerializeField]
  ScriptableObjectData.ColorState colorStateData;
  public override void OnSkinn() {
    base.OnSkinn();

    if (colorStateData == null)
      return;
    b.transition = UnityEngine.UI.Selectable.Transition.ColorTint;
    b.colors = colorStateData.colorBlock;
  }
}
