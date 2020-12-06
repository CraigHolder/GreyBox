using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class player_controller_behavior : MonoBehaviour
{
	public enum FerretState
	{
		Idle,
		Walking,
		Jumping,
		Slipping,
		Climbing
	}

	FerretState state = FerretState.Idle;

	bool can_climb = false;
	//bool is_climbing = false;
	bool on_ground = false;

	[Header("Config")]
	public bool b_disableachieve = false;
	public float MaxStamina = 100.0f;
	public float StaminaRecovery = 10.0f;
	public float StaminaRecoveryTime = 10.0f;
	public TMP_Text StaminaDisplay = null;
	private float stamina;
	private float srt = 0.0f;

	[Header("Movement Attributes")]
	public float PLAYER_SPEED = 10.0f;
	public float ROT_SPEED = 45.0f;
	public float PLAYER_JUMP = 20.0f;
	public float JumpCost = 25.0f;
	public float GravityModifier = 3.0f;
	public float SprintModifier = 2.5f;
	public float SprintCost = 25.0f;

	float player_orientation = 0.0f;
	Vector3 frwd_up_vel = new Vector3(); // For front-end physics
	Vector3 back_up_vel = new Vector3(); // For back-end physics

	[Header("Handles")]
	public Transform head = null;
	private Quaternion head_rot;
	public Transform handle = null;
	private Quaternion handle_rot;
	private float[] dist_from_last;
	public Grabber grabber = null;

	private Vector3[] origins;

	private CharacterController butt = null;
	private Transform back_handle = null;

	[Header("Trail Settings")]
	public Transform[] trail;
	private Quaternion[] start_rots;
	public float max_distance = 0.8f;

	[Header("Climb Settings")]
	public float LookTheta = 45.0f;
	public float LookTime = 1.0f;
	private float climb_timer = 0.0f;
	public float CLIMB_SPEED = 10.0f;

	[Header("Cam Settings")]
	public playerCamBehavior Cam = null;
	public float CameraSpeed = 90.0f;
	public float CameraDistance = 12.0f;

	public Vector3 vec3_checkpoint;

	// Start is called before the first frame update
	void Start()
    {
		stamina = MaxStamina;

		Cursor.lockState = CursorLockMode.Locked;

		handle_rot = handle.rotation;

		if (head != null)
		{
			head_rot = head.rotation;
		}

		start_rots = new Quaternion[trail.Length];
		dist_from_last = new float[trail.Length];
		origins = new Vector3[trail.Length];

		for (int c = 0; c < trail.Length; c++) {
			start_rots[c] = trail[c].rotation;

			if (c == 0)
				dist_from_last[c] = 0.0f;
			else
				dist_from_last[c] = (trail[c].position - trail[c - 1].position).magnitude;

			origins[c] = trail[c].position;
		}

		if (butt != null)
		{
			back_handle = trail[trail.Length - 1];
			butt.enabled = false;
		}

		if (Cam != null)
		{
			Cam.player_control = this.gameObject;
			Cam.CameraDistance = CameraDistance;
			Cam.CameraSpeed = CameraSpeed;
		}
    }

    // Update is called once per frame
    void Update()
    {
		float stamina_start = stamina;

		if (can_climb && Input.GetButton("Climb"))
		{
			state = FerretState.Climbing;
		}
		else if (state == FerretState.Climbing)
		{
			state = FerretState.Idle;
		}

		float sprint = Input.GetAxis("Sprint");

		bool hit_obj = false;
		bool moved = false;
		bool butt_moved = false;

		for (int c = 0; c < origins.Length; c++)
		{
			origins[c] = trail[c].position;
		}

		float dt = Time.deltaTime;

		float joystick_x = Input.GetAxis("Horizontal");
		float joystick_y = Input.GetAxis("Vertical");

		float magnitude = new Vector2(joystick_x, joystick_y).magnitude;

		Vector3 movement = new Vector3();
		CharacterController cc = GetComponent<CharacterController>();

		if (can_climb)
			climb_timer = (climb_timer + dt > LookTime) ? LookTime : climb_timer + dt;
		else
			climb_timer = (climb_timer - dt < 0.0f) ? 0.0f : climb_timer - dt;

		Vector3 dir;

		switch (state)
		{
			case FerretState.Climbing:
				movement += Vector3.up * PLAYER_SPEED * dt;
				break;

			case FerretState.Slipping:
				dir = Quaternion.Euler(0.0f, player_orientation, 0.0f) * Vector3.forward;
				ApplyGravity(ref movement, dt);
				movement += (dir * PLAYER_SPEED) * dt;

				break;
			default:
				if (on_ground && Input.GetButton("Jump") && stamina > 0.0f && stamina > JumpCost)
				{
					Jump(PLAYER_JUMP);

					stamina -= JumpCost;
				}

				ApplyGravity(ref movement, dt);

				if (magnitude > 0.01f)
				{
					float theta = (Mathf.Atan2(joystick_x, joystick_y) * Mathf.Rad2Deg);

					if (Cam != null)
						theta += Cam.GetTheta();

					if (Mathf.Abs(theta - player_orientation) > Mathf.Abs((theta + 360.0f) - player_orientation))
					{
						theta += 360.0f;
					}
					else if (Mathf.Abs(theta - player_orientation) > Mathf.Abs((theta - 360.0f) - player_orientation))
					{
						theta -= 360.0f;
					}

					float t_change = 0.0f;
					if (theta != player_orientation)
						t_change = (theta - player_orientation) / Mathf.Abs(theta - player_orientation);

					player_orientation += t_change * ROT_SPEED * dt;

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

					if (on_ground && sprint > 0.0f && stamina > 0.0f)
					{
						input_motion *= SprintModifier;

						stamina -= SprintCost * dt;
					}

					movement += input_motion;

					if (handle != null)
					{
						handle.rotation = transform.rotation;
					}
				}
				break;
		}

		if (cc != null)
		{
			Vector3 start_pos = cc.transform.position;

			CollisionFlags hits = cc.Move(movement);

			Vector3 end_pos = cc.transform.position;

			if (hits.HasFlag(CollisionFlags.Sides)){
				//Debug.Log("HOI!!!");
				hit_obj = true;
			}
			if (hits.HasFlag(CollisionFlags.Below))
			{
				//frwd_up_vel = Vector3.zero;
				on_ground = true;
				frwd_up_vel.y = Physics.gravity.y / 2.0f;

				Vector3 b_start = trail[trail.Length - 1].transform.position;
				ApplyTailGrav(dt);
				Vector3 b_end = trail[trail.Length - 1].transform.position;

				if (!Mathf.Approximately((b_end - b_start).magnitude, 0.0f))
					butt_moved = true;
			}
			else
			{
				on_ground = false;
			}

			if (!Mathf.Approximately((end_pos - start_pos).magnitude, 0.0f))
				moved = true;
		}
		else
		{
			transform.position += movement;
		}

		if (handle != null)
		{
			handle.position = transform.position;
		}

		if(!hit_obj && (moved || butt_moved))
			AdjustTail();

		/*
		if (butt != null)
		{
			Vector3 front_to_back = transform.position - butt.transform.position;

			Vector3 back_move = new Vector3();

			if (front_to_back.magnitude > 5.0f)
			{
				butt.transform.rotation = Quaternion.LookRotation(front_to_back.normalized, Vector3.up);

				back_move += front_to_back.normalized * PLAYER_SPEED * dt;
			}

			back_move += Physics.gravity * dt;
			butt.Move(back_move);

			if (back_handle != null)
			{
				back_handle.position = butt.transform.position;
				//back_handle.transform.rotation = butt.transform.rotation;
			}
		}
		*/

		// Stamina
		if (Mathf.Approximately(stamina_start - stamina, 0.0f))
		{
			if (srt > 0.0f)
				srt -= dt;
			else
			{
				stamina += StaminaRecovery * dt;
				if (stamina > MaxStamina)
					stamina = MaxStamina;
			}
		}
		else
		{
			srt = StaminaRecoveryTime;
		}

		if(stamina < 0.0)
        {
			stamina = 0.0f;
        }

		int temp = (int)stamina;
		StaminaDisplay.text = temp.ToString();
	}




	// Functions

	public void PlayerCanClimb(bool c)
	{
		can_climb = c;
	}

	public void Jump(float force)
	{
		frwd_up_vel.y = force;
	}

	public void Slip(bool f)
	{
		if (f)
		{
			state = FerretState.Slipping;
			DropItem();
		}
		else
			state = FerretState.Idle;
	}

	private void ApplyGravity(ref Vector3 movement, float dt) {
		frwd_up_vel += Physics.gravity * dt * GravityModifier;

		if (frwd_up_vel.y < Physics.gravity.y * GravityModifier)
			frwd_up_vel.y = Physics.gravity.y * GravityModifier;

		movement += frwd_up_vel * dt;
	}

	private void ApplyTailGrav(float dt)
	{
		if (butt == null)
			return;
		butt.transform.position = back_handle.position;
		butt.enabled = true;

		back_up_vel += Physics.gravity * dt;

		CollisionFlags cf = butt.Move(back_up_vel * dt);

		if (cf.HasFlag(CollisionFlags.Below))
			back_up_vel = Vector3.zero;

		back_handle.transform.position = butt.transform.position;

		butt.enabled = false;
	}

	private void AdjustTail()
	{
		Vector3[] new_pos = new Vector3[trail.Length];
		new_pos[0] = trail[0].position;

		// first pass
		for (int c = 1; c < trail.Length; c++)
		{

			Transform prev = trail[c - 1];
			Transform t = trail[c];

			Vector3 prev_forward = Quaternion.Inverse(start_rots[c - 1]) * prev.forward;
			Vector3 t_forward = Quaternion.Inverse(start_rots[c]) * t.forward;

			//Vector3 prev_forward = prev.forward;
			//Vector3 t_forward = t.forward;

			Vector3 prev_back = (prev.position - (prev_forward * max_distance / 2));

			//Vector3 dist =  prev_back - (t.position + t_forward * max_distance / 2);
			Vector3 dist = prev.position - origins[c];

			if (!Mathf.Approximately(dist.magnitude, max_distance))
			{
				t.position = origins[c];

				Vector3 t_to_prev_back = prev_back - t.position;

				Quaternion rot = Quaternion.LookRotation(dist.normalized, Vector3.up);

				t.rotation = rot;

				t.position += t.forward * (dist.magnitude - max_distance);

				t.rotation *= start_rots[c];

			}

			new_pos[c] = t.position;
		}

		//second pass

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

				t.position = new_pos[c];
			}
			else if (prev != null)
			{
				dir = (prev.position - t.position).normalized;

				local_up = (t.up + prev.up) / 2;

				t.position = new_pos[c];
			}
			else if (next != null)
			{
				local_up = (t.up + next.up) / 2;

				dir = (t.position - next.position).normalized;
			}

			t.rotation = Quaternion.LookRotation(dir, Vector3.up);
			//t.rotation = Quaternion.LookRotation(dir, head.up);

			//t.localRotation = Quaternion.Euler(t.localRotation.eulerAngles.x, 0.0f, t.localRotation.eulerAngles.z);

			//if (t.localRotation.eulerAngles.y > 5.0f || t.localRotation.eulerAngles.y < -5.0f)
			//{
			//	Debug.Log("HOI!!!!!!!");
			//}

			if (c == 0 && head != null)
			{
				float look_t = Mathf.Lerp(0.0f, -LookTheta, climb_timer / LookTime);

				if(can_climb)
					head.rotation = Quaternion.Euler(look_t, player_orientation, t.rotation.eulerAngles.z) * head_rot;
				else
					head.rotation = Quaternion.Euler(t.rotation.eulerAngles.x, player_orientation, t.rotation.eulerAngles.z) * head_rot;
			}

			t.rotation *= start_rots[c];

		}
		
	}

	public void DropItem()
	{
		if(grabber != null)
			grabber.Drop();
	}
}
