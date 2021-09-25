using UdonSharp;
using UdonSharpEditor;
using UnityEditor;
using UnityEngine;
using VRC.Udon;

public class ManageApp : EditorWindow
{
    public GameObject appObject;
    public GameObject harunatabObject;

    public void Init()
    {
        if (appObject != null) GetWindow<ManageApp>("Manage App").Show();
    }

    void OnGUI()
    {
        if (appObject != null)
        {
            if (appObject.GetUdonSharpComponent(typeof(UdonSharpBehaviour)).GetProgramVariable("HarudonIcon") != null) GUILayout.Label(((Sprite)appObject.GetUdonSharpComponent(typeof(UdonSharpBehaviour)).GetProgramVariable("HarudonIcon")).texture, GUILayout.Width(100), GUILayout.Height(100));
            if (appObject.GetUdonSharpComponent(typeof(UdonSharpBehaviour)).GetProgramVariable("HarudonName") != null) GUILayout.Label((string)appObject.GetUdonSharpComponent(typeof(UdonSharpBehaviour)).GetProgramVariable("HarudonName"), EditorStyles.largeLabel);
            GUILayout.Label(appObject.name, EditorStyles.miniLabel);
            if (PrefabUtility.GetPrefabInstanceHandle(appObject) == PrefabUtility.GetPrefabInstanceHandle(harunatabObject)) GUILayout.Label("This app is associated with harunatab, which means you cannot uninstall this app unless you unpack the prefab. I highly recommend to disable the behaviour, which works almost same as uninstalling the app. (Disabled apps are excluded from verification & doesn't show up in Home screen)", EditorStyles.helpBox);

            appObject.SetActive(EditorGUILayout.Toggle("Enable", appObject.activeSelf));
            EditorGUI.BeginDisabledGroup(PrefabUtility.GetPrefabInstanceHandle(appObject) == PrefabUtility.GetPrefabInstanceHandle(harunatabObject));
            if (GUILayout.Button("Uninstall & Remove"))
            {
                if (EditorUtility.DisplayDialog("Before removing the behaviour...", "You may have to remove all associated objects on yourself by navigating through the behaviour BEFORE uninstalling the behaviour. Press 'No' to go back and start removing stuffs yourself. If you press 'Yes', the behaviour will be removed from tablet and you CANNOT UNDO this action.", "Yes", "No"))
                {
                    DestroyImmediate(appObject);
                    Close();
                }
            }
            EditorGUI.EndDisabledGroup();
        } else
        {
            GUILayout.Label("Cannot read App data", EditorStyles.helpBox);
        }
    }
}
