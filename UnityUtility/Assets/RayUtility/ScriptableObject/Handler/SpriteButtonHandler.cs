using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteButtonHandler : ButtonBaseHandler
{
  [SerializeField]
  ScriptableObjectData.SpriteState spriteStateData;
  public override void OnSkinn() {
    base.OnSkinn();
    b.transition = UnityEngine.UI.Selectable.Transition.SpriteSwap;
    b.targetGraphic = i;

    b.image.sprite = spriteStateData.normalSpirte;
    b.spriteState = spriteStateData.spriteState;
  }
}
