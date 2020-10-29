using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    PlayerMovement s_player;
    GameObject obj_curritem;
    // Start is called before the first frame update
    void Start()
    {
        s_player = this.GetComponentInParent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void OnTriggerStay(Collider collision)
    {
        if(collision.gameObject.tag == "Grab")
        {
            obj_curritem = collision.gameObject;
            obj_curritem.GetComponent<Rigidbody>().detectCollisions = false;
            obj_curritem.transform.SetParent(s_player.gameObject.transform);
        }
    }
}
