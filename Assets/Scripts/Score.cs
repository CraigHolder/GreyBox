using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public int i_score = 1;
    public List<player_controller_behavior> connectedplayers;

    public bool moved = false;
    public bool networkedmoved = false;
    public bool grabbed = false;

    public int state;

    Vector3 lastpos;
    Vector3 lastrot;

    //float timer = 0.25f;

    void Start()
    {
        lastpos = transform.position;
        lastrot = transform.rotation.eulerAngles;
    }

    void Update()
    {
        if(((Vector3.Distance(transform.position, lastpos)  >= 0.1 && transform.rotation.eulerAngles != lastrot) && !networkedmoved))
        {
            moved = true;
            state = 1;
            Debug.Log(this.name + " Moved");
            lastpos = transform.position;
            lastrot = transform.rotation.eulerAngles;
            //timer = 0.25f;
        }
        else
        {
            moved = false;
            state = 0;
        }
        if(grabbed)
        {
            state = 2;
        }


        if (networkedmoved)
        {
            Debug.Log(this.name + " NetworkedMoved");
            lastpos = transform.position;
            lastrot = transform.rotation.eulerAngles;
            networkedmoved = false;
            //timer = 0.25f;
        }

        



        //if (grabbed && (transform.position == lastpos || transform.rotation.eulerAngles == lastrot))
        //{
        //    grabbed = false;
        //}

    }
}
