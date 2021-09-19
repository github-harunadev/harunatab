
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class HomeEventReceiver : UdonSharpBehaviour
{
    public UdonBehaviour MainUdon;

    public void Touch_Down()
    {
        MainUdon.SendCustomEvent("Touch_Down");
    }

    public void Touch_Up()
    {
        MainUdon.SendCustomEvent("Touch_Up");
    }

    public void Touch_Stay()
    {
        MainUdon.SendCustomEvent("Touch_Stay");
    }
}
