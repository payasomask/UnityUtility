using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName ="New AudioList",menuName ="ScriptableObjectData/AudioList")]
public class AudioListData : ScriptableObject
{
  [Serializable]
  public class AudioStruck {
    public string Name;
    public AudioClip AudioClip;
    [Range(0.0f, 1.0f)]
    public float Volume = 1.0f;
  }
  [SerializeField]
  public List<AudioStruck> List = new List<AudioStruck>();

  public AudioStruck GetAudioStruck(string StruckName) {
    for(int i = List.Count-1; i >= 0; i--) {
      AudioStruck a = List[i];
      if (a.Name == StruckName)
        return a;
    }
    return null;
  }
}
