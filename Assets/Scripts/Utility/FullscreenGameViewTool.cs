#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.Reflection;

[InitializeOnLoad]
public static class FullscreenGameViewTool
{
    static readonly Type GameViewType = Type.GetType("UnityEditor.GameView,UnityEditor");
    static readonly PropertyInfo ShowToolbarProperty = GameViewType?.GetProperty("showToolbar", BindingFlags.Instance | BindingFlags.NonPublic);
    static EditorWindow instance;

    static FullscreenGameViewTool()
    {
        AssemblyReloadEvents.beforeAssemblyReload += () =>
        {
            instance?.Close();
            instance = null;
        };
    }

    [MenuItem("Tools/Toggle Fullscreen GameView _F11")] // Hotkey: F11
    public static void Toggle()
    {
        if (GameViewType == null)
        {
            Debug.LogError("GameView type not found.");
            return;
        }

        if (instance != null)
        {
            instance.Close();
            instance = null;
        }
        else
        {
            instance = (EditorWindow)ScriptableObject.CreateInstance(GameViewType);
            ShowToolbarProperty?.SetValue(instance, false);
            instance.ShowPopup();
            instance.position = new Rect(Vector2.zero, new Vector2(Screen.currentResolution.width, Screen.currentResolution.height));
            instance.Focus();
        }
    }
}
#endif