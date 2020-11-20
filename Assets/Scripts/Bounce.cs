﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    public PlayerMovement s_Player;
    private bool b_active;
    AudioSource a_audiosource;
    public float f_bounceforce = 2f;
    BounceObjCommand c_objbounce;
    Subject S_Notifier = new Subject();
    Achievments achievmentobserver = new Achievments();


    void Start()
    {
        a_audiosource = this.GetComponent<AudioSource>();
        c_objbounce = new BounceObjCommand();

        S_Notifier.AddObserver(achievmentobserver);
    }

    // Start is called before the first frame update
    void OnTriggerStay(Collider collision)
    {
        if (b_active == false && collision.gameObject.tag == "Player")
        {
            S_Notifier.Notify(collision.gameObject, Observer.EventType.Bounce);

            s_Player = collision.gameObject.GetComponent<PlayerMovement>();
            s_Player.f_jumpspeed *= f_bounceforce;
            s_Player.f_jumptime *= f_bounceforce;

            s_Player.f_jumptimer = s_Player.f_jumptime;
            //c_control.Move(new Vector3(0, i_jumpspeed * Time.deltaTime, 0));
            s_Player.e_currstate = PlayerMovement.FerretState.Jumping;
            a_audiosource.Play();
            b_active = true;
        }
        else if (collision.gameObject.tag == "Grab" && collision.gameObject.GetComponent<Rigidbody>().isKinematic == false)
        {
            c_objbounce.Execute(c_objbounce, collision.gameObject);

            a_audiosource.Play();
        }

    }
    void OnTriggerExit(Collider collision)
    {
        //s_Player = collisionInfo.gameObject.GetComponent<PlayerMovement>();
        if(collision.gameObject.tag == "Player")
        {

            b_active = false;
            s_Player.f_jumpspeed /= f_bounceforce;
            s_Player.f_jumptime /= f_bounceforce;
        }
    }
}
