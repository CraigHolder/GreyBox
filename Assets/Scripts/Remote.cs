using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remote : MonoBehaviour
{
    private bool b_active;
    public bool b_speakeron;

    public FlyWeight fly_shareddata;

    public Subject S_Notifier = new Subject();
    Achievments achievmentobserver = new Achievments();

    public BounceObjCommand c_objbounce;

    public void Awake()
    {
        c_objbounce = new BounceObjCommand();
        S_Notifier.AddObserver(achievmentobserver);
    }


    // Start is called before the first frame update

    void Start()
    {

    }

    void OnTriggerStay(Collider collision)
    {
        if (b_active == false && b_speakeron == false)
        {
            b_speakeron = true;
            b_active = true;
            fly_shareddata.S_Notifier.Notify(collision.gameObject, Observer.EventType.Remote);
        }
        else if (b_active == false && b_speakeron == true)
        {
            b_speakeron = false;
            b_active = true;
        }

    }
    void OnTriggerExit(Collider collision)
    {
        //s_Player = collisionInfo.gameObject.GetComponent<PlayerMovement>();
        b_active = false;
    }
}
