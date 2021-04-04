using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuppetScript : MonoBehaviour
{
	public Transform Root;
	public Transform head = null;
	private Quaternion head_rot;

	[Header("Trail Settings")]
	public Transform[] trail;
	private Quaternion[] start_rots;
	public float max_distance = 0.8f;

	public Animator anims;

	public float orientation = 0.0f;
	Vector3[] origin_pos;


	[Header("Movement Attributes")]
	public float GravityModifier = 3.0f;
	float player_orientation = 0.0f;
	public float ROT_SPEED = 45.0f;
	public float PLAYER_SPEED = 10.0f;
	private Quaternion handle_rot; 
	Vector3 frwd_up_vel = new Vector3();
	int state = 0;

	public enum FerretState
	{
		Idle,
		Walking,
		Jumping,
		Slipping,
		Climbing,
		Ragdoll,
		Headbutt,
		War
	}

	public void Start()
	{
		start_rots = new Quaternion[trail.Length];

		if (head != null)
		{
			head_rot = head.rotation;
		}

		for (int c = 0; c < trail.Length; c++)
		{
			start_rots[c] = trail[c].rotation;
		}

		origin_pos = new Vector3[trail.Length];

		anims = this.GetComponentInParent<Animator>();
	}

	public void DeadReckoning(float joystick_x, float joystick_y)
    {
		//Controls
		float dt = Time.deltaTime;


		float reverse = Input.GetAxis("Reverse");

		float magnitude = new Vector2(joystick_x, joystick_y).magnitude;

		Vector3 movement = new Vector3();

		Vector3 dir;

		ApplyGravity(ref movement, dt);

		if (magnitude > 0.01f)
		{
			float theta = (Mathf.Atan2(joystick_x, joystick_y) * Mathf.Rad2Deg);

			//if (Cam != null)
			//	theta += Cam.GetTheta();

			if (Mathf.Abs(theta - player_orientation) > Mathf.Abs((theta + 360.0f) - player_orientation))
			{
				theta += 360.0f;
			}
			else if (Mathf.Abs(theta - player_orientation) > Mathf.Abs((theta - 360.0f) - player_orientation))
			{
				theta -= 360.0f;
			}

			if (reverse < 1.0f)
			{
				//reversing = false;
				float t_change_dir = 0.0f;
				if (theta != player_orientation)
					t_change_dir = (theta - player_orientation) / Mathf.Abs(theta - player_orientation);

				float t_change_mag = ROT_SPEED * dt;

				if (t_change_mag > Mathf.Abs(theta - player_orientation))
					t_change_mag -= (t_change_mag - Mathf.Abs(theta - player_orientation));

				player_orientation += t_change_dir * t_change_mag;

				if (player_orientation > 360.0f)
				{
					player_orientation -= 360.0f;
				}
				else if (player_orientation < -360.0f)
				{
					player_orientation += 360.0f;
				}

				transform.rotation = Quaternion.Euler(0.0f, player_orientation, 0.0f) * handle_rot;

				dir = Quaternion.Euler(0.0f, player_orientation, 0.0f) * Vector3.forward;

				Vector3 input_motion = (dir * PLAYER_SPEED) * dt;

				//if (on_ground && sprint > 0.0f && stamina > 0.0f)
				//{
				//	input_motion *= SprintModifier;
				//
				//	stamina -= SprintCost * dt;
				//}

				//if (sprint > 0.0f && stamina > 0.0f && stamina > SprintCost && SprintCooldown <= 0.0f)
				//{
				//	Headbutt();
				//}

				movement += input_motion;

				//if (handle != null)
				//{
				//	handle.rotation = transform.rotation;
				//}
				//
				//if (butt != null && back_handle != null)
				//{
				//	butt.enabled = false;
				//	butt.transform.position = back_handle.position;
				//	//butt.enabled = true;
				//}
			}
			

		}
		UpdatePos();
	}

	private void ApplyGravity(ref Vector3 movement, float dt)
	{
		frwd_up_vel += Physics.gravity * dt * GravityModifier;

		if (frwd_up_vel.y < Physics.gravity.y * GravityModifier)
			frwd_up_vel.y = Physics.gravity.y * GravityModifier;

		movement += frwd_up_vel * dt;
	}

	public void UpdatePos()
	{
		//Save positions
		for (int c = 0; c < trail.Length; c++)
		{
			origin_pos[c] = trail[c].position;
		}

		for (int c = 0; c < trail.Length; c++)
		{
			Transform prev = (c > 0) ? trail[c - 1] : null;
			Transform t = trail[c];
			Transform next = (c < trail.Length - 1) ? trail[c + 1] : null;

			Vector3 local_up = Vector3.up;
			Vector3 dir = Vector3.forward;

			if (prev != null && next != null)
			{
				Vector3 n_to_t = t.position - next.position;
				Vector3 t_to_p = prev.position - t.position;

				dir = ((n_to_t + t_to_p) / 2).normalized;

				local_up = (t.up + next.up + prev.up) / 3;

				t.position = origin_pos[c];
			}
			else if (prev != null)
			{
				dir = (prev.position - t.position).normalized;

				local_up = (t.up + prev.up) / 2;

				t.position = origin_pos[c];
			}
			else if (next != null)
			{
				local_up = (t.up + next.up) / 2;

				dir = (t.position - next.position).normalized;
			}

			t.rotation = Quaternion.LookRotation(dir, Vector3.up);


			// Determine the Head's rotation.
			if (c == 0 && head != null)
			{
				//float look_t = Mathf.Lerp(0.0f, -LookTheta, climb_timer / LookTime);

				head.rotation = Quaternion.Euler(t.rotation.eulerAngles.x, orientation, t.rotation.eulerAngles.z) * head_rot;
			}


			// Apply The Starting Rotations of the ferret in order to maintain the desired shape.
			t.rotation *= start_rots[c];
		}
	}

	public void AnimateFerret()
    {
        if (state == (int)FerretState.Idle || state == (int)FerretState.Slipping)
        {
			anims.Play("Idle", -1);
        }
		else if(state == (int)FerretState.Walking || state == (int)FerretState.Headbutt || state == (int)FerretState.Climbing)
        {
			anims.Play("Running", -1);
		}
		else if (state == (int)FerretState.Jumping)
		{
			anims.Play("Falling", -1);
		}

	}

	public int getState()
    {
		return state;
    }

	public void setState(int s)
    {
		state = s;
    }
}
