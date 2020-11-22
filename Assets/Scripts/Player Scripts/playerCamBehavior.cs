using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCamBehavior : MonoBehaviour
{
	public GameObject player_control;
	public float CameraSpeed = 45.0f;
	public float CameraDistance = 12.0f;

	private float theta = 0.0f;
	private Vector3 start_dir;

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

		theta += CameraSpeed * dt * horz_mv;

		if (theta > 180.0f)
			theta = -180.0f + (theta - 180);
		else if (theta < -180.0f)
			theta = 180 + (theta + 180);

		Vector3 dir = Quaternion.Euler(0.0f, theta, 0.0f) * start_dir.normalized * CameraDistance;

		transform.position = player_control.transform.position + dir;

		transform.LookAt(player_control.transform.position);
    }

	public float GetTheta()
	{
		return theta;
	}
}
