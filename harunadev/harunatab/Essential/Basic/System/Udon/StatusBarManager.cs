
using System;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class StatusBarManager : UdonSharpBehaviour
{
    public GameObject TabletBody;

    public Text UI_StatusBar_Clock;

    public Text UI_StatusBarExpand_Clock;
    public Text UI_StatusBarExpand_Owner;
    public Text UI_StatusBarExpand_PlayerList_Total;
    public Text UI_StatusBarExpand_PlayerList_List;

    public Text UI_LockScreen_Clock;

    private VRCPlayerApi[] Players = new VRCPlayerApi[128];

    private void FixedUpdate()
    {
        UI_StatusBar_Clock.text = DateTime.Now.ToString("HH:mm tt");
        UI_StatusBarExpand_Clock.text = DateTime.Now.ToString("HH:mm:ss") + " <size=70>" + DateTime.Now.ToString("tt") + "</size>\n<size=30>" + DateTime.Now.ToString("D") + "</size>";
        UI_LockScreen_Clock.text = DateTime.Now.ToString("HH:mm:ss") + " <size=70>" + DateTime.Now.ToString("tt") + "</size>\n<size=30>" + DateTime.Now.ToString("D") + "</size>";

        if (Networking.GetOwner(TabletBody) != null)
        {
            UI_StatusBarExpand_Owner.text = Networking.GetOwner(TabletBody).displayName;
            Players = VRCPlayerApi.GetPlayers(Players);
            byte count = 0;
            UI_StatusBarExpand_PlayerList_List.text = "";
            foreach (VRCPlayerApi individual in Players)
            {
                if (individual != null)
                {
                    count++;
                    UI_StatusBarExpand_PlayerList_List.text += individual.displayName + " (";
                    if (individual.IsUserInVR())
                    {
                        UI_StatusBarExpand_PlayerList_List.text += "VR)\n";
                    } else
                    {
                        UI_StatusBarExpand_PlayerList_List.text += "PC)\n";
                    }
                }
            }
            UI_StatusBarExpand_PlayerList_Total.text = "Total " + Convert.ToString(count) + " Player(s)";
        }
    }
}
