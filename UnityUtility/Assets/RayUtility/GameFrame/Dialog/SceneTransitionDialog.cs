using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SceneTransitionDialog : IDialogContext
{
  public SceneTransitionDialog(CommonAction onFadeInDone = null) {
    this.onFadeInDone = onFadeInDone;
  }
  bool binited = false;
  GameObject stmgr = null;
  SpriteRenderer sr = null;
  CommonAction onFadeInDone = null;
  float _FadeTime = 0.2f; 
  public float FadeTime {
    get { return _FadeTime; }
  }
  bool _IsFadeInDone = false;
  public bool IsFadeInDone {
    get { return _IsFadeInDone; }
  }
  public bool dismiss() {
    sr.DOColor(new Color(0.0f, 0.0f, 0.0f, 0.0f), _FadeTime);
    return true;
  }

  public DialogEscapeType getEscapeType() {
    return DialogEscapeType.DISMISS;
  }

  public DialogType getType() {
    return DialogType.DELAY_DESTROY;
  }

  public GameObject init(int dlgIdx) {
    stmgr = GameManager.instance.assetBundleLoader.instancePrefab("sceneTransitionDialog");
    sr = stmgr.transform.Find("bg").GetComponent<SpriteRenderer>();
    sr.DOColor(new Color(0.0f, 0.0f, 0.0f, 1.0f), _FadeTime).onComplete += ()=> {
      if (this.onFadeInDone != null) {
        this.onFadeInDone();
        this.onFadeInDone = null;
      }
      _IsFadeInDone = true;
    };
    return stmgr;
  }

  public bool inited() {
    return binited;
  }

  public DialogNetworkResponse setNetworkResponseEvent(string name, object payload) {
    return DialogNetworkResponse.PASS;
  }

  public DialogResponse setUIEvent(string name, UIEventType type, object[] extra_info) {
    return DialogResponse.PASS;
  }
}
