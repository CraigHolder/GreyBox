using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speakers : MonoBehaviour
{
    public Remote s_remote;
    CharacterController c_control;
    BoxCollider col_trigger;
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

        col_trigger = this.GetComponent<BoxCollider>();
        
    }

    void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            c_control = collision.gameObject.GetComponent<CharacterController>();
            c_control.Move(transform.forward * -100 * Time.deltaTime);
			collision.gameObject.GetComponent<player_controller_behavior>().DropItem();
			
            fly_shareddata.S_Notifier.Notify(collision.gameObject, Observer.EventType.Push);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(s_remote.b_speakeron == true && fly_shareddata.e_speakerstate == SpeakerState.Off)
        {
            this.GetComponent<Renderer>().material.color = Color.red;
            //b_on = true;
            fly_shareddata.e_speakerstate = SpeakerState.On;
        }
        else if (s_remote.b_speakeron == false && fly_shareddata.e_speakerstate == SpeakerState.On)
        {
            this.GetComponent<Renderer>().material.color = Color.yellow;
            fly_shareddata.e_speakerstate = SpeakerState.Off;
        }

        switch (fly_shareddata.e_speakerstate)
        {
            case SpeakerState.On:
                col_trigger.enabled = true;
                break;
            case SpeakerState.Off:
                col_trigger.enabled = false;
                break;
        }

    }





}
