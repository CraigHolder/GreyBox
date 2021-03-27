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
    Vector3 lastpos;
    Vector3 lastrot;

    void Start()
    {
        lastpos = transform.position;
        lastrot = transform.rotation.eulerAngles;
    }

    void Update()
    {
        if((transform.position != lastpos || transform.rotation.eulerAngles != lastrot) && !networkedmoved)
        {
            moved = true;
            Debug.Log(this.name + " Moved");
            lastpos = transform.position;
            lastrot = transform.rotation.eulerAngles;
        }
        else if (networkedmoved)
        {
            Debug.Log(this.name + " NetworkedMoved");
            lastpos = transform.position;
            lastrot = transform.rotation.eulerAngles;
            networkedmoved = false;
        }
    }
}
