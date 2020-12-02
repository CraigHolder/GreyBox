using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

//References:
// https://forum.unity.com/threads/creating-prefabs-from-models-by-script.606760/

public class ObjLoader : MonoBehaviour
{
    public Camera c_cam;
    public GameObject obj_Object;
    Rigidbody r_rigyBoi;

   // public PhysicMaterial ph_Friction;

    public Slider sdr_ZoomSlider;

    public TMP_InputField iF_Weight;
    public TMP_InputField iF_Scale;
    public TMP_InputField iF_Drag;
    public TMP_InputField iF_ObjName;

    // Start is called before the first frame update
    void Start()
    {
        r_rigyBoi = obj_Object.GetComponent<Rigidbody>();
        //ph_Friction = new PhysicMaterial();
    }

    public void RotateLeft()
    {
        obj_Object.transform.Rotate(0, 5.0f, 0);
    }

    public void RotateRight()
    {
        obj_Object.transform.Rotate(0, -5.0f, 0);
    }

    public void SavePrefab()
    {
        //v_objectPath is saving the designated path for the assets to be saved in and the file type used.
        //var v_objectPath = (GameObject)AssetDatabase.LoadMainAssetAtPath("Assets/Prefabs/" + iF_ObjName + ".fbx"); //The .fbx means this is limited to creating EXCLUSIVELY .fbx files.
        //v_instantiateObj is instantiating the prefab before creating it so that there will not be any problems with the creation.
        //var v_instantiateObj = (GameObject)PrefabUtility.InstantiatePrefab(obj_Object);
        //v_instantiateObj.AddComponent<Rigidbody>();
        //v_instantiateObj.GetComponent<Rigidbody>().mass = float.Parse(iF_Weight.text);
        //v_instantiateObj.AddComponent<MeshCollider>();
        //v_instantiateObj.GetComponent<MeshCollider>().convex = true;
        //v_prefabObj is finally saving the obj as a prefab in the designated spot and setting their name.
        //PrefabUtility.
        //var v_prefabVarient = PrefabUtility.SaveAsPrefabAsset(ph_Friction, "Assets/Prefabs/" + iF_ObjName.text + "_Friction.prefab");

        var v_prefabVarient = PrefabUtility.SaveAsPrefabAsset(obj_Object, "Assets/Prefabs/" + iF_ObjName.text + "_Variant.prefab");
    }

    // Update is called once per frame
    void Update()
    {
        //value represents a base value that strings are set to if it's not a number.
        float value = 1.0f;
        //If statement checks to see if the scale text is a float or words, then uses the input or a 1
        if(float.TryParse(iF_Scale.text.ToString(), out value) == true)
        {
            //Sets the indicated objects scale(xyz) to the input or base value.
            obj_Object.transform.localScale = new Vector3(float.Parse(iF_Scale.text.ToString()), float.Parse(iF_Scale.text.ToString()), float.Parse(iF_Scale.text.ToString()));
        }
        //If statement checks to see if the weight text is a float or words, then uses the input or a 1
        if (float.TryParse(iF_Weight.text.ToString(), out value) == true)
        {
            //Sets the rigidbody's mass value to the input or 1.
            r_rigyBoi.mass = float.Parse(iF_Weight.text.ToString());
        }
        //If statement checks to see if the drag text is a float or words, then uses the input or a 1
        if (float.TryParse(iF_Drag.text.ToString(), out value) == true)
        {
            //Sets the rigidbody's drag value to the input or 1.
            r_rigyBoi.drag = float.Parse(iF_Drag.text.ToString());
        }
        //if (float.TryParse(iF_Friction.text.ToString(), out value) == true)
        //{
        //    //Sets both dynamic and static friction to the input or base number, can be modified to be seperate but requires another input.
        //    ph_Friction.staticFriction = float.Parse(iF_Friction.text.ToString());
        //    ph_Friction.dynamicFriction = float.Parse(iF_Friction.text.ToString());
        //    obj_Object.GetComponent<Collider>().material = ph_Friction;
        //} // IMPORTANT NOTE: Must apply physics to the collider before exporting.

        //Camera zoom slider modifies the z value and uses inputted other values.
        c_cam.transform.position = new Vector3(c_cam.transform.position.x, c_cam.transform.position.y, -10 * sdr_ZoomSlider.value);


    }
}
