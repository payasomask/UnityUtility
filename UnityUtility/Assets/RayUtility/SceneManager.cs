#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void CommonAction();
public class SceneManager : MonoBehaviour {
  AsyncOperation asyncLoad = null;
  //AsyncOperation asyncUnLoad = null;
  CommonAction onDone = null;
  Scene currentScene;
  string nextScene;
  int dialogIndex = -1;
  SceneTransitionDialog STD = null;
  object[] nextSceneInitParams = null;
  public void Init() {
    currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
  }

  public void preLoadScene(string scene, CommonAction onDone = null) {
    if (asyncLoad != null)
      return;

    this.onDone = onDone;
    this.nextScene = scene;

    STD = new SceneTransitionDialog(() => {
      LoadScene(this.nextScene);
    });
    dialogIndex = GameManager.instance.dialogManager.show(STD);
  }
  public void preLoadScene(string scene, object[] _params = null, CommonAction onDone = null) {
    if (asyncLoad != null)
      return;

    this.onDone = onDone;
    this.nextScene = scene;

    STD = new SceneTransitionDialog(() => {
      LoadScene(this.nextScene);
    });
    dialogIndex = GameManager.instance.dialogManager.show(STD);

    nextSceneInitParams = _params;
  }


  void LoadScene(string scene) {
#if UNITY_EDITOR
    string scenePath = GameManager.instance.assetBundleLoader.ScenePath(scene.ToString());
    asyncLoad = EditorSceneManager.LoadSceneAsyncInPlayMode(scenePath, new LoadSceneParameters() { loadSceneMode = LoadSceneMode.Single });
#elif !UNITY_EDITOR
    asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene.ToString(), new LoadSceneParameters() { loadSceneMode = LoadSceneMode.Single });
#endif
    asyncLoad.allowSceneActivation = false;
  }



  public void Update() {
    CheckSceneInited();

    if (asyncLoad == null)
      return;

    IsSceneLoaded();
  }

  void IsSceneLoaded() {

    //must be wait FadeInDone
    if (asyncLoad.isDone) {
      showScene();
      return;
    }

    if (asyncLoad.progress >= 0.9f && STD.IsFadeInDone) {
      //asyncLoad is done ,wait set asyncLoad allowSceneActivation = true;
      asyncLoad.allowSceneActivation = true;
    }

    return;
  }

  void InitSceneManager(Scene scene) {
    GameObject[] root = scene.GetRootGameObjects();
    for (int i = 0; i < root.Length; i++) {
      ISceneManager sceneManager = root[i].GetComponent<ISceneManager>();
      if (sceneManager != null) {
        Debug.Log("595 - init sceneManager : " + sceneManager.gameObject.name);
        GameManager.instance.SetCurrentScene(sceneManager);
        sceneManager.InitScene(nextSceneInitParams);
        sceneManager.setPreSceneParams(nextSceneInitParams);
        nextSceneInitParams = null;
      }
    }
  }

  void showScene() {
    GameManager.instance.dialogManager.dismiss(dialogIndex);
    currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
    Debug.Log("294 - preLoadScene Done : " + currentScene.name);
    InitSceneManager(currentScene);
    if (onDone != null) {
      GameManager.instance.assetBundleLoader.ReleaseCache();
      onDone();
      onDone = null;
    }
    asyncLoad = null;
  }

  void CheckSceneInited() {
    if (STD == null)
      return;

    if (GameManager.instance.currentScene == null)
      return;

    if(GameManager.instance.currentScene.name != nextScene.ToString()) {
      return;
    }

    //檢查場景啟動結束了沒
    if (!GameManager.instance.currentScene.Inited())
      return;

    GameManager.instance.dialogManager.dismiss(dialogIndex);
    STD = null;
  }
}
