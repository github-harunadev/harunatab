
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class DisplayCameraResize : UdonSharpBehaviour
{
    public GameObject TargetObject;
    public Camera TargetCamera;

    private void Update()
    {
        TargetCamera.orthographicSize = TargetObject.transform.localScale.x * 0.054F;
    }
}
