using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    public PlayerMovement s_Player;
    private bool b_active;
    AudioSource a_audiosource;
    public float f_bounceforce = 2f;
    BounceObjCommand c_objbounce;

    public TutorialQuestManager t_manager;
    public Plugin_Manager p_handler;

    Subject S_Notifier = new Subject();
    Achievments achievmentobserver = new Achievments();



    void Start()
    {
        a_audiosource = this.GetComponent<AudioSource>();
        c_objbounce = new BounceObjCommand();

        S_Notifier.AddObserver(achievmentobserver);
    }

    // Start is called before the first frame update
    public void OnTriggerStay(Collider collision)
    {
        if (b_active == false && collision.gameObject.tag == "Player")
        {
            S_Notifier.Notify(collision.gameObject, Observer.EventType.Bounce);

            if (t_manager) {
                S_Notifier.TutorialNotify(collision.gameObject, Observer.EventType.Tut_Bounce, t_manager);
            }

            if (p_handler)
            {
                p_handler.recordBounce();
            }


			//s_Player.f_jumptimer = s_Player.f_jumptime;
			//c_control.Move(new Vector3(0, i_jumpspeed * Time.deltaTime, 0));
			//s_Player.e_currstate = PlayerMovement.FerretState.Jumping;

			player_controller_behavior player = collision.GetComponent<player_controller_behavior>();

			player.Jump(f_bounceforce);

            a_audiosource.Play();
            b_active = true;
        }
        else if (collision.gameObject.tag == "Grab" && collision.gameObject.GetComponent<Rigidbody>().isKinematic == false)
        {
            c_objbounce.Execute(c_objbounce, collision.gameObject);

            a_audiosource.Play();
        }

    }
    public void OnTriggerExit(Collider collision)
    {
        //s_Player = collisionInfo.gameObject.GetComponent<PlayerMovement>();
        if(collision.gameObject.tag == "Player")
        {

            b_active = false;
           // s_Player.f_jumpspeed /= f_bounceforce;
            //s_Player.f_jumptime /= f_bounceforce;
        }
    }
}
