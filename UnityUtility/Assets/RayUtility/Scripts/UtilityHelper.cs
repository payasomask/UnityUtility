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

  //????????????????????????UtilityHelper.ResolutionXY??????????????????????????????????????????UV?????????????????????
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

  //??????enhanced scroller??????cell?????????????????????Pixel???/??????????????????????????????cell???????????????
  //?????????????????????????????????cell??????/???????????????????????????????????????????????????scroller
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
  }  //UNITY??????LayoutGroup???children????????????????????????AspectRatioFitter???
  //?????????LayoutGroup?????????child????????????????????????????????????????????????????????????
  //?????????????????????text???auto size
  public void AspectRatioFitterImageSize(RectTransform rT) {
    rT.sizeDelta /= UtilityHelper.CanvasScale;
  }

  //text?????????????????????????????????size??????????????????????????????autosize(??????????????????????????????????????????????????????size??????fontsize????????????????????????)
  //???????????????fontSize?????????
  public void AspectRatioFitterTextSize(TMP_Text tt) {
    tt.rectTransform.sizeDelta /= UtilityHelper.CanvasScale;
    tt.fontSize /= UtilityHelper.CanvasScale;
  }
}

public class SaveData{
  public string Version;
  public string hashofContents;
}
