using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.DOTweenEditor;

[CustomEditor(typeof(AnimateHandler))]
public class AnimateHandlerEidtor : Editor {
  public override void OnInspectorGUI() {
    base.OnInspectorGUI();

    var script = (AnimateHandler)target;

    if (GUILayout.Button("IN", GUILayout.Height(40))) {
      if (DOTweenEditorPreview.isPreviewing)
        DOTweenEditorPreview.Stop(true, true);

      AnimateHandler[] all = script.GetComponentsInChildren<AnimateHandler>();
      List<DG.Tweening.Tween> tmp = new List<DG.Tweening.Tween>();
      foreach (var v in all) {
        tmp.AddRange(v.getInTweens());
      }

      DG.Tweening.Tween[] Tweens = tmp.ToArray();
      foreach (var v in Tweens)
        DOTweenEditorPreview.PrepareTweenForPreview(v);

      DOTweenEditorPreview.Start();
    }
    if (GUILayout.Button("OUT", GUILayout.Height(40))) {
      if (DOTweenEditorPreview.isPreviewing)
        DOTweenEditorPreview.Stop(true, true);

      AnimateHandler[] all = script.GetComponentsInChildren<AnimateHandler>();
      List<DG.Tweening.Tween> tmp = new List<DG.Tweening.Tween>();
      foreach (var v in all) {
        tmp.AddRange(v.getOutTweens());
      }

      DG.Tweening.Tween[] Tweens = tmp.ToArray();
      foreach (var v in Tweens)
        DOTweenEditorPreview.PrepareTweenForPreview(v);
      DOTweenEditorPreview.Start();
    }
    if (GUILayout.Button("Stop", GUILayout.Height(40))) {
      if (DOTweenEditorPreview.isPreviewing)
        DOTweenEditorPreview.Stop(true, true);

      AnimateHandler[] all = script.GetComponentsInChildren<AnimateHandler>();
      List<DG.Tweening.Tween> tmp = new List<DG.Tweening.Tween>();
      foreach (var v in all) {
        tmp.AddRange(v.getOutTweens());
      }

      DG.Tweening.Tween[] Tweens = tmp.ToArray();
      foreach (var v in Tweens)
        v.fullPosition = 0;
    }
  }
}