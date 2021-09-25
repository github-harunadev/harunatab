using System;
using UdonSharp;
using UdonSharpEditor;
using UnityEditor;
using UnityEngine;
using VRC.Udon;

public class InstallApp : EditorWindow
{
    public GameObject behaviourObject;
    public GameObject UIObject;

    public GameObject tabSystem;
    public GameObject tabBehaviours;
    public GameObject tabRender;
    public GameObject tabReserved;
    public GameObject tabPointer;
    public GameObject tabCurrentUI;
    public GameObject tabSuspendedUI;
    public void Init()
    {
        behaviourObject = null;
        UIObject = null;
        GetWindow<InstallApp>("Install App").Show();
    }

    void OnGUI()
    {
        GUILayout.Label("This installer associates only required system objects to app. You have to manually associate all the rest of objects with yourself according to the app developer's instruction.", EditorStyles.helpBox);
        GUILayout.Label("Once you've installed the objects to the tablet, you CANNOT UNDO it. You can uninstall it in 'Installed Apps' section.", EditorStyles.helpBox);
        behaviourObject = EditorGUILayout.ObjectField("Behaviour Object / Prefab", behaviourObject, typeof(GameObject), true) as GameObject;
        UIObject = EditorGUILayout.ObjectField("UI Object / Prefab", UIObject, typeof(GameObject), true) as GameObject;
        if (GUILayout.Button("Install"))
        {
            if (behaviourObject != null)
            {
                for (int i = 0; i < tabRender.transform.childCount; i++)
                {
                    GameObject item = tabRender.transform.GetChild(i).gameObject;
                    if (item.name == "Canvas")
                    {
                        for (int i1 = 0; i1 < item.transform.childCount; i1++)
                        {
                            GameObject item1 = item.transform.GetChild(i1).gameObject;
                            if (item1.name == "Main")
                            {
                                for (int i2 = 0; i2 < item1.transform.childCount; i2++)
                                {
                                    GameObject item2 = item1.transform.GetChild(i2).gameObject;
                                    if (item2.name == "Reserved")
                                    {
                                        tabReserved = item2;
                                    }
                                    else if (item2.name == "SuspendedApp")
                                    {
                                        tabSuspendedUI = item2;
                                    }
                                    else if (item2.name == "CurrentApp")
                                    {
                                        tabCurrentUI = item2;
                                    }
                                }
                            }
                        }
                    }
                }

                GameObject newBehaviourObject = (GameObject)PrefabUtility.InstantiatePrefab(behaviourObject);
                newBehaviourObject.transform.SetParent(tabBehaviours.transform);
                newBehaviourObject.transform.localScale = new Vector3(1, 1, 1);
                Selection.activeGameObject = newBehaviourObject;

                UdonSharpBehaviour usb = newBehaviourObject.GetUdonSharpComponent(typeof(UdonSharpBehaviour));
                UdonBehaviour ub = newBehaviourObject.GetComponent<UdonBehaviour>();
                usb.UpdateProxy();
                usb.SetProgramVariable("Pointer", tabPointer.transform.GetChild(0).gameObject);
                usb.SetProgramVariable("UI_CurrentApp", tabCurrentUI);
                usb.SetProgramVariable("UI_SuspendedApp", tabSuspendedUI);

                if (UIObject != null)
                {
                    GameObject newUIObject = (GameObject)PrefabUtility.InstantiatePrefab(UIObject);
                    newUIObject.transform.SetParent(tabReserved.transform);
                    newUIObject.transform.localScale = new Vector3(1, 1, 1);

                    usb.SetProgramVariable("MainUI", newUIObject);

                    UdonSharpBehaviour usb1 = newUIObject.GetUdonSharpComponent(typeof(UdonSharpBehaviour));
                    usb1.UpdateProxy();
                    usb1.SetProgramVariable("MainBehaviour", ub);
                    usb1.ApplyProxyModifications();
                }

                usb.ApplyProxyModifications();
                Close();
            }
        }
    }
}
