
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common;

public class MainSystem : UdonSharpBehaviour
{
    private string SystemVersion = "1.0.0000";
    private bool alreadyInitialized = false;
    private bool alreadyLateInitialized = false;
    public GameObject Pointer;

    public GameObject Behaviours;
    public UdonBehaviour INITBehaviour;

    public GameObject UI_Initializing;
    public Text Log;
    public GameObject UI_Main;

    public GameObject UI_Home;
    public GameObject UI_SuspendedApp;
    public GameObject UI_CurrentApp;

    public GameObject UI_Lockscreen;

    public GameObject UI_VisiblePointer;
    public GameObject UI_Sleep;

    [UdonSynced] public string[] InitBehaviours = null;
    [UdonSynced] public byte ScreenStatus = 0;
    // 0 = Screen Off
    // 1 = Locked
    // 2 = Status Bar Expanded
    // 3 = Normal
    public byte ScreenStatusTouch = 0;
    // 0 = No touch
    // 1 = Normal touch
    // 2 = System touch

    public UdonBehaviour StatusBarManager;
    public GameObject UI_StatusBar;
    public GameObject UI_StatusBarExpand;

    public void addLog(string msg) { Log.text += msg + "\n"; }

    private void FixedUpdate()
    {
        if (alreadyInitialized && Networking.LocalPlayer != Networking.GetOwner(this.gameObject))
        {
            if (ScreenStatus == 0)
            {
                UI_Sleep.SetActive(true);
            }
            else if (ScreenStatus == 1)
            {
                UI_Sleep.SetActive(false);
                UI_Lockscreen.SetActive(true);
            }
            else if (ScreenStatus == 2)
            {
                UI_Sleep.SetActive(false);
                UI_Lockscreen.SetActive(false);
                UI_StatusBar.SetActive(false);
                UI_StatusBarExpand.SetActive(true);
            }
            else if (ScreenStatus == 3)
            {
                UI_Sleep.SetActive(false);
                UI_Lockscreen.SetActive(false);
                UI_StatusBar.SetActive(true);
                UI_StatusBarExpand.SetActive(false);
            }
        }
    }

    public void harudon_init()
    {
        Debug.Log("harunatab system harudon_init");
        if (!alreadyInitialized)
        {
            alreadyInitialized = true;

            UI_Main.SetActive(false);
            UI_Initializing.SetActive(true);

            Log.text += "harunaOS system version " + SystemVersion + "\n\n" +
                "Made with love by harunadev\n\n";

            addLog("[ SYST ] Starting up...");
            addLog("[ SYST ] Detected " + Behaviours.transform.childCount + " harudon behaviours");

            for (int i = 0; i < Behaviours.transform.childCount; i++)
            {
                GameObject item = Behaviours.transform.GetChild(i).gameObject;
                if (item.name != "INIT")
                {
                    addLog("[ SYST ] Verifying " + item.name + " harudon behaviours");
                    ((UdonBehaviour)item.GetComponent(typeof(UdonBehaviour))).SendCustomEvent("harudon_verify");
                }
            }

            addLog("[ SYST ] Verification done.");
            if (Networking.LocalPlayer == Networking.GetOwner(this.gameObject))
            {
                addLog("[ SYST ] You're the owner of this tablet.");
            }
            else
            {
                if (Networking.GetOwner(this.gameObject) != null)
                {
                    addLog("[ SYST ] " + Networking.GetOwner(this.gameObject).displayName + " is the owner of this tablet.");
                }
                else
                {
                    addLog("[ SYST ] You're not the owner of this tablet.");
                }
            }

            addLog("[ SYST ] Verifying INIT");
            INITBehaviour.SendCustomEvent("harudon_verify");

            addLog("[ SYST ] Starting INIT");
            INITBehaviour.SendCustomEvent("harudon_init");
        }
    }

    public override void OnDeserialization()
    {
        if (Networking.LocalPlayer != Networking.GetOwner(this.gameObject))
        {
            if (!alreadyLateInitialized)
            {
                if (InitBehaviours != null)
                {
                    SendOutLateInit();
                }
            }
        }
    }

    public void SendOutLateInit()
    {
        alreadyLateInitialized = true;
        foreach(string BehaviourItemName in InitBehaviours)
        {
            for (int i = 0; i < Behaviours.transform.childCount; i++)
            {
                GameObject item = Behaviours.transform.GetChild(i).gameObject;
                if (item.name == BehaviourItemName)
                {
                    addLog("[ SYST ] Sending lateinit event to " + item.name + " harudon behaviour");
                    ((UdonBehaviour)item.GetComponent(typeof(UdonBehaviour))).SendCustomEvent("harudon_lateinit");
                }
            }
        }
    }

    public void ThrowCriticalError()
    {
        Log.text += "\n\n\nCritical Error has occurred throughout the harunatab system. This tablet will not be fixed in this instance, so please create another instance.";
    }

    public void Touch_GoHome()
    {
        if (alreadyInitialized)
        {
            if (ScreenStatus == 0)
            {
                System_DisplayOn();
            }
            else if (ScreenStatus == 1)
            {
                System_DisplayOff();
            }
            else if (ScreenStatus == 2)
            {
                System_CloseStatusBar();
            }
            else if (ScreenStatus == 3)
            {
                if (UI_CurrentApp.transform.childCount > 0)
                {
                    System_GoHome();
                } else
                {
                    System_DisplayOff();
                }
            }
        } else
        {
            harudon_init();
        }
    }

