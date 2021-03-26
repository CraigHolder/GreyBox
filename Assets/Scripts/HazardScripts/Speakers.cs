using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speakers : MonoBehaviour
{
    public Remote s_remote;
    CharacterController c_control;
    BoxCollider col_trigger;
    Vector3 vec3_defaultscale;
    public ParticleSystem[] effects;

    //SpeakerState e_state = SpeakerState.Off;
    //
    //Subject S_Notifier = new Subject();
    //Achievments achievmentobserver = new Achievments();

    public FlyWeight fly_shareddata;


    public enum SpeakerState
    {
        On,Off
    }

    void Start()
    {
        vec3_defaultscale = this.transform.localScale;
        col_trigger = this.GetComponent<BoxCollider>();
        effects[0].gameObject.SetActive(false);
        effects[1].gameObject.SetActive(false);
    }

    void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "PlayerController")
        {
            c_control = collision.gameObject.GetComponent<CharacterController>();
            c_control.Move(transform.forward * -100 * Time.deltaTime);
            if(collision.gameObject.GetComponent<player_controller_behavior>() != null)
            {

                collision.gameObject.GetComponent<player_controller_behavior>().DropItem();
            }
			
            fly_shareddata.S_Notifier.Notify(collision.gameObject, Observer.EventType.Push);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if(on == false && fly_shareddata.e_speakerstate == SpeakerState.On)
        //{
        //    this.GetComponent<Renderer>().material.color = Color.red;
        //    //b_on = true;
        //    effects[0].gameObject.SetActive(true);
        //    effects[1].gameObject.SetActive(true);
        //    on = true;
        //    //fly_shareddata.e_speakerstate = SpeakerState.On;
        //}
        //else if (on == true && fly_shareddata.e_speakerstate == SpeakerState.Off)
        //{
        //    this.GetComponent<Renderer>().material.color = Color.yellow;
        //    effects[1].gameObject.SetActive(false);
        //    effects[0].gameObject.SetActive(false);
        //    on = false;
        //    //fly_shareddata.e_speakerstate = SpeakerState.Off;
        //}

        switch (fly_shareddata.e_speakerstate)
        {
            case SpeakerState.On:
                col_trigger.enabled = true;
                effects[0].gameObject.SetActive(true);
                effects[1].gameObject.SetActive(true);
                this.transform.localScale = vec3_defaultscale + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
                break;
            case SpeakerState.Off:
                effects[1].gameObject.SetActive(false);
                effects[0].gameObject.SetActive(false);
                col_trigger.enabled = false;
                break;
        }

    }





}
