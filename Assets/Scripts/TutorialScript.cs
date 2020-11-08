using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
    public Text t_scoretext;
    public Text t_introtext;
    public TextMesh t_nesttext;
    public TextMesh t_remotetext;
    public TextMesh t_objecttext;
    int i_step = 0;
    public GameObject obj_bounce;
    public GameObject obj_remote;
    public GameObject obj_nest;
    public GameObject obj_grab;
    public GameObject obj_wall1;
    public GameObject obj_wall2;
    public PlayerMovement s_player;
    public bool b_hazardenter = false;
    float f_timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        t_scoretext.text = "Use WASD to move the ferret";
        t_nesttext.gameObject.SetActive(false);
        t_remotetext.gameObject.SetActive(false);
        t_objecttext.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (f_timer < 2f)
        {
            f_timer += Time.deltaTime;
        }
        else
        {
            t_introtext.color = new Color(t_introtext.color.r, t_introtext.color.g, t_introtext.color.b, (t_introtext.color.a - Time.deltaTime)); ;
        }

        switch (i_step)
        {
            case 0:
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
                {
                    t_scoretext.text = "Use Shift to sprint";

                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        t_scoretext.text = "Use Space to jump";
                        i_step++;
                    }
                    //i_step++;
                }
                break;
            case 1:
                if (Input.GetKey(KeyCode.Space))
                {
                    t_scoretext.text = "Bouncy objects can be used to get to higher places";
                    i_step++;
                }
                break;
            case 2:
                if (obj_bounce.GetComponent<Bounce>().s_Player != null)
                {
                    t_objecttext.gameObject.SetActive(true);
                    t_scoretext.text = "Small objects can be picked up and dropped with E";
                    i_step++;
                }
                break;
            case 3:
                if (obj_grab.GetComponent<Rigidbody>().isKinematic == true)
                {
                    t_objecttext.gameObject.SetActive(false);
                    t_scoretext.text = "Stolen objects give your team points when delivered\nto your nest";
                    obj_wall1.GetComponent<BoxCollider>().enabled = false;
                    t_nesttext.gameObject.SetActive(true);
                    i_step++;
                }
                break;
            case 4:
                if (obj_nest.GetComponent<Nest>().i_teamscore > 0)
                {
                    t_nesttext.gameObject.SetActive(false);
                    t_scoretext.text = "Hazards like this rug force you to let go of \nanything you have picked up and/or slow you down";
                    i_step++;
                }
                break;
            case 5:
                if (s_player.f_jumpspeed == 0)
                {
                    b_hazardenter = true;
                }
                else if (s_player.f_jumpspeed != 0 && b_hazardenter == true)
                {
                    t_scoretext.text = "Remotes can be used to turn on various objects";
                    t_remotetext.gameObject.SetActive(true);
                    i_step++;
                }
                break;
            case 6:
                if (obj_remote.GetComponent<Remote>().b_speakeron == true)
                {
                    t_remotetext.gameObject.SetActive(false);
                    t_scoretext.text = "Good luck Bandit";
                    obj_wall2.GetComponent<BoxCollider>().enabled = false;
                    i_step++;
                }
                break;
        }
        

    }
}