    public void Touch_Down()
    {
        if (ScreenStatus == 0)
        {

        } else if (ScreenStatus == 3)
        {
            ScreenStatusTouch = 1;
            if (UI_CurrentApp.transform.childCount < 1)
            {
                ((UdonBehaviour)UI_Home.GetComponent(typeof(UdonBehaviour))).SendCustomEvent("Touch_Down");
            }
            else
            {
                ((UdonBehaviour)(UI_CurrentApp.transform.GetChild(0)).gameObject.GetComponent(typeof(UdonBehaviour))).SendCustomEvent("Touch_Down");
            }
            Vector3 PointerPosition = Pointer.transform.localPosition;
            if (PointerPosition.z <= 40)
            {
                System_OpenStatusBar();
            }
        } else
        {
            ScreenStatusTouch = 2;
            System_Touch_Down();
        }
        UI_VisiblePointer.SetActive(true);
        UI_VisiblePointer.transform.localPosition = new Vector3(Pointer.transform.localPosition.x, Pointer.transform.localPosition.z, 0);
    }

    public void Touch_Up()
    {
        if (ScreenStatusTouch == 1)
        {
            if (UI_CurrentApp.transform.childCount < 1)
            {
                ((UdonBehaviour)UI_Home.GetComponent(typeof(UdonBehaviour))).SendCustomEvent("Touch_Up");
            } else
            {
                ((UdonBehaviour)(UI_CurrentApp.transform.GetChild(0)).gameObject.GetComponent(typeof(UdonBehaviour))).SendCustomEvent("Touch_Up");
            }
        } else if (ScreenStatusTouch == 2)
        {
            ScreenStatusTouch = 0;
            System_Touch_Up();
        }
        UI_VisiblePointer.SetActive(false);
    }

    public void Touch_Stay()
    {
        if (ScreenStatus == 0)
        {

        }
        else if (ScreenStatus == 3)
        {
            ScreenStatusTouch = 1;
            if (UI_CurrentApp.transform.childCount < 1)
            {
                ((UdonBehaviour)UI_Home.GetComponent(typeof(UdonBehaviour))).SendCustomEvent("Touch_Stay");
            }
            else
            {
                if (UI_CurrentApp.transform.GetChild(0) != null) ((UdonBehaviour)UI_CurrentApp.transform.GetChild(0).gameObject.GetComponent(typeof(UdonBehaviour))).SendCustomEvent("Touch_Stay");
            }
        }
        else
        {
            ScreenStatusTouch = 2;
            System_Touch_Stay();
        }
        UI_VisiblePointer.transform.localPosition = new Vector3(Pointer.transform.localPosition.x, Pointer.transform.localPosition.z, 0);
    }

    public void System_Touch_Down()
    {
        Vector3 PointerPosition = Pointer.transform.localPosition;
        if (ScreenStatus == 2)
        {
            if (PointerPosition.x >= 900 && PointerPosition.x <= 1020 && PointerPosition.z >= 876 && PointerPosition.z <= 996)
            {
                System_CloseStatusBar();
            }
        } else if (ScreenStatus == 1)
        {
            if (PointerPosition.x >= 0 && PointerPosition.x <= 1920 && PointerPosition.z >= 0 && PointerPosition.z <= 1080)
            {
                System_UnlockScreen();
            }
        }
    }

    public void System_Touch_Up()
    {
        
    }

    public void System_Touch_Stay()
    {

    }

    public void System_EndINIT()
    {
        addLog("\n\n -- Welcome to harunaOS   :3");
        System_LockScreen();
        UI_Main.SetActive(true);
        UI_Initializing.SetActive(false);
        RequestSerialization();
    }

    public void System_OpenStatusBar()
    {
        UI_StatusBar.SetActive(false);
        UI_StatusBarExpand.SetActive(true);
        ScreenStatus = 2;
        RequestSerialization();
    }

    public void System_CloseStatusBar()
    {
        UI_StatusBar.SetActive(true);
        UI_StatusBarExpand.SetActive(false);
        ScreenStatus = 3;
        RequestSerialization();
    }

    public void System_LockScreen()
    {
        ScreenStatus = 1;
        RequestSerialization();
        UI_Lockscreen.SetActive(true);
    }

    public void System_UnlockScreen()
    {
        ScreenStatus = 3;
        RequestSerialization();
        UI_Lockscreen.SetActive(false);
    }

    public void System_DisplayOn()
    {
        UI_Sleep.SetActive(false);
        System_LockScreen();
    }

    public void System_DisplayOff()
    {
        UI_Sleep.SetActive(true);
        ScreenStatus = 0;
        RequestSerialization();
    }

    public void System_GoHome()
    {
        for(int i = 0; i < UI_CurrentApp.transform.childCount; i++)
        {
            ((UdonBehaviour)UI_CurrentApp.transform.GetChild(i).GetComponent(typeof(UdonBehaviour))).SendCustomEvent("harudon_stop");
            UI_CurrentApp.transform.GetChild(i).SetParent(UI_SuspendedApp.transform);
        }
    }
}
