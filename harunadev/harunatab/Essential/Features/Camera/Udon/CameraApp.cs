
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class CameraApp : UdonSharpBehaviour
{
    public GameObject Pointer;
    public GameObject MainUI;

    public Camera Camera1;
    public Camera Camera2;

    public GameObject Lens1;
    public GameObject Lens2;

    public GameObject Icon_Take_Lens1;
    public GameObject Icon_Refresh_Lens1;
    public GameObject Icon_Take_Lens2;
    public GameObject Icon_Refresh_Lens2;

    public GameObject UI_CurrentApp;
    public GameObject UI_SuspendedApp;

    public Sprite HarudonIcon;
    public string HarudonName;

    public void harudon_verify()
    {

    }

    public void harudon_init()
    {
        Camera1.enabled = true;
        Camera2.enabled = true;
        Lens1.SetActive(true);
        Lens2.SetActive(false);
        Icon_Take_Lens1.SetActive(true);
        Icon_Take_Lens2.SetActive(true);
        Icon_Refresh_Lens1.SetActive(false);
        Icon_Refresh_Lens2.SetActive(false);

        MainUI.SetActive(true);
        MainUI.transform.SetParent(UI_CurrentApp.transform);
    }

    public void harudon_stop()
    {
        Camera1.enabled = false;
        Camera2.enabled = false;
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
        Vector3 PointerPosition = Pointer.transform.localPosition;
        if (PointerPosition.x >= 1700 && PointerPosition.x <= 1872 && PointerPosition.z >= 48 && PointerPosition.z <= 804)
        {
            if (Lens1.activeSelf)
            {
                //Toggle_Lens1();
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Toggle_Lens1");
            } else
            {
                //Toggle_Lens2();
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Toggle_Lens2");
            }
        }
        if (PointerPosition.x >= 1700 && PointerPosition.x <= 1872 && PointerPosition.z >= 857 && PointerPosition.z <= 1029)
        {
            //Toggle_LensChange();
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Toggle_LensChange");
        }
    }

    public void Touch_Up()
    {

    }

    public void Touch_Stay()
    {

    }

    public void Toggle_Lens1()
    {
        if (Camera1.enabled)
        {
            Camera1.enabled = false;
            Icon_Refresh_Lens1.SetActive(true);
            Icon_Take_Lens1.SetActive(false);
        } else
        {
            Camera1.enabled = true;
            Icon_Refresh_Lens1.SetActive(false);
            Icon_Take_Lens1.SetActive(true);
        }
    }

    public void Toggle_Lens2()
    {
        if (Camera2.enabled)
        {
            Camera2.enabled = false;
            Icon_Refresh_Lens2.SetActive(true);
            Icon_Take_Lens2.SetActive(false);
        }
        else
        {
            Camera2.enabled = true;
            Icon_Refresh_Lens2.SetActive(false);
            Icon_Take_Lens2.SetActive(true);
        }
    }

    public void Toggle_LensChange()
    {
        if (Lens1.activeSelf)
        {
            Lens1.SetActive(false);
            Lens2.SetActive(true);
        } else
        {
            Lens1.SetActive(true);
            Lens2.SetActive(false);
        }
    }
}
