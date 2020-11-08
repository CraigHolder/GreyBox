using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TEMPORARY UI STUFF
using TMPro;

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

    //TEMPORARY UI STUFF
    public GameObject text_obj;
    private TextMeshProUGUI textpro;

    private float f_sprintmult = 1.0f;
    private bool b_sprinting = false;
    private bool b_cansprint = true;
    private bool b_recoverydelay = false;
    private bool b_recovering = false;

    public const float MAX_STAMINA = 100.0f;
    private float f_stamina = MAX_STAMINA;
    private float f_staminadecay = 50;
    private float f_staminarecov = 12f;

    private float f_staminadelay = 3.0f; //Recovery Delay

    // Start is called before the first frame update
    void Start()
    {
        f_jumptimer = f_jumptime;
        vec3_checkpoint = new Vector3(48, 1, -48);
        Cursor.lockState = CursorLockMode.Locked;
        f_mouseyprev = Input.GetAxis("Mouse Y");

        //TEMPORARY UI STUFF
        textpro = text_obj.GetComponent<TMPro.TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        //print(f_stamina);

        //IF YOU HAVE STAMINA YOU CAN SPRINT
        if(f_stamina > 0)
        {
            b_cansprint = true;
        }
        else
        {
            b_cansprint = false;
        }

        //Delay stamina Recovery on finishing sprint.
        if (f_stamina < MAX_STAMINA && !b_sprinting && !b_recoverydelay && !b_recovering)
        {
            b_recoverydelay = true;
        }
      
       //STAMINA DECAY
        if (b_sprinting)
        {
            //Stamina Decay
            f_stamina -= f_staminadecay * Time.deltaTime /** 10*/;

            if (f_stamina <= 0.0f)
            {
                f_stamina = 0.0f;
                b_cansprint = false;
            }
        }
       
        // Recovery Delay TIMER
        if(b_recoverydelay && f_staminadelay > 0)
        {
             f_staminadelay -= Time.deltaTime;
        }
        // BEGIN RECOVERING
        if(f_staminadelay <= 0)
        {
             
             f_staminadelay = 3.0f;
             b_recoverydelay = false;
             b_recovering = true;
        }

        if (b_recovering)
        {
            f_stamina += f_staminarecov * Time.deltaTime * 5;

            if (f_stamina >= MAX_STAMINA)
            {
                f_stamina = MAX_STAMINA;
            }

        }


        //SPRINT
        if (Input.GetKey(KeyCode.LeftShift) && b_cansprint && f_stamina > 0)
        {
            f_sprintmult = 2.5f;
            b_sprinting = true;
            b_recovering = false;
        }
        else
        {
            f_sprintmult = 1.0f;
            b_sprinting = false;
        }

        //TEMPORARY UI STUFF
        textpro.text = f_stamina.ToString("#");

        //Movement
        if (Input.GetKey(KeyCode.W))
        {
            c_control.Move(transform.forward * f_speed * Time.deltaTime * f_sprintmult);
            //c_control.Move(new Vector3(0, 0, i_speed * Time.deltaTime));
        }
        else if (Input.GetKey(KeyCode.S))
        {
            c_control.Move(transform.forward * -f_speed * Time.deltaTime * f_sprintmult);
        }

        if (Input.GetKey(KeyCode.D))
        {
            c_control.Move(transform.right * f_speed * Time.deltaTime * f_sprintmult);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            c_control.Move(transform.right * -f_speed * Time.deltaTime * f_sprintmult);
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
        if (Input.GetKeyDown(KeyCode.E) && b_isgrabbing == false)
        {
            b_isgrabbing = true;
        }
        else if (Input.GetKeyDown(KeyCode.E) && b_isgrabbing == true)
        {
            b_isgrabbing = false;
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

    public float getStamina()
    {
        return f_stamina;
    }
}
