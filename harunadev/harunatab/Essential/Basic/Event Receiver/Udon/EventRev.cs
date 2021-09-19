
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;


public class EventRev : UdonSharpBehaviour
{
    public UdonBehaviour MainBehaviour;

    public void harudon_verify()
    {
        MainBehaviour.SendCustomEvent("harudon_verify");
    }

    public void harudon_init()
    {
        MainBehaviour.SendCustomEvent("harudon_init");
    }

    public void harudon_stop()
    {
        MainBehaviour.SendCustomEvent("harudon_stop");
    }

    public void harudon_lateinit()
    {
        MainBehaviour.SendCustomEvent("harudon_lateinit");
    }

    public void Touch_Down()
    {
        MainBehaviour.SendCustomEvent("Touch_Down");
    }

    public void Touch_Up()
    {
        MainBehaviour.SendCustomEvent("Touch_Up");
    }

    public void Touch_Stay()
    {
        MainBehaviour.SendCustomEvent("Touch_Stay");
    }
}
