using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardMoat : MonoBehaviour
{
    public player_controller_behavior s_Player;
    private bool b_active;
    //public float f_speedforce = 0.6f;
    //public float f_jumpspeeddef;
    //public float f_jumptimedef;

    void Start()
    {
        s_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<player_controller_behavior>();
        //f_jumpspeeddef = s_Player.f_jumpspeed;
        //f_jumptimedef = s_Player.f_jumptime;
    }

    // Start is called before the first frame update
    void OnTriggerStay(Collider collision)
    {
        if (b_active == false && collision.gameObject.tag == "Player")
        {
            s_Player = collision.gameObject.GetComponent<player_controller_behavior>();
            //s_Player.f_speed *= f_speedforce;
            //s_Player.f_jumpspeed = 0f;
            //s_Player.f_jumptime = 0f;

            b_active = true;
        }
        else if (collision.gameObject.tag == "Player")
        {
			s_Player.DropItem();
        }

        if (collision.gameObject.tag == "Grab")
        {
            Destroy(collision.gameObject);
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
        }
    }
}
