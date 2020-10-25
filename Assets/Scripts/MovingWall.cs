using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWall : MonoBehaviour
{
    public bool b_leftright = false;
    public float f_speed;
    public float f_dist;
    float f_currdist;
    CharacterController c_control;

    // Start is called before the first frame update
    void Start()
    {
        c_control = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(b_leftright == true)
        {
            c_control.Move(new Vector3(f_speed * Time.deltaTime, 0, 0));
            f_currdist += Mathf.Abs(f_speed * Time.deltaTime);
            if (f_currdist >= f_dist)
            {
                f_currdist = 0;
                f_speed = f_speed * -1.0f;
            }
        }
        else
        {
            c_control.Move(new Vector3(0, 0, f_speed * Time.deltaTime));
            f_currdist += Mathf.Abs(f_speed * Time.deltaTime);
            if (f_currdist >= f_dist)
            {
                f_currdist = 0;
                f_speed = f_speed * -1.0f;
            }
        }
    }
}
