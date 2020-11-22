using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjLoader : MonoBehaviour
{
    public Camera c_cam;
    public GameObject obj_Object;
    Rigidbody r_rigyBoi;

    public PhysicMaterial ph_Friction;

    public Slider sdr_ZoomSlider;

    public TMP_InputField iF_Weight;
    public TMP_InputField iF_Scale;
    public TMP_InputField iF_Friction;

    // Start is called before the first frame update
    void Start()
    {
        r_rigyBoi = obj_Object.GetComponent<Rigidbody>();
        ph_Friction = new PhysicMaterial();
    }

    public void RotateLeft()
    {
        obj_Object.transform.Rotate(0, 5.0f, 0);
    }

    public void RotateRight()
    {
        obj_Object.transform.Rotate(0, -5.0f, 0);
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
        //If statement checks to see if the friction text is a float or words, then uses the input or a 1
        if (float.TryParse(iF_Friction.text.ToString(), out value) == true)
        {
            //Sets both dynamic and static friction to the input or base number, can be modified to be seperate but requires another input.
            ph_Friction.staticFriction = float.Parse(iF_Friction.text.ToString());
            ph_Friction.dynamicFriction = float.Parse(iF_Friction.text.ToString());
        } // IMPORTANT NOTE: Must apply physics to the collider before exporting.

        //Camera zoom slider modifies the z value and uses inputted other values.
        c_cam.transform.position = new Vector3(c_cam.transform.position.x, c_cam.transform.position.y, -10 * sdr_ZoomSlider.value);


    }
}
