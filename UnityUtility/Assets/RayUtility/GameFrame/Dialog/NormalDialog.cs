using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NormalDialog : IDialogContext
{
  bool binited = false;
  string _content;
  string _title;
  TextMeshProUGUI contextText;
  public NormalDialog(string title, string content) {
    _content = content;
    _title = title;
  }
  public void UpdateContentText(string context) {
    contextText.text = context;
  }
  public bool dismiss() {
    //...
    return true;
  }

  public DialogEscapeType getEscapeType() {
    return DialogEscapeType.DISMISS;
  }

  public DialogType getType() {
    return DialogType.NORMAL;
  }

  public GameObject init(int dlgIdx) {
    GameObject root = GameManager.instance.assetBundleLoader.instancePrefab("normalDialog");
    Transform canvasT = root.transform.Find("Canvas");
    UtilityHelper.setupCameraCanvas(canvasT);
    UtilityHelper.setupWorldCanvas(canvasT,Vector3.zero);
    canvasT.transform.Find("Image/title").GetComponent<TextMeshProUGUI>().text = _title;
    contextText = canvasT.transform.Find("Image/content").GetComponent<TextMeshProUGUI>();
    contextText.text = _content;
    return root;
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
