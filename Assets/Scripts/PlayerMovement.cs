using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject obj_player;
    public CharacterController c_control;
    public Vector3 vec3_checkpoint;
    public float f_speed;
    public float f_jumpspeed;
    public float f_jumptime;
    public float f_gravity;
    public FerretState e_currstate;
    public bool b_isgrabbing = false;
    public Grabber s_grab;
    float f_mouseyprev;

    public float f_jumptimer;

    public GameObject obj_cam;

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
        f_mouseyprev = Input.GetAxis("Mouse Y");
    }

    // Update is called once per frame
    void Update()
    {
        //Movement
        if (Input.GetKey(KeyCode.W))
        {
            c_control.Move(transform.forward * f_speed * Time.deltaTime);
            //c_control.Move(new Vector3(0, 0, i_speed * Time.deltaTime));
        }
        else if (Input.GetKey(KeyCode.S))
        {
            c_control.Move(transform.forward * -f_speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            c_control.Move(transform.right * f_speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            c_control.Move(transform.right * -f_speed * Time.deltaTime);
        }

        //Jumping
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


        //Mouse Player movement
        transform.Rotate(0, Input.GetAxis("Mouse X") * 1.1f, 0);


        //Checks if absolute angel is between 35 & 0 degrees OR 350(-10) & 360(0) degrees, this is the spot we WANT the camera.
        if (obj_cam.transform.eulerAngles.x <= 35.0f && obj_cam.transform.eulerAngles.x >= 0.0f ||
            obj_cam.transform.eulerAngles.x >= 350.0f && obj_cam.transform.eulerAngles.x <= 360.0f)
        {
            obj_cam.transform.Rotate(Input.GetAxis("Mouse Y") * -0.5f, 0, 0); //The negative multiplier is just so that the camera moves nicer
            if (Input.GetAxis("Mouse Y") != 0.0f)
            {
                f_mouseyprev = (Input.GetAxis("Mouse Y") * -0.5f); //So long as the mouse had previously moved, save the axis it moved for the corrector.
            }
        }
        //To prevent the camera from locking, this checks directly after the sweetspot for 10 units
        //This is done specifically to account for negative angles.
        else if(obj_cam.transform.eulerAngles.x >= 35.0f && obj_cam.transform.eulerAngles.x <= 45.0f && f_mouseyprev <= Input.GetAxis("Mouse Y") ||
                obj_cam.transform.eulerAngles.x <= 350.0f && obj_cam.transform.eulerAngles.x >= 340.0f && f_mouseyprev >= Input.GetAxis("Mouse Y"))
        {
           obj_cam.transform.Rotate(Input.GetAxis("Mouse Y") * -0.5f, 0, 0);
        }


        //Grab
        if (Input.GetKey(KeyCode.E))
        {
            //b_isgrabbing = true;
        }

        switch (e_currstate)
        {
            case FerretState.Idle:
                c_control.Move(new Vector3(0, -f_gravity * Time.deltaTime, 0));
                break;
            case FerretState.Jumping:
                c_control.Move(new Vector3(0, f_jumpspeed * Time.deltaTime, 0));
                break;
        }
    }
}
