#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class AssetBundleLoader {

  Dictionary<string, string> SpriteAssetDic = new Dictionary<string, string>();
  Dictionary<string, string> PrefabAssetDic = new Dictionary<string, string>();
  Dictionary<string, string> AudioClipAssetDic = new Dictionary<string, string>();
  Dictionary<string, string> SceneAssetDic = new Dictionary<string, string>();
  Dictionary<string, string> MaterialAssetDic = new Dictionary<string, string>();

  Dictionary<string, Sprite> SpriteCacheDic = new Dictionary<string, Sprite>();
  Dictionary<string, Sprite[]> SpriteSheetCacheDic = new Dictionary<string, Sprite[]>();
  Dictionary<string, GameObject> PrefabCacheDic = new Dictionary<string, GameObject>();
  Dictionary<string, AudioClip> AudioClipCacheDic = new Dictionary<string, AudioClip>();
  Dictionary<string, Material> MaterialCacheDic = new Dictionary<string, Material>();

  const string AudioFolderName = "Audios";
  const string PrefabsFolderName = "Prefabs";
  const string TextruePackFolderName = "TexturePacker";
  const string ScenesFolderName = "Scenes";
  const string FontMaterialFolderName = "FontMaterial";

  public void Init() {
#if UNITY_EDITOR
    string[] allBundleNames = AssetDatabase.GetAllAssetBundleNames();
    foreach (var bundleName in allBundleNames) {
      string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundle(bundleName);
      foreach (var assetPath in assetPaths) {
        string fileName = Path.GetFileNameWithoutExtension(assetPath);
        if (assetPath.Contains(".png")) {
          SpriteAssetDic.Add(fileName, assetPath);
        } else if (assetPath.Contains("prefab")) {
          PrefabAssetDic.Add(fileName, assetPath);
        } else if (assetPath.Contains("mp3") || assetPath.Contains("ogg")) {
          AudioClipAssetDic.Add(fileName, assetPath);
        } else if (assetPath.Contains("unity")) {
          SceneAssetDic.Add(fileName, assetPath);
        } else if (assetPath.Contains(".mat")) {
          MaterialAssetDic.Add(fileName, assetPath);
        }
      }
    }
#elif !UNITY_EDITOR
    InitRunTime();
#endif
  }

  void InitRunTime() {
    Debug.Log("this is runtime");
  }

  public Sprite instanceSprite(string atlasName, string spriteName) {

    if (String.IsNullOrEmpty(atlasName) || String.IsNullOrEmpty(spriteName))
      return null;

    string cacheName = atlasName + "_" + spriteName;
    if (SpriteCacheDic.ContainsKey(cacheName)) {
      return SpriteCacheDic[cacheName];
    }
#if UNITY_EDITOR
    if (!SpriteAssetDic.ContainsKey(atlasName)) {
      Debug.Log("669 - 沒有此大圖或是沒有給他設定BundleName : " + atlasName);
      return null;
    }
    string assetPath = SpriteAssetDic[atlasName];
    UnityEngine.Object[] sprites = null;
    sprites = AssetDatabase.LoadAllAssetsAtPath(assetPath);
    foreach (var sprite in sprites) {
      if (sprite.name == spriteName) {
        Sprite s = sprite as Sprite;
        SpriteCacheDic.Add(cacheName, s);
        return s;
      }
    }
#elif !UNITY_EDITOR
    Sprite[] sprites = Resources.LoadAll<Sprite>(TextruePackFolderName + "/" + atlasName);
    Sprite s = null;
    foreach (var sprite in sprites) {
      if (!SpriteCacheDic.ContainsKey(atlasName + "_" + sprite.name))
        SpriteCacheDic.Add(atlasName + "_" + sprite.name, sprite);
      if (sprite.name == spriteName) {
        s = sprite;
      }
    }
    return s;
#endif
    Debug.Log("669 - 大圖 : " + atlasName + "，並未包含 Sprite : " + spriteName);
    return null;

  }

  public Sprite instanceResourcesSprite(string res_path, string spriteName) {

    if (String.IsNullOrEmpty(res_path) || String.IsNullOrEmpty(spriteName))
      return null;

    string cacheName = res_path + "_" + spriteName;
    if (SpriteCacheDic.ContainsKey(cacheName)) {
      return SpriteCacheDic[cacheName];
    }

    var sprite = Resources.Load<Sprite>(res_path + "/" + spriteName);
    if (sprite == null) {
      Debug.Log("669 - 資料夾 : " + res_path + "，並未包含 Sprite : " + spriteName);
      return null;
    }
    SpriteCacheDic.Add(cacheName,sprite);
    return sprite;
  }

  public GameObject instancePrefab(string name) {
    GameObject go;
    if (PrefabCacheDic.ContainsKey(name)) {
      go = GameObject.Instantiate(PrefabCacheDic[name]);
      rePositionTouchEventReceiverChildren(go);
      return go;
    }

#if UNITY_EDITOR
    if (!PrefabAssetDic.ContainsKey(name)) {
      Debug.Log("669 - 沒有此Prefab : " + name);
      return null;
    }
    string assetPath = PrefabAssetDic[name];
    UnityEngine.Object prefab = null;
    prefab = AssetDatabase.LoadAssetAtPath(assetPath, typeof(UnityEngine.GameObject));
#elif !UNITY_EDITOR
    GameObject prefab = Resources.Load<GameObject>(PrefabsFolderName + "/" + name);
        if (prefab == null)
      return null;
#endif
    GameObject p = prefab as GameObject;
    go = GameObject.Instantiate(p);
    PrefabCacheDic.Add(name, p);
    rePositionTouchEventReceiverChildren(go);
    //reScaleTouchEventReceiverChildren(go);
    return go;
  }

  void rePositionTouchEventReceiverChildren(GameObject go) {
    ITouchEventReceiver[] srs = go.GetComponentsInChildren<ITouchEventReceiver>(true);
    foreach (var v in srs) {
      Vector3 target = UtilityHelper.RePosition(v.GetGameObject().transform.position);
      target = new Vector3(target.x, target.y, v.GetGameObject().transform.position.z);
      v.GetGameObject().transform.position = target;
    }
  }
  void reScaleTouchEventReceiverChildren(GameObject go) {
    ITouchEventReceiver[] srs = go.GetComponentsInChildren<ITouchEventReceiver>(true);
    foreach (var v in srs) {
      v.GetGameObject().transform.localScale *= UtilityHelper.CanvasScale;
    }
  }

  public AudioClip instanceAudioClip(string audioName) {
    if (String.IsNullOrEmpty(AudioFolderName) || String.IsNullOrEmpty(audioName))
      return null;

    string cacheName = AudioFolderName + "_" + audioName;
    if (AudioClipCacheDic.ContainsKey(cacheName)) {
      return AudioClipCacheDic[cacheName];
    }

#if UNITY_EDITOR
    if (!AudioClipAssetDic.ContainsKey(audioName)) {
      Debug.Log("669 - 沒有此資料夾 ， 或是該檔案未設定bundleName : " + AudioFolderName + "/" + audioName);
      return null;
    }
    string assetPath = AudioClipAssetDic[audioName];
    UnityEngine.AudioClip clip = null;
    clip = (AudioClip)AssetDatabase.LoadAssetAtPath(assetPath, typeof(UnityEngine.AudioClip));
#elif !UNITY_EDITOR
        AudioClip clip = Resources.Load<AudioClip>(AudioFolderName + "/" + audioName);
#endif

    if (clip == null) {
      Debug.Log("669 - 音效讀取失敗");
      return null;
    }
    AudioClipCacheDic.Add(cacheName, clip);
    return clip;
  }

  public Sprite[] instanceSpriteSheet(string atlasName) {
    if (String.IsNullOrEmpty(atlasName))
      return null;

    string cacheName = atlasName;
    if (SpriteSheetCacheDic.ContainsKey(cacheName)) {
      return SpriteSheetCacheDic[cacheName];
    }

#if UNITY_EDITOR
    if (!SpriteAssetDic.ContainsKey(atlasName)) {
      Debug.Log("669 - 沒有此大圖 ， 或是該檔案未設定bundleName : " + atlasName);
      return null;
    }
    string assetPath = SpriteAssetDic[atlasName];
    UnityEngine.Object[] assets = null;
    assets = AssetDatabase.LoadAllAssetRepresentationsAtPath(assetPath);
#elif !UNITY_EDITOR
    Sprite[] assets = Resources.LoadAll<Sprite>(TextruePackFolderName + "/" + atlasName);
#endif
    if (assets == null || assets.Length == 0) {
      Debug.Log("669 - 技能讀取失敗");
      return null;
    }
    //偷塞一張空的圖，這樣在spriteAnimator，就不用特地關閉它
    Sprite[] sprites = new Sprite[assets.Length + 1];
    int index = 0;
    foreach (var v in assets) {
      sprites[index] = v as Sprite;
      index++;
    }
    sprites[assets.Length] = Sprite.Create(null, Rect.zero, Vector2.zero);
    SpriteSheetCacheDic.Add(cacheName, sprites);
    return sprites;
  }

  public string ScenePath(string SceneName) {
    if (String.IsNullOrEmpty(SceneName))
      return "";
#if UNITY_EDITOR
    string path = "";
    if (!SceneAssetDic.ContainsKey(SceneName)) {
      Debug.Log("669 - 沒有此資料夾 ， 或是該檔案未設定bundleName : " + SceneName);
      return path;
    }
    return SceneAssetDic[SceneName];
#elif !UNITY_EDITOR
    return SceneName;
#endif
  }

  public ScriptableObject instanceScriptableObject(string resourcesPath,string fileName) {
    return Resources.Load<ScriptableObject>(resourcesPath + "/" + fileName);
  }

  public Material instanceMaterial(string name) {
    if (MaterialCacheDic.ContainsKey(name)) {
      return MaterialCacheDic[name];
    }

#if UNITY_EDITOR
    if (!MaterialAssetDic.ContainsKey(name)) {
      Debug.Log("669 - 沒有此Material 或是沒有設定bundleName : " + name);
      return null;
    }
    string assetPath = MaterialAssetDic[name];
    UnityEngine.Object material = null;
    material = AssetDatabase.LoadAssetAtPath(assetPath, typeof(UnityEngine.Material));
#elif !UNITY_EDITOR
    Material material = Resources.Load<Material>(FontMaterialFolderName + "/" + name);
        if (material == null)
      return null;
#endif
    Material m = material as UnityEngine.Material;
    MaterialCacheDic.Add(name, m);
    return m;
  }

  public void ReleaseCache() {
    SpriteCacheDic.Clear();
    SpriteSheetCacheDic.Clear();
    AudioClipCacheDic.Clear();
    PrefabCacheDic.Clear();
    Resources.UnloadUnusedAssets();
  }
}
