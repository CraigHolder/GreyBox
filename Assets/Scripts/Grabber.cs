using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    public GameObject s_player;
    public GameObject obj_curritem;
    
    public Collider col_collider;
    public Transform t_mouth;

    public Vector3 vec3_grabloc;
    public Vector3 vec3_grabrot;
    public Transform t_wasd;

    Subject S_Notifier = new Subject();
    Achievments achievmentobserver = new Achievments();

	private bool b_isgrabbing;
	private bool grab_pressed = false;

	// Start is called before the first frame update
	void Start()
    {
        //s_player = this.GetComponentInParent<PlayerMovement>();

        col_collider = this.GetComponent<BoxCollider>();

        obj_curritem = null;

        S_Notifier.AddObserver(achievmentobserver);
    }

    // Update is called once per frame
    void Update()
    {
		//Grab
		if (Input.GetButton("Grab"))
		{
			if (!grab_pressed)
			{
				b_isgrabbing = !b_isgrabbing;
				grab_pressed = true;
			}
		}
		else
		{
			if (grab_pressed)
				grab_pressed = false;
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
                obj_curritem.GetComponent<Rigidbody>().isKinematic = false;
                obj_curritem.GetComponent<Collider>().isTrigger = false;
                obj_curritem.transform.SetParent(null);
                obj_curritem = null;
            }

        }
    }

    void OnTriggerStay(Collider collision)
    {
        if(collision.gameObject.tag == "Grab" && obj_curritem == null)
        {
            obj_curritem = collision.gameObject;
            obj_curritem.GetComponent<Rigidbody>().isKinematic = true;
            vec3_grabloc = obj_curritem.GetComponent<Score>().vec3_grabpoint;
            vec3_grabrot = obj_curritem.GetComponent<Score>().vec3_grabrotation;
            obj_curritem.GetComponent<Collider>().isTrigger = true;

            //t_grabloc.parent.rotation = t_grabloc.rotation;
            //t_grabloc.parent.position = t_grabloc.transform.position - transform.localPosition;

            //t_mouth.transform.rotation = Quaternion.Euler(vec3_grabrot);
            //
            
            obj_curritem.transform.SetParent(t_mouth.transform);
            obj_curritem.transform.localPosition = vec3_grabloc;
            obj_curritem.transform.localRotation = Quaternion.Euler(vec3_grabrot);
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
