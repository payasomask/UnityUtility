// Copyright 2014 Jarrah Technology (http://www.jarrahtechnology.com). All Rights Reserved. 

using UnityEngine;
using UnityEngine.UI;

public static class CameraExtensions
{
  public static void LayerCullingShow( Camera cam, int layerMask)
  {
    cam.cullingMask |= layerMask;
  }

  public static void LayerCullingShow( Camera cam, string layer)
  {
    LayerCullingShow(cam, 1 << LayerMask.NameToLayer(layer));
  }

  public static void LayerCullingHide( Camera cam, int layerMask)
  {
    cam.cullingMask &= ~layerMask;
  }

  //關閉指定layer
  public static void LayerCullingHide( Camera cam, string layer)
  {
    LayerCullingHide(cam, 1 << LayerMask.NameToLayer(layer));
  }

  public static void LayerCullingToggle( Camera cam, int layerMask)
  {
    cam.cullingMask ^= layerMask;
  }

  public static void LayerCullingToggle( Camera cam, string layer)
  {
    LayerCullingToggle(cam, 1 << LayerMask.NameToLayer(layer));
  }

  public static bool LayerCullingIncludes( Camera cam, int layerMask)
  {
    return (cam.cullingMask & layerMask) > 0;
  }

  public static bool LayerCullingIncludes( Camera cam, string layer)
  {
    return LayerCullingIncludes(cam, 1 << LayerMask.NameToLayer(layer));
  }

  public static void LayerCullingToggle( Camera cam, int layerMask, bool isOn)
  {
    bool included = LayerCullingIncludes(cam, layerMask);
    if (isOn && !included)
    {
      LayerCullingShow(cam, layerMask);
    }
    else if (!isOn && included)
    {
      LayerCullingHide(cam, layerMask);
    }
  }

  public static void LayerCullingToggle( Camera cam, string layer, bool isOn)
  {
    LayerCullingToggle(cam, 1 << LayerMask.NameToLayer(layer), isOn);
  }

  public static void CanvasScreenSpaceCamera(Canvas c){
    c.renderMode = RenderMode.ScreenSpaceCamera;
    c.worldCamera = Camera.main;
    CanvasScaler cs = c.GetComponent<CanvasScaler>();
    cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
    cs.referenceResolution = new Vector2(UtilityHelper.ResolutionX, UtilityHelper.ResolutionY);
    cs.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
    cs.referencePixelsPerUnit = 1;
  }
}