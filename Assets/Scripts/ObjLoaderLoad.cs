using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;


//References:
// https://docs.unity3d.com/ScriptReference/EditorUtility.OpenFilePanel.html

public class ObjLoaderLoad : MonoBehaviour
{
    string pathway;
    public RawImage tex;

    public void OpenExplorer()
    {
        pathway = EditorUtility.OpenFilePanel("Overwrite with png", "", "png");
    }
}
