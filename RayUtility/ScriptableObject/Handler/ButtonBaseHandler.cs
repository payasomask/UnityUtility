using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
[ExecuteInEditMode]
[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public abstract class ButtonBaseHandler : MonoBehaviour
{  
  protected Image i;
  protected Button b;
  void Oninit() {
    if (i == null) i = GetComponent<Image>();
    if (b == null) b = GetComponent<Button>();
  }

  public virtual void OnSkinn() { return; }

  private void Awake() {
    Oninit();
  }

  private void Update() {
    if (Application.isEditor)
      OnSkinn();
  }
}
