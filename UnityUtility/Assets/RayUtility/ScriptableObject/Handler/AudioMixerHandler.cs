using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AudioMixerHandler : MonoBehaviour {
  AudioMixerData audioMixerData;

  public void init(AudioMixerData audioMixerData) {
    this.audioMixerData = audioMixerData;
  }

  void OnUpdateVolume() {
    if (audioMixerData == null)
      return;
    audioMixerData.setMasterVolume(audioMixerData.masterVolume);
    audioMixerData.setBgmVolume(audioMixerData.BgmVolume);
    audioMixerData.setSfxVolume(audioMixerData.SfxVolume);
  }

  public void SetMasterVolume(float vol) {
    audioMixerData.masterVolume = vol;
  }

  private void Update() {
    if (Application.isEditor)
      OnUpdateVolume();
  }
}
