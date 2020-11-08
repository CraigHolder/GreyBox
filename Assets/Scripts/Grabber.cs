using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    public PlayerMovement s_player;
    public GameObject obj_curritem;
    public Collider col_collider;

    // Start is called before the first frame update
    void Start()
    {
        s_player = this.GetComponentInParent<PlayerMovement>();

        col_collider = this.GetComponent<BoxCollider>();

        obj_curritem = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (s_player.b_isgrabbing == true)
        {
            col_collider.enabled = true;
        }
        else
        {
            col_collider.enabled = false;
            if (obj_curritem != null)
            {
                obj_curritem.GetComponent<Rigidbody>().isKinematic = false;
                obj_curritem.GetComponent<Collider>().isTrigger = false;
                obj_curritem.transform.SetParent(null);
            }
           
        }
    }

    void OnTriggerStay(Collider collision)
    {
        if(collision.gameObject.tag == "Grab")
        {
            obj_curritem = collision.gameObject;
            obj_curritem.GetComponent<Rigidbody>().isKinematic = true;
            obj_curritem.transform.SetParent(s_player.gameObject.transform);
            obj_curritem.GetComponent<Collider>().isTrigger = true;
        }
    }
}
