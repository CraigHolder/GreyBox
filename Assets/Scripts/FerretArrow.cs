using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FerretArrow : MonoBehaviour
{
    //public MeshRenderer obj_tip;
    //public GameObject obj_ctrl;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<MeshRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider collision)
    {
        
        //this.GetComponent<MeshRenderer>().enabled = false;
        //obj_tip.enabled = false;
        
        if (collision.gameObject.tag == "Important")
        {
            this.GetComponent<MeshRenderer>().enabled = true;
            //obj_tip.enabled = true;
            this.transform.LookAt(collision.gameObject.transform);

        }
    }
    void OnTriggerExit(Collider collision)
    {

        
        //obj_tip.enabled = false;

        if (collision.gameObject.tag == "Important")
        {
            this.GetComponent<MeshRenderer>().enabled = false;

        }
    }
}
