
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ExampleApp : UdonSharpBehaviour
{
    public GameObject Pointer; // Touch Point Object
    public GameObject MainUI; // Main UI

    public GameObject UI_CurrentApp; // harunatab System, do not change it
    public GameObject UI_SuspendedApp; // harunatab System, do not change it

    public Sprite HarudonIcon; // Icon sprite that will shown in Home Screen
    public string HarudonName; // App name that will shown in Home Screen

    // When harunaOS is started, the system will send harudon_verify event to all installed behaviours.
    public void harudon_verify()
    {

    }

    // When the app is launched from Home Screen, harudon_init event will be occurred.
    public void harudon_init()
    {
        MainUI.SetActive(true);
        MainUI.transform.SetParent(UI_CurrentApp.transform);
    }

    // When home button is pressed and this app is sent to Suspended App, harudon_stop event will be occurred.
    public void harudon_stop()
    {

    }

    // When tablet owner is not local player and tablet has already started, harudon_lateinit event will occurred to some apps, letting apps sync themselves.
    public void harudon_lateinit()
    {

    }

    // Similar to harudon_lateinit, but this event will occur additionally to also sync the display activity of your app.
    public void harudon_lateinit_ui()
    {
        MainUI.SetActive(true);
        MainUI.transform.SetParent(UI_CurrentApp.transform);
    }

    // When Touch down is detected
    public void Touch_Down()
    {
        Vector3 PointerPosition = Pointer.transform.localPosition;
        int x = (int)PointerPosition.x;
        int y = (int)PointerPosition.z;
        int pressure = (int)PointerPosition.y;
        if (x >= 0 && x <= 1920 && y >= 0 && y <= 1080)
        {
            
        }
    }

    // When Touch is discontinued
    public void Touch_Up()
    {

    }

    // Called every frame, if the touch stays on screen
    public void Touch_Stay()
    {

    }
}
