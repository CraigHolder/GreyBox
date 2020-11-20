using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rug : MonoBehaviour
{
    public PlayerMovement s_Player;
    private bool b_active;
    //public AudioSource a_audiosource;
    public float f_speedforce = 0.6f;
    public float f_jumpspeeddef;
    public float f_jumptimedef;

    void Start()
    {
        s_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        f_jumpspeeddef = s_Player.f_jumpspeed;
        f_jumptimedef = s_Player.f_jumptime;
    }

    // Start is called before the first frame update
    void OnTriggerStay(Collider collision)
    {
        if (b_active == false && collision.gameObject.tag == "Player")
        {
            s_Player = collision.gameObject.GetComponent<PlayerMovement>();
            s_Player.f_speed *= f_speedforce;
            s_Player.f_jumpspeed = 0f;
            s_Player.f_jumptime = 0f;
            //s_Player.b_isgrabbing = false;

            //s_Player.f_jumptimer = s_Player.f_jumptime;
            //c_control.Move(new Vector3(0, i_jumpspeed * Time.deltaTime, 0));
            //s_Player.e_currstate = PlayerMovement.FerretState.Jumping;
            //a_audiosource.Play();
            b_active = true;
        }
        else if (collision.gameObject.tag == "Player")
        {
            s_Player.b_isgrabbing = false;
        }
    }
    void OnTriggerExit(Collider collision)
    {
        //s_Player = collisionInfo.gameObject.GetComponent<PlayerMovement>();
        if (collision.gameObject.tag == "Player")
        {

            b_active = false;
            s_Player.f_speed *= (1f / f_speedforce);
            s_Player.f_jumpspeed = f_jumpspeeddef;
            s_Player.f_jumptime = f_jumptimedef;
        }
    }
}
