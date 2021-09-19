
using System;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class CookieClickerApp : UdonSharpBehaviour
{
    public GameObject MainUI;

    public Animator CookieAnim;

    public Text ClickIndicator;

    public GameObject UI_CurrentApp;
    public GameObject UI_SuspendedApp;

    [UdonSynced] public ulong cookieclicks = 0;

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
        RequestSerialization();
    }

    public void harudon_lateinit_ui()
    {
        MainUI.SetActive(true);
        MainUI.transform.SetParent(UI_CurrentApp.transform);
    }

    public void Touch_Down()
    {
        ClickCookie();
    }

    public void Touch_Up()
    {

    }

    public void Touch_Stay()
    {

    }

    public void ClickCookie()
    {
        if (Networking.LocalPlayer == Networking.GetOwner(this.gameObject))
        {
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "FakeClickCookie");
            CookieAnim.Play("Base Layer.CookieClickAnimation", -1);
            cookieclicks++;
            ClickIndicator.text = Convert.ToString(cookieclicks);
        }
    }

    public void FakeClickCookie()
    {
        if (Networking.LocalPlayer != Networking.GetOwner(this.gameObject))
        {
            CookieAnim.Play("Base Layer.CookieClickAnimation", -1);
        }
    }
}
