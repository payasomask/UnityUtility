using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New String", menuName = "ScriptableObjectData/String")]

public class StringData : ScriptableObject
{
  [SerializeField]
  string _Value;
  public string Value {
    set {
      _Value = value;
    }
  }
  public static implicit operator string(StringData sd) {
    if (sd == null)
      return String.Empty;
    return sd._Value;
  }
  //public static implicit operator StringData(string s) {
  //  StringData sd = new StringData();
  //  sd.Value = s;
  //  return sd;
  //}
  public void Clear() {
    _Value = "";
  }

  public T DeserializeObject<T>() {
    try {
      return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(_Value);
    }
    catch {
      return default(T);
    }
  }
}
