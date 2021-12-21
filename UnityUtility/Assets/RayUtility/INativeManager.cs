using UnityEngine;

public abstract class INativeManager : MonoBehaviour {
  public delegate void StringCallBack(string data);
  public StringCallBack OnLoginSuccess = null;
  public StringCallBack OnLoginFail = null;
  public virtual void init() {
    return;
  }

  public virtual void Login() {
    return;
  }

  public virtual void Vibrate(float duration = 0.0f) {
    return;
  }
  public virtual void OpenUrl(string url) {
    return;
  }
  public virtual void PostToken(string url,string code,EventDelegate onDone) {
    return;
  }
  public virtual void RefreshToken(string url, string code, EventDelegate onDone) {
    return;
  }
  public virtual void CaptureScreenShotAndShared(Texture2D t) { return; }
}

public class EditorNative : INativeManager {
  public override void OpenUrl(string url) {
    Debug.Log("Open url : " + url);
    Application.OpenURL(url);
    return;
  }
}