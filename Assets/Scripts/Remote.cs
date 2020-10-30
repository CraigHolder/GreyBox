using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remote : MonoBehaviour
{
    private bool b_active;
    public bool b_speakeron;

    // Start is called before the first frame update
    

    void OnTriggerStay(Collider collision)
    {
        if (b_active == false && b_speakeron == false)
        {
            b_speakeron = true;
            b_active = true;
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
