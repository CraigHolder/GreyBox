using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    public GameObject s_player;
    public GameObject obj_curritem;

    public CapsuleCollider col_frontcontrol;
    
    public Collider col_collider;
    public GameObject obj_mouth;
    int i_playerID;

    public Vector3 vec3_grabloc;
    public Vector3 vec3_grabrot;
    public Transform t_wasd;
    public Transform t_itemhandle;
    Subject S_Notifier = new Subject();
    Achievments achievmentobserver = new Achievments();

	private bool b_isgrabbing;
	private bool grab_pressed = false;

	// Start is called before the first frame update
	void Start()
    {
        //s_player = this.GetComponentInParent<PlayerMovement>();
        i_playerID = s_player.GetComponent<player_controller_behavior>().i_playerID;
        col_collider = this.GetComponent<BoxCollider>();

        obj_curritem = null;

        S_Notifier.AddObserver(achievmentobserver);
    }

    // Update is called once per frame
    void Update()
    {
        //Grab
        switch (i_playerID)
            {
            case 1:
                if (Input.GetButton("Grab"))
                {
                    if (!grab_pressed)
                    {
                        b_isgrabbing = !b_isgrabbing;
                        grab_pressed = true;
                        t_itemhandle.position = obj_mouth.transform.position;

                    }
                }
                else
                {
                    if (grab_pressed)
                        grab_pressed = false;
                }
                break;
            case 2:
                if (Input.GetButton("Grab2"))
                {
                    if (!grab_pressed)
                    {
                        b_isgrabbing = !b_isgrabbing;
                        grab_pressed = true;
                        t_itemhandle.position = obj_mouth.transform.position;

                    }
                }
                else
                {
                    if (grab_pressed)
                        grab_pressed = false;
                }
                break;
        }

        


		if (b_isgrabbing == true)
        {
            col_collider.enabled = true;
        }
        else
        {
            col_collider.enabled = false;
            if (obj_curritem != null)
            {
                //obj_curritem.GetComponent<Rigidbody>().isKinematic = false;
                //obj_mouth.GetComponent<ConfigurableJoint>().connectedBody = null;
                t_itemhandle.gameObject.GetComponent<FixedJoint>().connectedBody = null;
                s_player.GetComponent<player_controller_behavior>().CURR_PLAYER_SPEED = s_player.GetComponent<player_controller_behavior>().PLAYER_SPEED;
                obj_curritem.layer = 0;
                //obj_curritem.GetComponent<Collider>().isTrigger = false;
                //obj_curritem.transform.SetParent(null);
                obj_curritem = null;
            }

        }
    }

    void OnTriggerStay(Collider collision)
    {
        if(collision.gameObject.tag == "Grab" && obj_curritem == null)
        {
            obj_curritem = collision.gameObject;
            //obj_curritem.GetComponent<Rigidbody>().isKinematic = true;


            vec3_grabloc = obj_curritem.GetComponent<Score>().vec3_grabpoint;

            t_itemhandle.gameObject.GetComponent<FixedJoint>().connectedBody = obj_curritem.GetComponent<Rigidbody>();


            s_player.GetComponent<player_controller_behavior>().CURR_PLAYER_SPEED = s_player.GetComponent<player_controller_behavior>().PLAYER_SPEED - (1 / obj_curritem.GetComponent<Rigidbody>().mass);

            //obj_mouth.GetComponent<ConfigurableJoint>().connectedBody = obj_curritem.GetComponent<Rigidbody>();
            obj_curritem.layer = 8;
            //obj_mouth.GetComponent<ConfigurableJoint>().connectedAnchor = vec3_grabloc;
            //Physics.IgnoreCollision(col_frontcontrol, obj_curritem.GetComponent<Collider>());


            //vec3_grabrot = obj_curritem.GetComponent<Score>().vec3_grabrotation;


            //obj_curritem.GetComponent<Collider>().isTrigger = true;

            //t_grabloc.parent.rotation = t_grabloc.rotation;
            //t_grabloc.parent.position = t_grabloc.transform.position - transform.localPosition;

            //t_mouth.transform.rotation = Quaternion.Euler(vec3_grabrot);
            //

            //obj_curritem.transform.SetParent(t_mouth.transform);
            //obj_curritem.transform.localPosition = vec3_grabloc;
            //obj_curritem.transform.localRotation = Quaternion.Euler(vec3_grabrot);
            //obj_curritem.transform.SetPositionAndRotation(t_mouth.transform.position + vec3_grabloc, t_mouth.transform.localRotation * Quaternion.Euler(vec3_grabrot));

            Debug.Log(vec3_grabloc);
            Debug.Log(vec3_grabrot);
            //obj_curritem.transform.localPosition = vec3_grabloc;
            //obj_curritem.transform.localRotation = Quaternion.Euler(vec3_grabrot);




            S_Notifier.Notify(s_player, Observer.EventType.PickupObject);

        }
    }

	public void Drop()
	{
		b_isgrabbing = false;
	}
}
