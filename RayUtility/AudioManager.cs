using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static AudioListData;

public class AudioManager {

  const float fadeTime = 0.5f;
  public enum Channel {
    BGM1,
    BGM2,
    SE,
    SZ
  }

  Dictionary<Channel, string> mixerGroupName = new Dictionary<Channel, string>
  {
    { Channel.BGM1,"Bgm" },
    { Channel.BGM2,"Bgm" },
    { Channel.SE,"Sfx" },
  };

  AudioSource[] audioSources;
  AudioListData BgmList;
  AudioListData SfxList;
  AudioMixerData audioMixerData;
  AudioMixerHandler AMH;
  public void Init(GameManager gm) {
    GameObject go = new GameObject("AudioManager");
    go.transform.SetParent(gm.transform);

    audioMixerData = GameManager.instance.assetBundleLoader.instanceScriptableObject(
    "ScriptableObjects/Audio/Mixer", "Default")
  as AudioMixerData;

    BgmList = GameManager.instance.assetBundleLoader.instanceScriptableObject(
    "ScriptableObjects/Audio/Bgm", "Default")
  as AudioListData;

    SfxList = GameManager.instance.assetBundleLoader.instanceScriptableObject(
    "ScriptableObjects/Audio/Sfx", "Default")
  as AudioListData;

    AMH = go.AddComponent<AudioMixerHandler>();
    AMH.init(audioMixerData);

    List<AudioSource> tmp = new List<AudioSource>((int)Channel.SZ);
    for (int i = 0; i < (int)Channel.SZ; i++) {
      AudioSource aS = go.AddComponent<AudioSource>();
      aS.outputAudioMixerGroup = audioMixerData.getGroup(mixerGroupName[((Channel)i)]);
      aS.Stop();
      aS.volume = 0;
      aS.playOnAwake = false;
      tmp.Add(aS);
    }
    audioSources = tmp.ToArray();
  }
  public void PlayBGM(string audioname, bool loop = true) {

    AudioSource currentBGMCh = getCurrentPlayBGMChannel();
    //AudioClip ac = GameManager.instance.assetBundleLoader.instanceAudioClip(audioname);
    AudioStruck AS = BgmList.GetAudioStruck(audioname);
    AudioClip ac = AS.AudioClip;
    float volume = AS.Volume;

    if (currentBGMCh == null) {
      //預設使用 BGM1 channel播放BGM
      currentBGMCh = audioSources[0];
      currentBGMCh.clip = ac;
      currentBGMCh.volume = 0;
      currentBGMCh.loop = loop;
      currentBGMCh.Play();
      //預設fadeIn
      DOTween.To(() => currentBGMCh.volume, x => currentBGMCh.volume = x, 1, fadeTime);
      return;
    }

    AudioSource IdleBGMCh = getIdleBGMChannel();
    if (IdleBGMCh == null) {
      Debug.Log("485 - 沒有閒置的BGM Channel 可以播放BGM");
      return;
    }

    //fade BGM between currentBGMCh 、 IdleBGMCh
    IdleBGMCh.volume = 0f;
    IdleBGMCh.clip = ac;
    IdleBGMCh.loop = loop;
    DOTween.To(() => currentBGMCh.volume, x => currentBGMCh.volume = x, 0, fadeTime).onComplete += () => { currentBGMCh.Stop(); };
    DOTween.To(() => IdleBGMCh.volume, y => IdleBGMCh.volume = y, volume, fadeTime);
    IdleBGMCh.Play();
  }
  public void PlayEffect(string name) {
    AudioStruck AS = SfxList.GetAudioStruck(name);
    AudioClip ac = AS.AudioClip;
    float volume = AS.Volume;

    audioSources[(int)Channel.SE].volume = volume;
    //audioSources[(int)Channel.SE].PlayOneShot(GameManager.instance.assetBundleLoader.instanceAudioClip(name));
    audioSources[(int)Channel.SE].PlayOneShot(ac);
  }

  AudioSource getCurrentPlayBGMChannel() {
    for (int i = (int)Channel.BGM1; i < (int)Channel.SE; i++) {
      if (audioSources[i].isPlaying)
        return audioSources[i];
    }
    return null;
  }
  AudioSource getIdleBGMChannel() {
    for (int i = (int)Channel.BGM1; i < (int)Channel.SE; i++) {
      if (!audioSources[i].isPlaying)
        return audioSources[i];
    }
    return null;
  }

  public void SetMasterVolume(float vol) {
    AMH.SetMasterVolume(vol);
    audioMixerData.SavePrefs();
  }
}