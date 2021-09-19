
using System;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class MainHome : UdonSharpBehaviour
{
    public GameObject Behaviours;
    public GameObject Item;
    public GameObject HomeActivity;
    public GameObject LauncherIconList;

    public UdonBehaviour harunatabSystem;

    public GameObject Pointer;

    public Sprite HarudonIcon;
    public string HarudonName;

    public void harudon_verify()
    {

    }

    public void harudon_init()
    {
        for(int i = 0; i < Behaviours.transform.childCount; i++)
        {
            GameObject item = Behaviours.transform.GetChild(i).gameObject;
            if (item.name != "INIT" && item.name != "Home")
            {
                GameObject LauncherIcon = VRCInstantiate(Item);
                LauncherIcon.SetActive(true);
                LauncherIcon.transform.SetParent(LauncherIconList.transform, false);
                LauncherIcon.name = item.name;
                ((Image)LauncherIcon.transform.GetChild(0).gameObject.GetComponent(typeof(Image))).sprite = (Sprite)((UdonBehaviour)item.GetComponent(typeof(UdonBehaviour))).GetProgramVariable("HarudonIcon");
                ((Text)LauncherIcon.transform.GetChild(1).gameObject.GetComponent(typeof(Text))).text = (string)((UdonBehaviour)item.GetComponent(typeof(UdonBehaviour))).GetProgramVariable("HarudonName");

            }

        }
    }

    public void harudon_stop()
    {

    }

    public void harudon_lateinit()
    {

    }

    public void harudon_lateinit_ui()
    {

    }

    public void Touch_Down()
    {
        Vector3 PointerPosition = Pointer.transform.localPosition;
        for (int i = 0; i < LauncherIconList.transform.childCount; i++)
        {
            int x = i / 4;
            int y = i - (x * 4);
            if (PointerPosition.x >= (120 + (200 * Convert.ToSingle(x))) && PointerPosition.x <= (240 + (200 * Convert.ToSingle(x))) && PointerPosition.z >= (160 + (220 * Convert.ToSingle(y))) && PointerPosition.z <= (340 + (220 * Convert.ToSingle(y))))
            {
                for (int ii = 0; ii < Behaviours.transform.childCount; ii++)
                {
                    GameObject item = Behaviours.transform.GetChild(ii).gameObject;
                    if (item.name == LauncherIconList.transform.GetChild(i).gameObject.name)
                    {
                        ((UdonBehaviour)item.GetComponent(typeof(UdonBehaviour))).SendCustomEvent("harudon_init");
                        UpdateInitBehaviour();
                    }
                }
            }
        }
        Debug.Log("Home Touch Registered, " + PointerPosition.x + " " + PointerPosition.z);
    }

    public void Touch_Up()
    {

    }

    public void Touch_Stay()
    {

    }

    public void UpdateInitBehaviour()
    {

    }
}
