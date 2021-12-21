using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ObjectsData", menuName = "ScriptableObjectData/Objects")]
public class ObjectsData : ScriptableObject {
  Dictionary<string, object> ObjectsDic = null;
  public int Count {
    get { return ObjectsDic == null ? 0 : ObjectsDic.Count; }
  }
  private void OnEnable() {
    ObjectsDic = new Dictionary<string, object>();
  }
  private void OnDestroy() {
    ObjectsDic = new Dictionary<string, object>();
  }

  public void add(string key, object o) {
    ObjectsDic.Add(key, o);
  }
  public void clear() {
    ObjectsDic.Clear();
  }
  public T[] getArray<T>() {
    T[] tmp = new T[ObjectsDic.Count];
    int index = 0;
    foreach (var v in ObjectsDic) {
      T t = (T)v.Value;
      tmp[index] = t;
      index++;
    }
    return tmp;
  }

  public T getObjectByKey<T>(string key) {
    if (!ObjectsDic.ContainsKey(key))
      return default(T);
    return (T)ObjectsDic[key];
  }
}
