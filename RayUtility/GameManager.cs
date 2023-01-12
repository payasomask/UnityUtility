using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Newtonsoft.Json;

public class GameManager : MonoBehaviour {

  static GameManager _instance = null;
  public static GameManager instance {
    get {
      return _instance;
    }
  }

  public JsonLoader jsonLoader = null;
  public AssetBundleLoader assetBundleLoader = null;
  [HideInInspector]
  public ISceneManager currentScene = null;
  public AudioManager audioManager = null;
  [HideInInspector]
  public SceneManager sceneManager = null;
  [HideInInspector]
  public TouchEventHandlerQueue touchEventHandlerQueue = null;
  public IUIDialog dialogManager = null;
  public FontManager fontManager = null;
  [HideInInspector]
  public INativeManager nativeManager = null;
  [SerializeField]
  ObjectsData storysData;
  [SerializeField]
  ObjectsData characterData;
  [SerializeField]
  EventBaseData StorySyncEventData;
  private void Awake() {
    if (_instance != null)
      return;
    _instance = this;
    _instance.Init();
  }
  void Init() {
    UnityEngine.Object.DontDestroyOnLoad(this.gameObject);

    Application.targetFrameRate = 60;

    jsonLoader = new JsonLoader();
    jsonLoader.Init();

    assetBundleLoader = new AssetBundleLoader();
    assetBundleLoader.Init();

    DOTween.Init();

    audioManager = new AudioManager();
    audioManager.Init(this);

    GameObject mainC = assetBundleLoader.instancePrefab("Main Camera");
    mainC.transform.SetParent(gameObject.transform);

    GameObject tmpGo = new GameObject("SceneManager");
    tmpGo.transform.SetParent(transform);
    sceneManager = tmpGo.AddComponent<SceneManager>();
    sceneManager.Init();

    tmpGo = new GameObject("TouchEventHandlerQueue");
    tmpGo.transform.SetParent(transform);
    touchEventHandlerQueue = tmpGo.AddComponent<TouchEventHandlerQueue>();
    touchEventHandlerQueue.HighLightEnable = false;

    tmpGo = new GameObject("DialogManager");
    tmpGo.transform.SetParent(transform);
    TouchEventHandler teh = tmpGo.AddComponent<TouchEventHandler>();
    dialogManager = tmpGo.AddComponent<UIDialog>();
    teh.priority = -1;

    fontManager = new FontManager();

    tmpGo = new GameObject("NativeManager");
    tmpGo.transform.SetParent(transform);
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
    nativeManager = tmpGo.AddComponent<EditorNative>();
#elif UNITY_ANDROID && !UNITY_EDITOR
    nativeManager = tmpGo.AddComponent<AndroidNative>();
#endif
    nativeManager.init();

    //iapManager = new IAPManager();

    //networkRequestManager = gameObject.AddComponent<NetworkRequestManager>();

    //tokenManager = GetComponent<TokenManager>();

    NextScene();
  }

  void NextScene() {
    //GameManager.instance.sceneManager.preLoadScene(Unlight.Scene.Login, null);
  }

  public void SetCurrentScene(ISceneManager manager) {
    currentScene = manager;
  }

  public void setUIEvent(string name, UIEventType type, object[] _params) {

    //if (networkRequestManager.isBusy()) {
    //  Debug.Log("340 - networkRequestManager has task, ignore UIEvent");
    //  return;
    //}

    if (dialogManager.setUIEvent(name, type, _params)) {
      return;
    }
    if (currentScene == null)
      return;
    currentScene.OnUIEvent(name, type, _params);
  }
  public void OnNetWorkResponse(string header, object[] _params) {
    if (currentScene == null)
      return;
    currentScene.OnNetWorkResponse(header, _params);

    return;
  }

  public void reStartGame() {
    NextScene();
  }
}
