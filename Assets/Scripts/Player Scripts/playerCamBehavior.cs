using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCamBehavior : MonoBehaviour
{
	public GameObject player_control;
	public float CameraSpeed = 45.0f;
	public float CameraDistance = 9.0f;

	private float theta = 0.0f;
	private Vector3 start_dir;

    float f_mouseyprev = 0.0f;

    float vert_theta = 0.0f;
    

    // Start is called before the first frame update
    void Start()
    {
		start_dir = new Vector3(0.0f, 5.0f, -10.0f);
	}

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;

        float horz_mv = Input.GetAxis("Mouse X");
        float vert_mv = Input.GetAxis("Mouse Y");

        theta += CameraSpeed * dt * horz_mv;
        vert_theta += CameraSpeed * dt * vert_mv;

        if (theta > 180.0f)
            theta = -180.0f + (theta - 180);
        else if (theta < -180.0f)
            theta = 180 + (theta + 180);

        vert_theta = Mathf.Clamp(vert_theta, -15.0f, 45.0f);

        Vector3 dir = Quaternion.Euler(0.0f, theta, 0.0f) * start_dir.normalized * CameraDistance;

        transform.position = player_control.transform.position + dir;

        transform.LookAt(player_control.transform.position);

        transform.Rotate(-vert_theta, 0, 0);


        //Checks if absolute angel is between 35 & 0 degrees OR 350(-10) & 360(0) degrees, this is the spot we WANT the camera.
       // if (this.transform.eulerAngles.x <= 35.0f && this.transform.eulerAngles.x >= 0.0f ||
       //     this.transform.eulerAngles.x >= 350.0f && this.transform.eulerAngles.x <= 360.0f)
       // {
       //     this.transform.Rotate(Input.GetAxis("Mouse Y") * -0.5f, 0, 0); //The negative multiplier is just so that the camera moves nicer
       //     if (Input.GetAxis("Mouse Y") != 0.0f)
       //     {
       //         f_mouseyprev = (Input.GetAxis("Mouse Y") * -0.5f); //So long as the mouse had previously moved, save the axis it moved for the corrector.
       //     }
       // }
       // //To prevent the camera from locking, this checks directly after the sweetspot for 10 units
       // //This is done specifically to account for negative angles.
       // else if (this.transform.eulerAngles.x >= 35.0f && this.transform.eulerAngles.x <= 45.0f && f_mouseyprev <= Input.GetAxis("Mouse Y") ||
       //         this.transform.eulerAngles.x <= 350.0f && this.transform.eulerAngles.x >= 340.0f && f_mouseyprev >= Input.GetAxis("Mouse Y"))
       // {
       //     this.transform.Rotate(-f_mouseyprev, 0, 0);
       // }
    
    }

	public float GetTheta()
	{
		return theta;
	}
}
