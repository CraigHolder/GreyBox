using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    public GameObject s_player;
    public GameObject obj_curritem;
    public Collider col_collider;
    public Transform t_mouth;

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
            obj_curritem.transform.SetPositionAndRotation(t_mouth.transform.position, t_mouth.transform.rotation);
            obj_curritem.transform.SetParent(gameObject.transform);
            obj_curritem.GetComponent<Collider>().isTrigger = true;

            S_Notifier.Notify(gameObject, Observer.EventType.PickupObject);

        }
    }

	public void Drop()
	{
		b_isgrabbing = false;
	}
}
