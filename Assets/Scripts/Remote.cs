using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remote : MonoBehaviour
{
    public bool b_active;
    public bool b_speakeron;
    public MeshRenderer mr_light;
    public Material M_on;
    public Material M_off;

    public FlyWeight fly_shareddata;

    public Subject S_Notifier = new Subject();
    Achievments achievmentobserver = new Achievments();

    //public BounceObjCommand c_objbounce;

    public void Awake()
    {
        //c_objbounce = new BounceObjCommand();
        S_Notifier.AddObserver(achievmentobserver);
    }


    // Start is called before the first frame update

    void Start()
    {
        mr_light.material = M_off;
    }

    void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "PlayerController")
        {
            if (b_active == false && b_speakeron == false)
            {
                b_speakeron = true;
                b_active = true;
                mr_light.material = M_on;
                fly_shareddata.e_speakerstate = Speakers.SpeakerState.On;
                fly_shareddata.S_Notifier.Notify(collision.gameObject, Observer.EventType.Remote);
            }
            else if (b_active == false && b_speakeron == true)
            {
                b_speakeron = false;
                mr_light.material = M_off;
                fly_shareddata.e_speakerstate = Speakers.SpeakerState.Off;
                b_active = true;
            }

        }
    }
    void OnTriggerExit(Collider collision)
    {
        //s_Player = collisionInfo.gameObject.GetComponent<PlayerMovement>();
        b_active = false;
    }
}
