using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public int i_score = 1;
    public List<player_controller_behavior> connectedplayers;

    public float timer;
    public bool moved;
    Transform lastpos;

    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0 && moved == true)
        {

        }
        if(this.transform != lastpos)
        {
            lastpos = this.transform;
        }
        else
        {
            lastpos = this.transform;
        }
    }


}
