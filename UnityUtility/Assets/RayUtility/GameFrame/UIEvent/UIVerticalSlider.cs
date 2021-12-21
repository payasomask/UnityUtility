using UnityEngine;
using System.Collections;

public class UIVerticalSlider : MonoBehaviour, ITouchEventReceiver {
  Vector2 velocity =Vector2.zero;
  bool add_velocity =false;

  //是否傳遞訊息 (此 ui 底下的 ui controller 不對 touch event 有反應)
  public bool passTouchEvent =false;
  public bool passClickEvent =true;

  bool focusing =true;
	
	// Update is called once per frame
	void Update () {
    if (add_velocity){
      if (velocity.magnitude>0.1f){
        velocity*=0.9f;
        Vector2 tmpDisplacement =velocity*Time.deltaTime;

        GameManager.instance.setUIEvent(gameObject.name, UIEventType.VERTICAL_SLIDER, new object[]{tmpDisplacement, false});
      }
    }
	
	}

  public bool getStopPassLongPressEvent(Vector3 pt){
    return !passClickEvent;
  }

  public float getLongPressDuration(){
    return 0.7f;
  }

  public bool OnClick(Vector2 pt){
    return !passClickEvent;
  }

  public void OnLongPress(Vector2 pt, out bool request_focusing){
    request_focusing =false;
  }

  float vel =0f;
  float acc_displacement =0f;
  bool moving =false;
  public bool OnTouchMove(Vector2 curr_touch, Vector2 displacement, out bool request_focusing){
    request_focusing =false;

    if (Mathf.Abs(displacement.y)>0.0f){

      if (focusing && moving){
        GameManager.instance.setUIEvent(gameObject.name, UIEventType.VERTICAL_SLIDER, new object[]{displacement, true});
        velocity =displacement/Time.deltaTime;
        velocity.x =0f;
      }else{

        acc_displacement+=Mathf.Abs(displacement.y);
        vel=Mathf.Abs(displacement.y)/Time.deltaTime;
        if (vel>=500f || acc_displacement>=25f){
          moving =true;
          request_focusing =true;
          vel =0f;
        }
      }
    }
    return !passTouchEvent;
  }

  public bool OnTouchEnter(Vector2 posi, out bool request_focusing){
    request_focusing =false;
    add_velocity =false;
    velocity =Vector2.zero;

    return !passTouchEvent;
  }

  public void OnTouchLeave(){
    add_velocity =true;
    moving =false;
    acc_displacement =0f;

  }

  public void OnFocusRequested(GameObject requested_obj){
    if (requested_obj==this.gameObject){
      return;
    }

    if (requested_obj==null){
      focusing =true;
      return;
    }

    //deactivate ontouchmove event
    focusing =false;
    add_velocity =false;
    velocity =Vector2.zero;
    moving =false;
    acc_displacement =0f;
  }

  public void onHighLight(bool mouseIn) {
    return;
  }

  public GameObject GetGameObject() {
    return gameObject;
  }
}
