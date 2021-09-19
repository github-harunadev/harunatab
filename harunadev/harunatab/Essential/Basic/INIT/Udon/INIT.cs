
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class INIT : UdonSharpBehaviour
{
    // INIT by harunadev
    public Text Log;
    public UdonBehaviour SystemBehaviour;
    public GameObject Behaviours;

    public string[] inits;
    private int count = 0;

    public void harudon_verify()
    {
        Log.text += "\n------------ INIT verification will run in init \n\n";
    }
    public void harudon_init()
    {
        count = 0;

        Log.text += "\n\n------------ INIT ------------\nSomething probably similar to init.d but absolutely no haha\nMade by harunadev\n------------------------------\n\n";

        scan_start();
    }

    public void scan_start()
    {
        repeatScanINIT();
    }

    public void repeatScanINIT()
    {
        string taskname = inits[count].Substring(0, inits[count].IndexOf(";"));
        string taskruntime = inits[count].Substring(inits[count].IndexOf(";") + 1, inits[count].Length - inits[count].IndexOf(";") - 1);

        if (taskname == "INIT")
        {
            ThrowException("InitOnInitException");
        } else
        {
            byte found = 0;
            for (int i = 0; i < Behaviours.transform.childCount; i++)
            {
                GameObject findBehaviour = Behaviours.transform.GetChild(i).gameObject;
                if (findBehaviour.name == taskname)
                {
                    found = 1;
                    Log.text += "[ INIT ] Starting " + taskname + "...";
                    UdonBehaviour targetBehaviour = (UdonBehaviour)findBehaviour.GetComponent(typeof(UdonBehaviour));
                    if (targetBehaviour == null)
                    {
                        ThrowException("BehaviourNullException");
                    } else
                    {
                        targetBehaviour.SendCustomEvent("harudon_init");
                    }
                }
            }
            if (found == 0)
            {
                ThrowException("BehaviourNotFoundException");
            } else
            {
                if (count == inits.Length - 1)
                {
                    Log.text += "\n\n------------ INIT TASK END ------------\n\n";
                    SystemBehaviour.SendCustomEvent("System_EndINIT");
                } else
                {
                    count++;
                    repeatScanINIT();
                }
            }
        }
    }

    public void ThrowException(string exception)
    {
        if (exception == "InitOnInitException") Log.text += "\n\n--------- INIT ERROR ---------\nException [InitOnInitException]: Trying to start INIT task in INIT, which will occur infinite loop.\n\nFurther tasks are stopped. \n------------------------------\n\n";
        if (exception == "BehaviourNotFoundException") Log.text += "\n\n--------- INIT ERROR ---------\nException [BehaviourNotFoundException]: Listed INIT tasks are not found, or not available.\n\nFurher tasks are stopped. \n------------------------------\n\n";
        if (exception == "BehaviourNullException") Log.text += "\n\n--------- INIT ERROR ---------\nException [BehaviourNullException]: Behaviour component from target INIT task is not found, or not available.\n\nFurher tasks are stopped. \n------------------------------\n\n";

        SystemBehaviour.SendCustomEvent("ThrowCriticalError");
    }
}
