using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "New AudioMixer", menuName = "ScriptableObjectData/AudioMixer")]
public class AudioMixerData : ScriptableObject {
  [SerializeField]
  AudioMixer Mixer;
  //sound is db so the minvalue cant be 0,must closet 0
  [Range(0.0001f, 1.0f)]
  public float masterVolume = 1.0f;
  [Range(0.0001f, 1.0f)]
  public float BgmVolume = 1.0f;
  [Range(0.0001f, 1.0f)]
  public float SfxVolume = 1.0f;

  public void setMasterVolume(float value) {
    //Mathf.Log10 sound is db
    Mixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
  }
  public void setBgmVolume(float value) {
    //Mathf.Log10 sound is db
    Mixer.SetFloat("BgmVolume", Mathf.Log10(value) * 20);
  }
  public void setSfxVolume(float value) {
    //Mathf.Log10 sound is db
    Mixer.SetFloat("SfxVolume", Mathf.Log10(value) * 20);
  }

  public AudioMixerGroup getGroup(string groupName) {
    AudioMixerGroup[] groups = Mixer.FindMatchingGroups(groupName);
    if (groups == null)
      return null;
    if (groups.Length <= 0)
      return null;
    return groups[0];
  }

  private void OnEnable() {
    bool UsePrefs = Application.isEditor ? false : true;
    if (UsePrefs)
      LoadPrefs();
  }

  public void SavePrefs() {
    string key = name + "MixerPrefs";
    string json = JsonConvert.SerializeObject(this);
    PlayerPrefs.SetString(key, json);
    PlayerPrefs.Save();
  }

  void LoadPrefs() {
    string key = name + "MixerPrefs";
    if (!PlayerPrefs.HasKey(key)) {
      return;
    }
    AudioMixerData pref = JsonConvert.DeserializeObject<AudioMixerData>(PlayerPrefs.GetString(key));
    masterVolume = pref.masterVolume;
    BgmVolume = pref.BgmVolume;
    SfxVolume = pref.SfxVolume;
    return;
  }
}

