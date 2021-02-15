using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabDetect : MonoBehaviour
{
    public Grabber s_Grabscript;
    public GameObject obj_exclaim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Grab" && s_Grabscript.obj_curritem == null)
        {
            obj_exclaim.SetActive(true);
        }
        else
        {
            obj_exclaim.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        
        obj_exclaim.SetActive(false);
        
    }
}
