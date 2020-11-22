using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speakers : MonoBehaviour
{
    public Remote s_remote;
    CharacterController c_control;
    BoxCollider col_trigger;
    SpeakerState e_state = SpeakerState.Off;

    Subject S_Notifier = new Subject();
    Achievments achievmentobserver = new Achievments();

    enum SpeakerState
    {
        On,Off
    }

    void Start()
    {
        col_trigger = this.GetComponent<BoxCollider>();
        S_Notifier.AddObserver(achievmentobserver);
    }

    void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            c_control = collision.gameObject.GetComponent<CharacterController>();
            c_control.Move(transform.forward * -100 * Time.deltaTime);
            S_Notifier.Notify(collision.gameObject, Observer.EventType.Push);
			collision.gameObject.GetComponent<player_controller_behavior>().DropItem();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(s_remote.b_speakeron == true && e_state == SpeakerState.Off)
        {
            this.GetComponent<Renderer>().material.color = Color.red;
            //b_on = true;
            e_state = SpeakerState.On;
        }
        else if (s_remote.b_speakeron == false && e_state == SpeakerState.On)
        {
            this.GetComponent<Renderer>().material.color = Color.yellow;
            e_state = SpeakerState.Off;
        }

        switch (e_state)
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
