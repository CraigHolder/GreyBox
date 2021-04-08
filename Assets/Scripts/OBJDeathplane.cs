using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBJDeathplane : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Grab")
        {
            collision.gameObject.transform.position += new Vector3(0,80,0);
        }

    }
}
