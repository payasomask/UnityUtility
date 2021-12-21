using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FontManager {
  public void white(TextMeshPro text, bool originColor = true) {
    if (!originColor)
      text.color = Color.white;
    text.fontMaterial = GameManager.instance.assetBundleLoader.instanceMaterial("white");
  }
  public void black(TextMeshPro text, bool originColor = true) {
    if (!originColor)
      text.color = Color.black;
    text.fontMaterial = GameManager.instance.assetBundleLoader.instanceMaterial("black");
  }
}
