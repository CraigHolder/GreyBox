using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject obj_player;
    public CharacterController c_control;
    public Vector3 vec3_checkpoint;
    public int i_speed;
    public int i_jumpspeed;
    public float f_jumptime;
    public float f_gravity;
    public FerretState e_currstate;

    public float f_jumptimer;

    public enum FerretState
    {
        Idle,
        Jumping,
    }


    // Start is called before the first frame update
    void Start()
    {
        f_jumptimer = f_jumptime;
        vec3_checkpoint = new Vector3(48, 1, -48);
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            c_control.Move(transform.forward * i_speed * Time.deltaTime);
            //c_control.Move(new Vector3(0, 0, i_speed * Time.deltaTime));
        }
        else if (Input.GetKey(KeyCode.S))
        {
            c_control.Move(transform.forward * -i_speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            c_control.Move(transform.right * i_speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            c_control.Move(transform.right * -i_speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.Space) && c_control.isGrounded == true)
        {
            f_jumptimer = f_jumptime;
            //c_control.Move(new Vector3(0, i_jumpspeed * Time.deltaTime, 0));
            e_currstate = FerretState.Jumping;
        }
        else if (e_currstate == FerretState.Jumping)
        {
            f_jumptimer -= Time.deltaTime;
            if (f_jumptimer <= 0)
            {
                e_currstate = FerretState.Idle;
            }
        }

        transform.Rotate(0, Input.GetAxis("Mouse X"), 0);

        

        switch (e_currstate)
        {
            case FerretState.Idle:
                c_control.Move(new Vector3(0, -f_gravity * Time.deltaTime, 0));
                break;
            case FerretState.Jumping:
                c_control.Move(new Vector3(0, i_jumpspeed * Time.deltaTime, 0));
                break;
        }
    }
}
