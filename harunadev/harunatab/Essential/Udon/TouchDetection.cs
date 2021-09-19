
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class TouchDetection : UdonSharpBehaviour
{
    public GameObject Pointer;
    public UdonBehaviour harunatabSystem;
    private bool touched = false;
    private bool sentTouchUp = false;

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == Pointer.name)
        {
            System.SendCustomEvent("Touch_Down");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == Pointer.name)
        {
            System.SendCustomEvent("Touch_Up");
        }
    }*/

    private void Update()
    {
        //Debug.Log("Touch register" + Pointer.transform.localPosition.x + " " + Pointer.transform.localPosition.y + " " + Pointer.transform.localPosition.z);
        Vector3 PointerPosition = Pointer.transform.localPosition;
        if (Convert.ToInt32(PointerPosition.x) >= 0 && Convert.ToInt32(PointerPosition.x) < 1920 && Convert.ToInt32(PointerPosition.z) >= 0 && Convert.ToInt32(PointerPosition.z) < 1080 && Convert.ToInt32(PointerPosition.y) < 130 && Convert.ToInt32(PointerPosition.y) > -130)
        {
            if (touched)
            {
                harunatabSystem.SendCustomEvent("Touch_Stay");
            } else
            {
                touched = true;
                harunatabSystem.SendCustomEvent("Touch_Down");
            }
            sentTouchUp = false;
        } else
        {
            if (!sentTouchUp)
            {
                harunatabSystem.SendCustomEvent("Touch_Up");
                sentTouchUp = true;
                touched = false;
            }
        }
    }
}
