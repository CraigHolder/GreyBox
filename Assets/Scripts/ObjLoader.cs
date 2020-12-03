using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;
using UnityEngine.Windows;

//References:
// https://forum.unity.com/threads/creating-prefabs-from-models-by-script.606760/
// https://www.youtube.com/watch?v=Vh_XkNwThg4&feature=emb_title
// https://answers.unity.com/questions/881890/load-material-from-assets.html

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
    public Toggle t_togglyBoi;

    private string pathway;
    private string pathway2;
    private string pathway3;
    private Material m_Mat;
    public RawImage r_Tex;
    public Texture t_Texture;

    private static string basePath = "Assets/Resources/Prefabs/";

    // Start is called before the first frame update
    void Start()
    {
        r_rigyBoi = obj_Object.GetComponent<Rigidbody>();
        m_Mat = obj_Object.GetComponent<MeshRenderer>().material;
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

    public void OpenExplorer()
    {
        pathway = EditorUtility.OpenFilePanel("Overwrite with png", "", "png");
        GetMat();
    }

    public void OpenModelExplorer()
    {
        pathway2 = EditorUtility.OpenFilePanel("Overwrite with png", "", "obj");
        GetMod();
    }

    public void Load()
    {
        //pathway3 = EditorUtility.OpenFilePanel("Load Prefab", "", "prefab");
        if (Resources.Load<GameObject>("Prefabs/EditablePrefabs/" + iF_ObjName.text + "_Variant") != null)
        {
            Destroy(obj_Object);
            obj_Object = Instantiate(Resources.Load<GameObject>("Prefabs/EditablePrefabs/" + iF_ObjName.text + "_Variant"));
            r_rigyBoi = obj_Object.GetComponent<Rigidbody>();
            obj_Object.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Prefabs/CustomMats/" + iF_ObjName.text + "_Material");
            r_Tex.texture = Resources.Load<Texture>("Prefabs/ImportedTextures/" + iF_ObjName.text + "_Texture");
            m_Mat = obj_Object.GetComponent<MeshRenderer>().material;
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public void New()
    {
        //pathway3 = EditorUtility.OpenFilePanel("Load Prefab", "", "prefab");
        if (Resources.Load<GameObject>("Prefabs/EditablePrefabs/Placeholder_Obj") != null)
        {
            Destroy(obj_Object);
            obj_Object = Instantiate(Resources.Load<GameObject>("Prefabs/EditablePrefabs/Placeholder_Obj"));
            r_rigyBoi = obj_Object.GetComponent<Rigidbody>();
            m_Mat = obj_Object.GetComponent<MeshRenderer>().material;
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    void GetMod()
    {
        if (pathway2 != null)
        {
            AssetDatabase.Refresh();
            if (File.Exists(basePath + "ImportedModels/" + iF_ObjName.text + "_Model.obj") == false)
            {
                SaveModel(pathway2);
                UpdateMod();
            }
            else
            {
                Debug.Log("GetMod() Failed! Name already in use!");
            }
        }
    }

    public void SaveModel(string filePath)
    {
        AssetDatabase.Refresh();
        FileUtil.CopyFileOrDirectory(filePath, basePath + "ImportedModels/" + iF_ObjName.text + "_Model.obj");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    void UpdateMod()
    {
        AssetDatabase.Refresh();
        obj_Object.GetComponent<MeshFilter>().mesh = Resources.Load<Mesh>("Prefabs/ImportedModels/" + iF_ObjName.text + "_Model");
    }

    void GetMat()
    {
        if (pathway != null)
        {
            AssetDatabase.Refresh();
            if (File.Exists(basePath + "ImportedTextures/" + iF_ObjName.text + "_Texture.png") == false)
            {
                SaveTexture(pathway);
                CreateMat();
                UpdateMat();
            }
            else
            {
                Debug.Log("GetMat() Failed! Name already in use!");
            }
        }
    }

    public void SaveTexture(string filePath)
    {
        FileUtil.CopyFileOrDirectory(filePath, basePath + "ImportedTextures/" + iF_ObjName.text + "_Texture.png");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    void UpdateMat()
    {
        AssetDatabase.Refresh();
        WWW www = new WWW("file:///" + basePath + "ImportedTextures/" + iF_ObjName.text + "_Texture.png");
        r_Tex.texture = www.texture;

        obj_Object.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Prefabs/CustomMats/" + iF_ObjName.text + "_Material");
    }
    void CreateMat()
    {
        Material material = new Material(Shader.Find("Standard"));

        //Mesh mesh = new Mesh();
        //mesh = obj_Object.GetComponent<MeshFilter>().mesh;
        //AssetDatabase.CreateAsset(mesh, basePath + "CustomMods/" + iF_ObjName.text + "_Model.obj");


        material.SetTexture("_MainTex", (Texture)AssetDatabase.LoadAssetAtPath(basePath + "ImportedTextures/" + iF_ObjName.text + "_Texture.png", typeof(Texture)));
        //(Texture)AssetDatabase.LoadAssetAtPath(basePath + "ImportedTextures/" + iF_ObjName.text + "_Texture.png", typeof(Texture))
        AssetDatabase.CreateAsset(material, basePath + "CustomMats/" + iF_ObjName.text + "_Material.mat");


        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    //void SaveMat()
    //{
    //    AssetDatabase.Refresh();
    //    Material material = new Material(Shader.Find("Standard"));
    //    material.SetTexture("_MainTex", (Texture)AssetDatabase.LoadAssetAtPath(basePath + "ImportedTextures/" + iF_ObjName.text + "_Texture.png", typeof(Texture)));
    //    //(Texture)AssetDatabase.LoadAssetAtPath(basePath + "ImportedTextures/" + iF_ObjName.text + "_Texture.png", typeof(Texture))
    //
    //    AssetDatabase.SaveAssets();
    //    AssetDatabase.Refresh();
    //}

    public void SavePrefab()
    {
        //var v_prefabVarient = PrefabUtility.SaveAsPrefabAssetAndConnect(obj_Object, basePath + "EditablePrefabs/" + iF_ObjName.text + "_Variant.prefab", InteractionMode.UserAction);
        obj_Object.GetComponent<Rigidbody>().useGravity = t_togglyBoi.isOn;
        obj_Object.GetComponent<Rigidbody>().isKinematic = !t_togglyBoi.isOn;
        if (File.Exists(basePath + "EditablePrefabs/" + iF_ObjName.text + "_Variant.prefab"))
        {
            File.Delete(basePath + "EditablePrefabs/" + iF_ObjName.text + "_Variant.prefab");
        }
        var v_prefabVarient = PrefabUtility.SaveAsPrefabAsset(obj_Object, basePath + "EditablePrefabs/" + iF_ObjName.text + "_Variant.prefab");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
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

        //Camera zoom slider modifies the z value and uses inputted other values.
        c_cam.transform.position = new Vector3(c_cam.transform.position.x, c_cam.transform.position.y, -10 * (sdr_ZoomSlider.value * 2));


    }
}

// Garbage Code:
/*
 
    void SaveMod()
    {
        //Material material = new Material(Shader.Find("Standard"));

        //Mesh mesh = new Mesh();
        //mesh = obj_Object.GetComponent<MeshFilter>().mesh;
        //AssetDatabase.CreateAsset(mesh, basePath + "CustomMods/" + iF_ObjName.text + "_Model.obj");


        //material.SetTexture("_MainTex", (Texture)AssetDatabase.LoadAssetAtPath(basePath + "ImportedTextures/" + iF_ObjName.text + "_Texture.png", typeof(Texture)));
        ////(Texture)AssetDatabase.LoadAssetAtPath(basePath + "ImportedTextures/" + iF_ObjName.text + "_Texture.png", typeof(Texture))
        //AssetDatabase.CreateAsset(material, basePath + "CustomMats/" + iF_ObjName.text + "_Material.mat");


        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    /////////////////////////////////////
    Update():
     //if (float.TryParse(iF_Friction.text.ToString(), out value) == true)
     //{
     //    //Sets both dynamic and static friction to the input or base number, can be modified to be seperate but requires another input.
     //    ph_Friction.staticFriction = float.Parse(iF_Friction.text.ToString());
     //    ph_Friction.dynamicFriction = float.Parse(iF_Friction.text.ToString());
     //    obj_Object.GetComponent<Collider>().material = ph_Friction;
     //} // IMPORTANT NOTE: Must apply physics to the collider before exporting.

    /////////////////////////////////////
    SavePrefab():
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

    /////////////////////////////////////
    UpdateMat():
     //r_Tex.texture = Resources.Load<Texture>("Prefabs/ImportedTextures/" + iF_ObjName.text + "_Texture.png");
            //m_Mat.mainTexture = r_Tex.texture;

     //obj_Object.GetComponent<MeshRenderer>().material.mainTexture = r_Tex.mainTexture;
        //gameObject.GetComponent<MeshRenderer>().material = lit;

    //void GetTex()
    //{
    //    if (pathway != null)
    //    {
    //        UpdateTex();
    //    }
    //}
    //
    //void UpdateTex()
    //{
    //    WWW www = new WWW("file:///" + pathway);
    //    tex.texture = www.texture;
    //}

    /////////////////////////////////////
    UpdateMod():
    //WWW www = new WWW("file:///" + basePath + "ImportedModels/" + iF_ObjName.text + "_Model.obj");
        //r_Tex.texture = www.texture;
        //Mesh fuck = Instantiate(Resources.Load<Mesh>("Prefabs/ImportedModels/" + iF_ObjName.text + "_Model"));

    //obj_Object.GetComponent<MeshFilter>().mesh = Resources.Load<Mesh>("Prefabs/CustomMods/" + iF_ObjName.text + "_Model");
        //obj_Object.GetComponent<MeshFilter>().mesh = (Mesh)Resources.Load("Prefabs/CustomMods/" + iF_ObjName.text + "_Model", typeof(Mesh));
        //obj_Object.GetComponent<MeshRenderer>().sharedMaterial = Resources.Load<Mesh>("Prefabs/CustomMods/" + iF_ObjName.text + "_Model");
        //obj_Object.GetComponent<MeshRenderer>().enabled = true;

        //r_Tex.texture = Resources.Load<Texture>("Prefabs/ImportedTextures/" + iF_ObjName.text + "_Texture.png");
        //m_Mat.mainTexture = r_Tex.texture;
        //SaveMod();

        //obj_Object.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Prefabs/CustomMats/" + iF_ObjName.text + "_Material");
        //obj_Object.GetComponent<MeshRenderer>().material.mainTexture = r_Tex.mainTexture;
        //gameObject.GetComponent<MeshRenderer>().material = lit;

     */
