using UdonSharp;
using UdonSharpEditor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ManageHarunatab : EditorWindow
{
    GameObject harunatabObject;
    bool harunatabRegistered = false;
    Vector2 scrollPos;
    bool showApps = false;
    bool showSetting = false;

    Sprite wallpaper = null;
    Sprite previousWallpaper = null;

    [MenuItem("harunatab/Manage")] public static void Init()
    {
        GetWindow<ManageHarunatab>("Manage harunatab").Show();
    }
    void OnGUI()
    {
        GUILayout.Label("Manage harunatab", EditorStyles.largeLabel);
        GUILayout.Label("harunatab management version 1", EditorStyles.miniLabel);
        EditorGUI.BeginDisabledGroup(harunatabRegistered == true);
        harunatabObject = EditorGUILayout.ObjectField("harunatab Object", harunatabObject, typeof(GameObject), true) as GameObject;
        EditorGUI.EndDisabledGroup();

        if (harunatabRegistered && harunatabObject != null)
        {
            GameObject tabSystem = null;
            GameObject tabINIT = null;
            GameObject tabBehaviours = null;
            GameObject tabRender = null;
            GameObject tabPointer = null;

            for (int i = 0; i < harunatabObject.transform.childCount; i++)
            {
                GameObject item = harunatabObject.transform.GetChild(i).gameObject;
                if (item.name == "Behaviours")
                {
                    tabBehaviours = item;
                } else if (item.name == "System")
                {
                    tabSystem = item;
                } else if (item.name == "INIT")
                {
                    tabINIT = item;
                } else if (item.name == "Render")
                {
                    tabRender = item;
                } else if (item.name == "PointerOrigin")
                {
                    tabPointer = item;
                }
            }

            if (GUILayout.Button("Unregister"))
            {
                harunatabRegistered = false;
                harunatabObject = null;
            }

            if (tabSystem != null && tabINIT != null && tabBehaviours != null)
            {
                if (PrefabUtility.GetPrefabInstanceHandle(harunatabObject) == null)
                {
                    GUILayout.Label("This tablet is not a prefab, which means you have to manually replace the files when you're updating into new tablet.", EditorStyles.helpBox);
                }
                showApps = EditorGUILayout.BeginFoldoutHeaderGroup(showApps, "Installed Apps");
                if (showApps)
                {
                    if (GUILayout.Button("Install new app"))
                    {
                        InstallApp installAppWindow = ScriptableObject.CreateInstance<InstallApp>();
                        installAppWindow.tabSystem = tabSystem;
                        installAppWindow.tabBehaviours = tabBehaviours;
                        installAppWindow.tabRender = tabRender;
                        installAppWindow.tabPointer = tabPointer;
                        installAppWindow.Init();
                    }
                    scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(256));
                    for (int i = 0; i < tabBehaviours.transform.childCount; i++)
                    {
                        GameObject item = tabBehaviours.transform.GetChild(i).gameObject;
                        if (item.name != "INIT" && item.name != "Home")
                        {
                            Rect r = EditorGUILayout.BeginHorizontal("Button");
                            if (GUI.Button(r, GUIContent.none))
                            {
                                ManageApp manageAppWindow = ScriptableObject.CreateInstance<ManageApp>();
                                manageAppWindow.appObject = item;
                                manageAppWindow.harunatabObject = harunatabObject;
                                manageAppWindow.Init();
                            }
                            string labelt = "";
                            if (!item.activeSelf) labelt += "*Disabled*     ";
                            GUILayout.Label(labelt + item.name, EditorStyles.miniLabel);
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                    EditorGUILayout.EndScrollView();
                }
                EditorGUILayout.EndFoldoutHeaderGroup(); 
                
                showSetting = EditorGUILayout.BeginFoldoutHeaderGroup(showSetting, "Tablet Settings");
                if (showSetting)
                {
                    if (wallpaper == null)
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
                                            if (item2.name == "Wallpaper")
                                            {
                                                wallpaper = item2.GetComponent<Image>().sprite;
                                                previousWallpaper = item2.GetComponent<Image>().sprite;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }











                    wallpaper = EditorGUILayout.ObjectField("Wallpaper", wallpaper, typeof(Sprite), false) as Sprite;
                    if (wallpaper != previousWallpaper)
                    {
                        if (GUILayout.Button("Apply Wallpaper"))
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
                                                if (item2.name == "Wallpaper")
                                                {
                                                    item2.GetComponent<Image>().sprite = wallpaper;
                                                    previousWallpaper = wallpaper;

                                                    Undo.RecordObject(item2, "Modify Wallpaper");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                EditorGUILayout.EndFoldoutHeaderGroup();

            } else
            {
                GUILayout.Label("This harunatab object is corrupt.", EditorStyles.helpBox);
            }

        }
        else
        {
            wallpaper = null;
            if (GUILayout.Button("Manage this tablet"))
            {
                if (harunatabObject != null)
                {
                    if (EditorUtility.IsPersistent(harunatabObject))
                    {
                        if (EditorUtility.DisplayDialog("This is not a scene object", "Please drop the object that exist in scene.", "OK"))
                        {

                        }
                    }
                    else
                    {
                        bool BehavioursExist = false;
                        bool SystemExist = false;
                        bool RenderExist = false;
                        bool INITExist = false;
                        bool TouchDetectionExist = false;

                        for (int i = 0; i < harunatabObject.transform.childCount; i++)
                        {
                            GameObject item = harunatabObject.transform.GetChild(i).gameObject;
                            if (item.name == "Behaviours")
                            {
                                BehavioursExist = true;
                            }
                            else if (item.name == "System")
                            {
                                SystemExist = true;
                            }
                            else if (item.name == "Render")
                            {
                                RenderExist = true;
                            }
                            else if (item.name == "INIT")
                            {
                                INITExist = true;
                            }
                            else if (item.name == "TouchDetection")
                            {
                                TouchDetectionExist = true;
                            }
                        }

                        if (BehavioursExist && SystemExist && RenderExist && INITExist && TouchDetectionExist)
                        {
                            harunatabRegistered = true;
                        }
                        else
                        {
                            if (EditorUtility.DisplayDialog("This is not a valid harunatab", "The object you've dropped doesn't seems to be a valid harunatab object.", "OK"))
                            {

                            }
                        }
                    }
                } else
                {
                    if (EditorUtility.DisplayDialog("Object null", "Please drop the harunatab in the field.", "OK"))
                    {

                    }
                }


            }
        }
    }
}
