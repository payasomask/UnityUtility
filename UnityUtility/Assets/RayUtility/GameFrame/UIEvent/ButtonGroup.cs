using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonGroup : MonoBehaviour {
  Dictionary<string, data> data_dic = null;
  IDictionaryEnumerator dataEnumerator = null;
  Transform[] chirlTs = null;

  data _currentData = null;
  class data {
    public ITouchEventReceiver receiver = null;
    public GameObject go;
  }
  //T : ITouchEventReceiver
  public void Init<T>(string defaultReceiverName = "") {
    int chirlCount = ChirlCount();
    if (chirlCount == 0)
      return;
    data_dic = new Dictionary<string, data>(chirlCount);
    for (int i = 0; i < chirlCount; i++) {
      Transform t = transform.GetChild(i);
      ITouchEventReceiver r = t.GetComponent<ITouchEventReceiver>();
      if (r.GetType() == typeof(T)) {
        data_dic.Add(t.gameObject.name, new data() { receiver = r, go = t.gameObject });
      }
    }

    HideAllChirl();

    if (!string.IsNullOrEmpty(defaultReceiverName))
      ActiveReceiver(defaultReceiverName);
  }

  int ChirlCount() {
    return transform.childCount;
  }

  void HideAllChirl() {
    if (chirlTs == null) {
      Transform[] chirlt = transform.GetComponentsInChildren<Transform>();
      List<Transform> tmp = new List<Transform>(chirlt);
      //GetComponentsInChildren<Transform>()會把自己也算進去，把自己排除
      tmp.RemoveAt(0);
      chirlTs = tmp.ToArray();
    }
    foreach (var v in chirlTs) {
      v.gameObject.SetActive(false);
    }
  }

  public void ActiveReceiver(string ReceiverName) {
    data targetReceiver = null;
    foreach (var v in data_dic) {
      v.Value.go.SetActive(false);
      if (v.Key == ReceiverName) {
        targetReceiver = v.Value;
      }
    }
    if (targetReceiver == null)
      return;
    targetReceiver.go.SetActive(true);
  }

  //一直循環開啟下一個receiver
  public void cycle() {

    if (_currentData != null)
      _currentData.go.SetActive(false);

    if (dataEnumerator == null)
      dataEnumerator = data_dic.GetEnumerator();

    data currentdata = null;
    if (_currentData == null) {
      dataEnumerator.MoveNext();
      currentdata = ((KeyValuePair<string, data>)dataEnumerator.Current).Value;
      _currentData = currentdata;
      currentdata.go.SetActive(true);
      return;
    }

    if (!dataEnumerator.MoveNext()) {
      dataEnumerator.Reset();
      dataEnumerator.MoveNext();
      currentdata = ((KeyValuePair<string, data>)dataEnumerator.Current).Value;
      _currentData = currentdata;
      currentdata.go.SetActive(true);
      return;
    }

    currentdata = ((KeyValuePair<string, data>)dataEnumerator.Current).Value;
    _currentData = currentdata;
    currentdata.go.SetActive(true);
  }
}
