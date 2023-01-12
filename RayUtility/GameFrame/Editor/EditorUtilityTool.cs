using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

public class EditorUtilityTool {
  //comboSlelect Objects if is TextAsset, export comboed TextAsset
  [MenuItem("EditorUtilityTool/Txt/Combo")]
  public static void ComboTxt() {
    UnityEngine.Object[] objects = Selection.objects;
    if (objects == null || objects.Length == 0)
      return;
    List<TextAsset> filterList = new List<TextAsset>();
    foreach(var item in objects) {
      if (item.GetType() != typeof(TextAsset))
        continue;
      filterList.Add(item as TextAsset);
    }

    if (filterList.Count == 0)
      return;

    string s = "";
    foreach(var v in filterList) {
      s += v.text;
    }

    string filepath = Path.Combine(Application.dataPath, "font");
    filepath = Path.Combine(filepath, "ComboTxt.txt");
    StreamWriter writer = new StreamWriter(filepath);
    writer.Write(s);
    writer.Flush();
    writer.Close();

    return;
  }
}
