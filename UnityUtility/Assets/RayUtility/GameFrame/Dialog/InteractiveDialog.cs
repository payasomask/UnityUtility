using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractiveDialog : IDialogContext
{

  string BoardName = "InteractiveDialog";
  float button_interval = 75.0f;
  public string message;
  string[] buttons_name;
  public bool binited = false;
  GameObject stmgr = null;
  //SpriteRenderer bg_sr = null;
  //float minWidth = 970.0f;
  //float minHeight = 363.4f;
  //float AddHeight = 40.0f;
  //第一行的字距離BG上緣距離
  //float first_line_offsetY = 70.0f;
  //總共有幾行(大約)
  //int Lines = 0;
  //BT距離BG下緣距離
  //float Bt_offsetY = 87.28f;
  //float topofbg_offset = -16.3f;
  CommonAction[] pHandlers;
  public InteractiveDialog(string msg, string[] buttons_name = null, CommonAction[] Handlers = null)
  {
    this.buttons_name = buttons_name;
    message = msg;
    pHandlers = Handlers;
    //每40個英文字元增加一個height
    //Lines = (message.Length / 40);
    //每18個中文增加一個height
    //Lines = (message.Length / 18);
  }

  public bool inited()
  {
    return binited;
  }

  void callback(object param = null)
  {
    if(param == null)
      return;
    else
    {
      int bt_index = (int)param;
      if (pHandlers != null)
        if(pHandlers[bt_index] != null)
          pHandlers[bt_index]();
    }
  }

  public bool dismiss(){

    //被外部呼叫 dismiss
    //...

    GameObject.Destroy(stmgr);

    return true;
  }

  public DialogType getType()
  {
    return DialogType.NORMAL;
  }
  int dialogindex = 0;
  public GameObject init(int dlgIdx)
  {
    ////////
    stmgr = GameManager.instance.assetBundleLoader.instancePrefab("InteractiveDialog");
    stmgr.name = "InteractiveDialog";

    int interval = 0;
    dialogindex = dlgIdx;

    Canvas textgo = stmgr.transform.Find("Scaleble_text").GetComponent<Canvas>();
    VerticalLayoutGroup VLG = textgo.transform.Find("Panel").GetComponent<VerticalLayoutGroup>();
    TextMeshProUGUI textmesh = textgo.transform.Find("Panel/content").GetComponent<TextMeshProUGUI>();
    textmesh.text = message;

    //每多一行bg +一個高度
    Image bg_sr = stmgr.transform.Find("Scaleble_text/Panel").GetComponent<Image>();
    //float currentH = bg_sr.sprite.bounds.size.y;
    float buttontotalX = 0;

    if (buttons_name != null && buttons_name.Length > 0) {
      //在字數不夠的情況最大值的左右寬度是300
      //那何謂字數沖不充足，那要以總按鈕的數量而定，而按鈕的數量
      //所以必須先計算出總按鈕的sprite寬度 + 按鈕尖閣寬度
      int buttonintervalcount = buttons_name.Length > 0 ? buttons_name.Length - 1 : 0;
      //calculate Bt posY
      //改用自定義高度基值為-41.06再根據字高調整高度
      //一個中文字36寬47.88高
      float bt_Y = -41.06f - textmesh.preferredHeight * 0.5f;

      //由於不確定字數 關鍵就是我能不能鎖住text的寬高，VerticalLayoutGroup 限制childControlWidth的情況下不能更改
      //所以我必須先判斷字數是否有超過我的最大允許範圍，最大容許範圍944.79/430.95 
      //並且去關閉childControlWidth/childControlHeight，
      //關閉有兩個作用，一，讓dialog長出來的寬高會是我設定最大容許範圍的寬高
      //二，讓textmesh的字overflow可以正常運作
      if (textmesh.preferredWidth > 944.79f) {
        VLG.childControlWidth = false;
      }
      if (textmesh.preferredHeight > 430.95f) {
        //但是這邊會讓按鈕的Y位置有落差，因為childControlHeight的情況下textmesh的width / height會完美的符合字的preferredwidth/height
        //所以這裡改用最大容許範圍的430.95f作為計算
        VLG.childControlHeight = false;
        bt_Y = -41.06f - 430.95f * 0.5f;
      }
      //最後不管怎樣，複寫設定字的寬高最大容許範圍944.79/430.95
      textmesh.rectTransform.sizeDelta = new Vector2(944.79f, 430.95f);

      //Z值由canvas物件為基準，因為此階段不知道stmgrZ值會被安排到多少
      float bt_Z = -10.0f;
      //X值目前不打算隨著panel增長

      //基數 偶數不同排列方式
      if (buttons_name.Length % 2 == 0) {
        for (int i = 0; i < buttons_name.Length; i++) {
          GameObject bt = GameManager.instance.assetBundleLoader.instancePrefab(buttons_name[i]);
          bt.name = "Interactive_" + buttons_name[i] + "_" + dlgIdx + "_" + i;
          bt.transform.SetParent(textgo.transform);
          bt.transform.localScale = Vector3.one;
          buttontotalX += bt.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
          if (i % 2 == 0) {
            interval++;
            bt.transform.localPosition = new Vector3(-button_interval * (interval + 1) * 0.5f, bt_Y, bt_Z);
          } else {
            bt.transform.localPosition = new Vector3(+button_interval * (interval + 1) * 0.5f, bt_Y, bt_Z);
          }
        }
      } else {
        for (int i = 0; i < buttons_name.Length; i++) {
          GameObject bt = GameManager.instance.assetBundleLoader.instancePrefab(buttons_name[i]);
          bt.name = "Interactive_" + buttons_name[i] + "_" + dlgIdx + "_" + i;
          bt.transform.SetParent(textgo.transform);
          bt.transform.localScale = Vector3.one;
          buttontotalX += bt.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
          if (i % 2 == 0) {
            bt.transform.localPosition = new Vector3(-button_interval * interval, bt_Y, bt_Z);
            interval++;
          } else {
            bt.transform.localPosition = new Vector3(+button_interval * interval, bt_Y, bt_Z);
          }
        }

      }



      buttontotalX += buttonintervalcount * button_interval;

    }
    //重新計算nine slice的左右寬度
    //如果字寬是0 那就 300
    //50為反向lerp 確保字寬大於 按鈕總寬 的話 也會有50的左右寬度
    //確保字寬小於 按鈕總寬 的話 最小的時候寬度是300
    float lerp = UnityEngine.Mathf.Clamp01((buttontotalX - textmesh.preferredWidth) / buttontotalX);
    int panelwidth = (int)(lerp * 200.0f);
    VLG.padding.left = panelwidth+(int)(100 * 1.0f - lerp);
    VLG.padding.right = panelwidth+ (int)(100 * 1.0f - lerp);


    binited = true;

    CameraExtensions.CanvasScreenSpaceCamera(textgo);
    return stmgr;
  }

  public DialogResponse setUIEvent(string name, UIEventType type, object[] extra_info){
    //攔截Interactive dlg 訊息
    if (name.Contains("Interactive_") && type ==UIEventType.BUTTON)
    {
      string[] token = name.Split('_');
      if (token.Length == 4)
      {
        int outDlgIdx = -1;
        int outbtindex = -1;
        int.TryParse(token[2], out outDlgIdx);
        int.TryParse(token[3], out outbtindex);

        //若是同意等的 event 則呼叫對應之 callback function
        callback(outbtindex);
      }

      //因為是 taken and dismiss, 所以外部的 dialog manager 之後會呼叫本 dialog 的 dismiss function
      return DialogResponse.TAKEN_AND_DISMISS;
    }

    //ui event 不屬於本 dialogue, pass 給其它物件
    return DialogResponse.PASS;
  }

  public DialogNetworkResponse setNetworkResponseEvent(string name, object payload){
    return DialogNetworkResponse.PASS;
  }

  public DialogEscapeType getEscapeType(){
    return DialogEscapeType.DISMISS;
  }
}
