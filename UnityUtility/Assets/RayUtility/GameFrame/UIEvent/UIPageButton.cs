using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class UIPageButton : MonoBehaviour, ITouchEventReceiver {

  SpriteRenderer sr = null;
  Sprite normalState = null;
  Sprite hoverState = null;
  Sprite disabledState = null;
  Sprite highLightState = null;

  public string hoverState_atlasName = null;
  public string hoverState_spriteName = null; //spritename

  public string disabledState_atlasName = null;
  public string disabledState_spriteName = null;

  public string highLightState_atlasName = null;
  public string highLightState_spriteName = null;

  //是否傳遞訊息 (此 ui 底下的 ui controller 不對 touch event 有反應)
  public bool passTouchEvent = true;
  public bool passClickEvent = false;

  //此按鈕 LONG_PRESS 的時間
  public float longPressDuration = 0.7f;

  [SerializeField]
  Color normalColor = UtilityHelper.ToColor("333333");
  [SerializeField]
  Color disabledColor = UtilityHelper.ToColor("535353");

  TMP_Text[] textChildrens = null;

  object[] clickParams = null;
  // Use this for initialization
  void Start() {
  }

  bool focusing = true;
  public void setParams(object[] _params) {
    clickParams = _params;
  }
  public void OnBroadCast(string name, float Startz) {
    if (sr == null)
      sr = GetComponent<SpriteRenderer>();

    if (textChildrens == null)
      textChildrens = transform.GetComponentsInChildren<TMP_Text>();

    if (normalState == null) {
      normalState = sr.sprite;
      setChildrensColor(normalColor);
    }

    if (name != gameObject.name) {
      transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, Startz);
      if (disabledState == null) {
        disabledState = GameManager.instance.assetBundleLoader.instanceSprite(disabledState_atlasName, disabledState_spriteName);
        sr.sprite = disabledState;
        setChildrensColor(disabledColor);
      } else {
        sr.sprite = disabledState;
        setChildrensColor(disabledColor);
      }
      return;
    }

    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, Startz - 5.0f);//比其他按鈕還前面5.0f
    sr.sprite = normalState;
    setChildrensColor(normalColor);

    //if pagebutton been click update unhighLightstate to current sr.sprite
    unhighLightstate = sr.sprite;
  }

  public bool getStopPassLongPressEvent(Vector3 pt) {
    return !passClickEvent;
  }

  public float getLongPressDuration() {
    return longPressDuration;
  }

  public bool OnClick(Vector2 pt) {

    if (focusing)
      GameManager.instance.setUIEvent(gameObject.name, UIEventType.PAGEBUTTON, clickParams);

    return !passClickEvent;
  }

  public void OnLongPress(Vector2 pt, out bool request_focusing) {
    request_focusing = true;

    if (focusing)
      GameManager.instance.setUIEvent(gameObject.name, UIEventType.BUTTON_LONG_PRESS, null);
  }

  public bool OnTouchMove(Vector2 curr_touch, Vector2 displacement, out bool request_focusing) {
    request_focusing = false;
    return !passTouchEvent;
  }

  bool isTouchEnter = false;
  public bool OnTouchEnter(Vector2 touchPosi, out bool request_focusing) {
    request_focusing = false;
    GameManager.instance.setUIEvent(gameObject.name, UIEventType.TOUCH_ENTER, null);
    isTouchEnter = true;

    if (sr == null) {
      sr = GetComponent<SpriteRenderer>();
    }

    if (sr == null) {
      return !passTouchEvent;
    }

    return !passTouchEvent;
  }

  public void OnTouchLeave() {

    //也不用恢復成normalstate
    //if (sr != null && normalState != null)
    //  sr.sprite = normalState;

    if (isTouchEnter) {
      GameManager.instance.setUIEvent(gameObject.name, UIEventType.TOUCH_LEAVE, null);
      isTouchEnter = false;
    }
  }

  public void OnFocusRequested(GameObject requested_obj) {
    if (requested_obj == this.gameObject) {
      return;
    }

    if (requested_obj == null) {
      focusing = true;
      return;
    }

    //deactivate onclick/onlongpress function
    focusing = false;

    //cancel hover status
    OnTouchLeave();
  }
  Sprite unhighLightstate = null;
  public void onHighLight(bool mouseIn) {
    if (highLightState == null)
      highLightState = GameManager.instance.assetBundleLoader.instanceSprite(highLightState_atlasName, highLightState_spriteName);

    if (mouseIn) {
      unhighLightstate = sr.sprite;
    }

    sr.sprite = mouseIn ? highLightState : unhighLightstate;
  }

  public GameObject GetGameObject() {
    return gameObject;
  }

  void setChildrensColor(Color c) {
    if (textChildrens == null)
      return;
    foreach(var v in textChildrens) {
      v.color = c;
    }
  }
}
