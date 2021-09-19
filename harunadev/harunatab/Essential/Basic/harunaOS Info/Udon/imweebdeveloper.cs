
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class imweebdeveloper : UdonSharpBehaviour
{
    public GameObject MainUI;

    public GameObject UI_CurrentApp;
    public GameObject UI_SuspendedApp;

    public Sprite HarudonIcon;
    public string HarudonName;

    public void harudon_verify()
    {

    }

    public void harudon_init()
    {
        MainUI.SetActive(true);
        MainUI.transform.SetParent(UI_CurrentApp.transform);
    }

    public void harudon_stop()
    {

    }

    public void harudon_lateinit()
    {

    }

    public void harudon_lateinit_ui()
    {
        MainUI.SetActive(true);
        MainUI.transform.SetParent(UI_CurrentApp.transform);
    }

    public void Touch_Down()
    {

    }

    public void Touch_Up()
    {

    }

    public void Touch_Stay()
    {

    }
}
