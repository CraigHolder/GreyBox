using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardSpill : MonoBehaviour
{
    public player_controller_behavior s_Player;
    private bool b_active;
    //public float f_speedforce = 0.6f;
    public float f_jumpspeeddef;
    public float f_jumptimedef;
    private float f_spillSpeed = 0.8f;
    public bool b_flatSurface = true;

    void Start()
    {
        //s_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        //f_jumpspeeddef = s_Player.f_jumpspeed;
        //f_jumptimedef = s_Player.f_jumptime;
    }

    public bool getSurface()
    {
        return b_flatSurface;
    }

    public float getSpeed()
    {
        return f_spillSpeed;
    }

    // Start is called before the first frame update
    void OnTriggerEnter(Collider collision)
    {
        if (b_active == false && collision.gameObject.tag == "Player")
        {
            s_Player = collision.gameObject.GetComponent<player_controller_behavior>();
			//s_Player.f_speed *= f_speedforce;
			//s_Player.f_jumpspeed = 0f;
			//s_Player.f_jumptime = 0f;
			//s_Player.e_currstate = PlayerMovement.FerretState.Slipping;
			//s_Player.h_curSpill = this;

			s_Player.Slip(true);

            b_active = true;
        }
        else if (collision.gameObject.tag == "Player")
        {
            //s_Player.b_isgrabbing = false;
			s_Player = collision.gameObject.GetComponent<player_controller_behavior>();
			s_Player.DropItem();
		}
    }
    void OnTriggerExit(Collider collision)
    {
        //s_Player = collisionInfo.gameObject.GetComponent<PlayerMovement>();
        if (collision.gameObject.tag == "Player")
        {

            b_active = false;
			// s_Player.f_speed *= (1f / f_speedforce);
			//s_Player.f_jumpspeed = f_jumpspeeddef;
			//s_Player.f_jumptime = f_jumptimedef;
			//s_Player.e_currstate = PlayerMovement.FerretState.Idle;

			s_Player.Slip(false);
		}
    }
}
