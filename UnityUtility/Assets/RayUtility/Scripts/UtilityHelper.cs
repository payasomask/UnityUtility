using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class UtilityHelper {
  public const float ResolutionX = 1080.0f;
  public const float ResolutionY = 1920.0f;

  public static int RandomInt(int lower, int upper) {
    return Mathf.FloorToInt(UnityEngine.Random.Range(0.0f, 1.0f) * (upper - lower + 1)) + lower;
  }

  public static float RandomFloat(float lower, float upper) {
    return UnityEngine.Random.Range(lower, upper);
  }

  public static float Lerp(float a, float b, float current, float total) {
    float t = current / total;
    return Mathf.Lerp(a, b, t);
  }
  public static float precent(float current, float total) {
    return current / total;
  }

  public static string LoadTxtFromResouce(string fileName) {
    return Resources.Load<TextAsset>(fileName).text;
  }

  // Example: "#ff000099".ToColor() red with alpha ~50%
  // Example: "ffffffff".ToColor() white with alpha 100%
  // Example: "00ff00".ToColor() green with alpha 100%
  // Example: "0000ff00".ToColor() blue with alpha 0%
  public static Color ToColor(this string color) {
    if (color.StartsWith("#", StringComparison.InvariantCulture)) {
      color = color.Substring(1); // strip #
    }

    if (color.Length == 6) {
      color += "FF"; // add alpha if missing
    }

    var hex = Convert.ToUInt32(color, 16);
    var r = ((hex & 0xff000000) >> 0x18) / 255f;
    var g = ((hex & 0xff0000) >> 0x10) / 255f;
    var b = ((hex & 0xff00) >> 8) / 255f;
    var a = ((hex & 0xff)) / 255f;

    return new Color(r, g, b, a);
  }

  public static T[] ShuffleCards<T>(T[] array) {
    int i = array.Length;
    int j;
    if (i == 0) {
      return array;
    }
    while (--i != 0) {
      System.Random ran = new System.Random();
      j = ran.Next() % (i + 1);
      T tmp = array[i];
      array[i] = array[j];
      array[j] = tmp;
    }
    return array;
  }

  public static bool IsIncludeEn(string source) {
    Regex rg = new Regex(@"[^A-Za-z]");
    return rg.IsMatch(source);
  }

  //重新計算位置基於UtilityHelper.ResolutionXY，在不同解析度下相對於攝影機UV位置的世界座標
  public static Vector3 RePosition(Vector3 WorldPosition) {
    Vector3 source = WorldPosition;
    float Xzero = -UtilityHelper.ResolutionX * 0.5f;
    float Yzero = -UtilityHelper.ResolutionY * 0.5f;
    float z = WorldPosition.z;

    float XNormalize = (source.x - Xzero) / UtilityHelper.ResolutionX;
    float YNormalize = (source.y - Yzero) / UtilityHelper.ResolutionY;
    Vector3 target = Camera.main.ViewportToWorldPoint(new Vector3(XNormalize, YNormalize));
    target.z = z;
    return target;
  }

  public static void setupCameraCanvas(Transform t) {
    Canvas c = t.GetComponent<Canvas>();
    c.renderMode = RenderMode.ScreenSpaceCamera;
    c.worldCamera = Camera.main;
    CanvasScaler cs = t.GetComponent<CanvasScaler>();
    cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
    cs.referenceResolution = new Vector2(UtilityHelper.ResolutionX, UtilityHelper.ResolutionY);
    cs.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
    cs.referencePixelsPerUnit = 1;
  }

  public static void setupWorldCanvas(Transform t, Vector3 defaultLocalPostion) {
    Canvas c = t.GetComponent<Canvas>();
    c.renderMode = RenderMode.WorldSpace;
    c.transform.localPosition = defaultLocalPostion;
    //c.transform.localScale = Vector3.one;
    c.worldCamera = Camera.main;
    CanvasScaler cs = t.GetComponent<CanvasScaler>();
    cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
    cs.referenceResolution = new Vector2(UtilityHelper.ResolutionX, UtilityHelper.ResolutionY);
    cs.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
    cs.referencePixelsPerUnit = 1;
  }

  public static void SaveJsonText(string fileName, SaveData sd) {
    string savePath = Path.Combine(Application.persistentDataPath, fileName + ".json");
    string json = JsonUtility.ToJson(sd);
    File.WriteAllText(savePath, json);
  }

  public static void SaveJsonTextWithSHA256(string fileName, SaveData sd) {
    string savePath = Path.Combine(Application.persistentDataPath, fileName + ".json");
    string json = JsonUtility.ToJson(sd,true);

    SHA256Managed crypt = new SHA256Managed();
    string hash = String.Empty;

    byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(json), 0, Encoding.UTF8.GetByteCount(json));

    foreach(byte bit in crypto) {
      hash += bit.ToString("x2");
    }

    sd.hashofContents = hash;

    File.WriteAllText(savePath, json);
  }
  public static void LoadJsonText(string fileName) {
    string savePath = Path.Combine(Application.persistentDataPath, fileName + ".json");
    string jsonString = File.ReadAllText(savePath);

    //var settings = new JsonSerializerSettings {
    //  NullValueHandling = NullValueHandling.Ignore,
    //  MissingMemberHandling = MissingMemberHandling.Ignore
    //};
    //object storys = Newtonsoft.Json.JsonConvert.DeserializeObject<object>(jsonString, settings);

    SaveData data = JsonUtility.FromJson<SaveData>(jsonString);
  }

  //使用enhanced scroller設定cell的寬高時是實際Pixel寬/高，會因為解析度改變cell而變大變小
  //為了在不同解析度，每個cell的寬/高都一樣的占比，需要提供縮放比例給scroller
  public static float CanvasScale { get { return ResolutionY / Screen.height; } }
}

public class CanvasHelper {
  static CanvasHelper _instance = null;
  public static CanvasHelper instance {
    get {
      if (_instance == null)
        _instance = new CanvasHelper();
      return _instance;
    }
  }  //UNITY使用LayoutGroup的children會被限制不能使用AspectRatioFitter，
  //需要被LayoutGroup排列的child想要保留原比例大小縮放的時候就會無法實現
  //此功能會類似於text的auto size
  public void AspectRatioFitterImageSize(RectTransform rT) {
    rT.sizeDelta /= UtilityHelper.CanvasScale;
  }

  //text有些情況會想要保留排版size大小，但是又不想開啟autosize(因為會造成不同長度的字，在不變的排版size而讓fontsize會變成不一致大小)
  //，而只調整fontSize的情況
  public void AspectRatioFitterTextSize(TMP_Text tt) {
    tt.rectTransform.sizeDelta /= UtilityHelper.CanvasScale;
    tt.fontSize /= UtilityHelper.CanvasScale;
  }
}

public class SaveData{
  public string Version;
  public string hashofContents;
}
