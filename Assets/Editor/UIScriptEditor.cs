using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public static class UIScriptEditor
{
    static UIScriptEditor()
    {
        EditorApplication.hierarchyChanged += AddCanvasGroupToUIScripts;
    }

    static void AddCanvasGroupToUIScripts()
    {
        UIScript[] uiScripts = GameObject.FindObjectsOfType<UIScript>();
        foreach (UIScript uiScript in uiScripts)
        {
            if (!uiScript.GetComponent<CanvasGroup>())
            {
                uiScript.gameObject.AddComponent<CanvasGroup>();
                Debug.Log("Added CanvasGroup to " + uiScript.gameObject.name);
            }
        }
    }
}