using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITextMeshProButton : MonoBehaviour, ITouchEventReceiver
{
  TextMeshPro text = null;
  [SerializeField]
  Color normalColor = Color.white;
  [SerializeField]
  Color hoverColor = UtilityHelper.ToColor("C8C8C8");
  [SerializeField]
  Color disabledColor = UtilityHelper.ToColor("C8C8C899");
  [SerializeField]
  Color highLightColor = UtilityHelper.ToColor("F5F5F5");

  //是否傳遞訊息 (此 ui 底下的 ui controller 不對 touch event 有反應)
  public bool passTouchEvent = true;
  public bool passClickEvent = false;

  //此按鈕 LONG_PRESS 的時間
  public float longPressDuration = 0.7f;

  public object[] clickParams = null;

  // Use this for initialization
  void Start() {
    //setEnabled(btn_enabled,true);
  }

  bool focusing = true;

  // Update is called once per frame
  void Update() {

  }
  public bool btn_enabled = true;
  bool Not_enabled_can_OnClick = false;
  public void setEnabled(bool enable, bool Emit_OnClick_When_Disabled = false) {
    btn_enabled = enable;
    Not_enabled_can_OnClick = Emit_OnClick_When_Disabled;
    if (text == null) {
      text = GetComponent<TextMeshPro>();
    }

    //if need stringGo
    //createStringGo();

    if (enable == true) {
      if (isTouchEnter == false)
        text.color = normalColor;
    } else {
      text.color = disabledColor;
    }
  }

  public void setParams(object[] _params) {
    clickParams = _params;
  }

  public bool getStopPassLongPressEvent(Vector3 pt) {
    return !passClickEvent;
  }

  public float getLongPressDuration() {
    return longPressDuration;
  }

  public bool OnClick(Vector2 pt) {

    if (btn_enabled == false && Not_enabled_can_OnClick == false)
      return !passClickEvent;

    setEnabled(!btn_enabled, true);

    if (focusing) {
      GameManager.instance.audioManager.PlayEffect("ButtonClick2");
      GameManager.instance.setUIEvent(gameObject.name, UIEventType.BUTTON, clickParams);
    }

    return !passClickEvent;
  }

  public void OnLongPress(Vector2 pt, out bool request_focusing) {
    request_focusing = true;
    if (btn_enabled == false)
      return;

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

    if (btn_enabled == false)
      return !passTouchEvent;

    GameManager.instance.setUIEvent(gameObject.name, UIEventType.TOUCH_ENTER, null);

    isTouchEnter = true;

    if (text == null) {
      text = GetComponent<TextMeshPro>();
    }

    if (text == null) {
      return !passTouchEvent;
    }

    text.color = hoverColor;

    return !passTouchEvent;
  }

  public void OnTouchLeave() {
    if (btn_enabled == false)
      return;

    if (text != null && normalColor != null)
      text.color = normalColor;

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

  public void onHighLight(bool mouseIn) {
    text.color = mouseIn ? highLightColor : normalColor;
  }

  public GameObject GetGameObject() {
    return gameObject;
  }

  //void createStringGo(){
  //  if (button_label_str != null){
  //    string_sr.sprite = btn_enabled ? button_label_str[0] : button_label_str[2] == null ? button_label_str[0] : button_label_str[2];
  //    return;
  //  }

  //  if (sr.sprite == null)//按鈕沒有圖就沒有必要生字
  //    return ;

  //  if (hoverState_atlasName == null && disabledState_atlasName == null)
  //    return;

  //  GameObject go = new GameObject("string_sr_go");
  //  string_sr =  go.AddComponent<SpriteRenderer>();
  //  string_sr.transform.SetParent(gameObject.transform, false);
  //  string_sr.transform.localPosition = Vector3.forward * -1.0f;
  //  string_sr.sortingLayerName = sr.sortingLayerName;
  //  string_sr.gameObject.layer = gameObject.layer;

  //  button_label_str =new Sprite[3];
  //  button_label_str[0] =find_sprite(sr.sprite.name);
  //  button_label_str[1] =find_sprite(hoverState_spriteName);
  //  button_label_str[2] =find_sprite(disabledState_spriteName);

  //  //如果是btn_enabled false 檢查有沒有button_label_str[2] ， 不能讓字是空的 先用button_label_str[0] 來顯示
  //  string_sr.sprite =btn_enabled?button_label_str[0]: button_label_str[2] == null ? button_label_str[0] : button_label_str[2];

  //}

  //Sprite find_sprite(string sprite_name) {
  //  if (sprite_name == null)
  //    return null;

  //  if (cxt == null) {
  //    MainLogic ml = GameObject.FindGameObjectWithTag("MainLogic").GetComponent<MainLogic>();
  //    if (ml != null) {
  //      cxt = ml.getContext();
  //    } else {
  //      return null;
  //    }
  //  }

  //  string lang = cxt.mPPM.Language.ToLower();
  //  List<string> atlas_list = cxt.mABL.get_atlas_list("multi_bt_string_" + lang, sprite_name);
  //  if (atlas_list.Count > 0)
  //    return cxt.mABL.InstantiateSprite(atlas_list[0], sprite_name);
  //  return null;
  //}
}
