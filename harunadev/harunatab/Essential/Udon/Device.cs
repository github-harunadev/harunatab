
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Device : UdonSharpBehaviour
{
    public GameObject Pointer;
    public VRC_Pickup ThisPickup;
    public UdonBehaviour System;

    public VRCPlayerApi PickupPlayer = null;

    [UdonSynced] public bool initialized = false;
    public bool initializedSelf = false;
    public bool alreadyInitializedSelf = false;

    private void Update()
    {
        if (PickupPlayer != null)
        {
            if (PickupPlayer.GetBonePosition(HumanBodyBones.LeftIndexDistal) != null && PickupPlayer.GetBonePosition(HumanBodyBones.RightIndexDistal) != null)
            {
                if (PickupPlayer.GetPickupInHand(VRC_Pickup.PickupHand.Left) == ThisPickup)
                {
                    Pointer.transform.SetPositionAndRotation(PickupPlayer.GetBonePosition(HumanBodyBones.RightIndexDistal), Pointer.transform.rotation);
                }
                else
                {
                    Pointer.transform.SetPositionAndRotation(PickupPlayer.GetBonePosition(HumanBodyBones.LeftIndexDistal), Pointer.transform.rotation);
                }
            }
        }
        if (initialized && !initializedSelf && !alreadyInitializedSelf)
        {
            alreadyInitializedSelf = true;
            System.SendCustomEvent("Touch_GoHome");
        }
    }

    public override void OnPickup()
    {
        PickupPlayer = ThisPickup.currentPlayer;
    }

    public override void OnDrop()
    {
        PickupPlayer = null;
    }

    public override void OnPickupUseDown()
    {
        initialized = true;
        initializedSelf = true;
        alreadyInitializedSelf = true;
        System.SendCustomEvent("Touch_GoHome");
    }
}
