using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

public class CustomMenu : MonoBehaviour
{
    [MenuItem("Custom/HotKey/SetActive %q")]
    public static void SetActive()
    {
        GameObject[] gameObjects = Selection.gameObjects;

        for(int i = 0; i < gameObjects.Length; i++)
        {
            gameObjects[i].SetActive(!gameObjects[i].activeSelf);
            EditorUtility.SetDirty(gameObjects[i]);
        }
    }

    [MenuItem("Custom/HotKey/CopyTransform %w")]
    public static void CopyTransform()
    {
        GameObject[] gameObjects = Selection.gameObjects;

        if (gameObjects.Length <= 0) return;

        ComponentUtility.CopyComponent(gameObjects[0].transform);
    }

    [MenuItem("Custom/HotKey/PasteTransform %#w")]
    public static void PasteTransform()
    {
        GameObject[] gameObjects = Selection.gameObjects;

        for(int i = 0; i < gameObjects.Length; i++)
        {
            ComponentUtility.PasteComponentValues(gameObjects[i].transform);    
        }
    }
}
