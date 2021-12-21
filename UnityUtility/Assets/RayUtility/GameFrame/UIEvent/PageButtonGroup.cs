using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageButtonGroup : MonoBehaviour {//負責管理同一個group的UIPAGEBUTTON的顯示(更換sprite而已)
  
  bool inited = false;
  List<UIPageButton> pageList = new List<UIPageButton>();
  float startZ;//在初始時期檢查所有UIPageButton的深度值...以方便在切換page的時後能夠正確的layout所有UIPageButton的深度值...Localposition
  public void Init()
  {
    if (inited == true)
      return;

    UIPageButton[] pbs = GetComponentsInChildren<UIPageButton>();
    foreach (var v in pbs){
      if (v.gameObject.transform.localPosition.z < startZ)//取得最靠近的攝影機的pageButton Z...
        startZ = v.gameObject.transform.localPosition.z;
      Add(v);
    }

    //BroadCast(defualtpagename);

    inited = true; 
  }
  public void Add(UIPageButton pb)
  {
    pageList.Add(pb);
  }
  public void BroadCast(string name)
  {
    if (inited == false)
      return;

    for (int i = 0; i < pageList.Count; i++)
      pageList[i].OnBroadCast(name,startZ);
  }
}
