using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ISceneManager : MonoBehaviour {
  protected GameObject root = null;
  protected Stack<IMenu> menuStack = new Stack<IMenu>();
  protected float menuDepth = 20;
  protected object[] preSceneParams = null;

  public abstract void OnUIEvent(string name, UIEventType type, object[] extraParams);
  public abstract void OnNetWorkResponse(string Header, object[] extraParams);


  public abstract void InitScene(object[] _params);

  protected void InitCanvas(string canvasPath = "Canvas") {
    //set canvas
    Transform t = transform.Find(canvasPath);
    if (t == null)
      return;
    Canvas c = t.GetComponent<Canvas>();
    c.renderMode = RenderMode.ScreenSpaceCamera;
    c.worldCamera = Camera.main;
    CanvasScaler cs = t.GetComponent<CanvasScaler>();
    cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
    cs.referenceResolution = new Vector2(UtilityHelper.ResolutionX, UtilityHelper.ResolutionY);
    cs.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
  }

  public abstract bool Inited();

  public virtual void addMenu(string prefabName, bool UnactivePreMenu = false, object[] _params = null) {
    GameObject menuGo = GameManager.instance.assetBundleLoader.instancePrefab(prefabName);
    menuGo.transform.SetParent(root.transform);
    menuGo.gameObject.transform.localPosition = new Vector3(0, 0, (menuStack.Count + 1) * -menuDepth);
    IMenu m = menuGo.GetComponent<IMenu>();
    if (menuStack.Count != 0 && UnactivePreMenu) {
      IMenu preMenu = menuStack.Peek();
      preMenu.dismiss();
    }

    menuStack.Push(m);
    //m.init()有可能會開啟另一個menu
    if (_params != null) m.init(_params);
    else m.init();

    IMenu lastMenu = menuStack.Peek();
    //確定了自己是最後一個menu的話，才會show()
    if(lastMenu == m)
      m.show();
  }
  public virtual void removeMenu() {
    if (menuStack.Count == 0)
      return;

    IMenu menu = menuStack.Pop();
    Destroy(menu.gameObject);

    if (menuStack.Count == 0)
      return;
    IMenu preMenu = menuStack.Peek();
    preMenu.show();
  }

  public bool HasMenu() {
    return menuStack.Count != 0;
  }

  public void setPreSceneParams(object[] o) {
    preSceneParams = o;
  }
}
