using UnityEngine;
using UnityEditor;

public static class RemoveAllScriptsMenu
{
    [MenuItem("Tools/Remove All Scripts From Selected")]
    private static void RemoveScripts()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            var scripts = obj.GetComponents<MonoBehaviour>();
            foreach (var s in scripts)
            {
                Undo.DestroyObjectImmediate(s);
            }
        }
    }
}