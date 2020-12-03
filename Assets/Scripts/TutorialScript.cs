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
    public GameObject obj_wall3;
    public GameObject obj_wall4;
    public GameObject obj_deathplane;

    public player_controller_behavior s_player;
    public Transform T_particlespawnpoint;
    ParticlePoolManager particlepoolmanager;

    public ThePluginManager s_plugin;

    float f_lasttime;
    
    public bool b_hazardenter = false;
    float f_timer = 0;

    public FlyWeight fly_shareddata;

    // Start is called before the first frame update
    void Start()
    {
        t_scoretext.text = "Use WASD to move the ferret";
        t_nesttext.gameObject.SetActive(false);
        t_remotetext.gameObject.SetActive(false);
        t_objecttext.gameObject.SetActive(false);
        s_player.b_disableachieve = true;

        f_lasttime = Time.time;

        particlepoolmanager = this.GetComponent<ParticlePoolManager>();
        s_plugin = this.GetComponent<ThePluginManager>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (i_step)
        {
            case 0:
                if (f_timer < 2f)
                {
                    f_timer += Time.deltaTime;
                }
                else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
                {
                    t_scoretext.text = "Use Shift to sprint";
                    t_introtext.color = new Color(t_introtext.color.r, t_introtext.color.g, t_introtext.color.b, (t_introtext.color.a - Time.deltaTime));
                    if (Input.GetKey(KeyCode.LeftShift) && t_introtext.color.a <= 0f)
                    {
                        t_scoretext.text = "Use Space to jump";
                        //obj_wall4.GetComponent<BoxCollider>().enabled = false;
                        Destroy(obj_wall4);
                        GameObject obj_temp = particlepoolmanager.GetParticle();
                        obj_temp.transform.position = T_particlespawnpoint.position;

                        NextStep();
                    }
                    //i_step++;
                }
                break;
            case 1:
                if (Input.GetKey(KeyCode.Space))
                {
                    t_scoretext.text = "Bouncy objects can be used to get to higher places";
                    GameObject obj_temp = particlepoolmanager.GetParticle();
                    obj_temp.transform.position = T_particlespawnpoint.position;

                    NextStep();
                }
                break;
            case 2:
                if (obj_bounce.GetComponent<Bounce>().s_Player != null)
                {
                    t_objecttext.gameObject.SetActive(true);
                    t_scoretext.text = "Small objects can be picked up and dropped with E";
                    GameObject obj_temp = particlepoolmanager.GetParticle();
                    obj_temp.transform.position = T_particlespawnpoint.position;
                    NextStep();
                }
                break;
            case 3:
                if (obj_grab.GetComponent<Rigidbody>().isKinematic == true)
                {
                    //t_objecttext.gameObject.SetActive(false);
                    Destroy(t_objecttext);
                    t_scoretext.text = "Stolen objects give your team points when delivered\nto your nest";
                    //obj_wall1.GetComponent<BoxCollider>().enabled = false;
                    Destroy(obj_wall1);
                    t_nesttext.gameObject.SetActive(true);
                    GameObject obj_temp = particlepoolmanager.GetParticle();
                    obj_temp.transform.position = T_particlespawnpoint.position;
                    NextStep();
                }
                break;
            case 4:
                if (obj_nest.GetComponent<Nest>().i_teamscore > 0)
                {
                    //t_nesttext.gameObject.SetActive(false);
                    Destroy(t_nesttext);
                    //obj_wall3.GetComponent<BoxCollider>().enabled = false;
                    Destroy(obj_wall3);
                    t_scoretext.text = "Hazards like this rug force you to let go of \nanything you have picked up and/or slow you down";
                    GameObject obj_temp = particlepoolmanager.GetParticle();
                    obj_temp.transform.position = T_particlespawnpoint.position;
                    NextStep();
                }
                break;
            case 5:
                if (s_player.PLAYER_JUMP == 0)
                {
                    b_hazardenter = true;
                }
                else if (s_player.PLAYER_JUMP != 0 && b_hazardenter == true)
                {
                    t_scoretext.text = "Remotes can be used to turn on various objects";
                    t_remotetext.gameObject.SetActive(true);
                    GameObject obj_temp = particlepoolmanager.GetParticle();
                    obj_temp.transform.position = T_particlespawnpoint.position;
                    NextStep();
                }
                break;
            case 6:
                if (obj_remote.GetComponent<Remote>().b_speakeron == true)
                {
                    //t_remotetext.gameObject.SetActive(false);
                    Destroy(t_remotetext);
                    t_scoretext.text = "Good luck Bandit";
                    //obj_wall2.GetComponent<BoxCollider>().enabled = false;
                    Destroy(obj_wall2);
                    GameObject obj_temp = particlepoolmanager.GetParticle();
                    obj_temp.transform.position = T_particlespawnpoint.position;
                    NextStep();

                }
                break;
            case 7:
                if(obj_deathplane.GetComponent<DeathPlane>().b_active == true)
                {
                    s_player.b_disableachieve = false;
                    fly_shareddata.S_Notifier.Notify(s_player.gameObject, Observer.EventType.Tutorial);
                    
                }
                break;
        }
    }

    void NextStep()
    {
        float f_currtime = Time.time;
        float f_checkpointtime = f_currtime - f_lasttime;
        f_lasttime = f_currtime;
        s_plugin.SaveTimer(f_checkpointtime);
        i_step++;
    }

}
