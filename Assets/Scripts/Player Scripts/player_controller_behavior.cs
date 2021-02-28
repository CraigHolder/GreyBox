using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;

public class player_controller_behavior : MonoBehaviour
{
	public enum FerretState
	{
		Idle,
		Walking,
		Jumping,
		Slipping,
		Climbing,
		Ragdoll,
		Headbutt
	}

	FerretState state = FerretState.Idle;

	bool can_climb = false;
	//bool is_climbing = false;
	bool on_ground = false;

	bool reversing = false;

	public int i_playerID;

	[Header("Config")]
	public bool b_disableachieve = false;
	public float MaxStamina = 100.0f;
	public float StaminaRecovery = 10.0f;
	public float StaminaRecoveryTime = 10.0f;
	public TMP_Text StaminaDisplay = null;
	private float stamina;
	private float srt = 0.0f;
	public float stambartime = 2f;
	float stamtimer;

	[Header("Movement Attributes")]
	public float PLAYER_SPEED = 10.0f;
	public float CURR_PLAYER_SPEED = 10.0f;
	public float ROT_SPEED = 45.0f;
	public float PLAYER_JUMP = 20.0f;
	public float JumpCost = 25.0f;
	public float GravityModifier = 3.0f;
	public float SprintModifier = 1200.5f;
	public float SprintCost = 25.0f;
	public float SprintCooldown = 0.0f;
	public float SprintDuration = 0.0f;

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
	public Image staminabar;
	public GameObject fullstaminabar;

	private Vector3[] origins;

	public CharacterController butt = null;
	public Transform back_handle = null;

	[Header("Trail Settings")]
	public Transform[] trail;
	private Quaternion[] start_rots;
	private Vector3[] start_dir;
	public float max_distance = 0.8f;

	[Header("Ragdoll Settings")]
	public Transform SkeletonRoot;
	public Transform Ragdoll;

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

	[Header("Debug Settings")]
	public Transform Reversing_Marker;

	// Start is called before the first frame update
	void Start()
    {
		stamina = MaxStamina;
		stamtimer = stambartime;
		Cursor.lockState = CursorLockMode.Locked;

		handle_rot = handle.rotation;

		if (head != null)
		{
			head_rot = head.rotation;
		}

		start_rots = new Quaternion[trail.Length];
		dist_from_last = new float[trail.Length];
		origins = new Vector3[trail.Length];
		start_dir = new Vector3[trail.Length];

		for (int c = 0; c < trail.Length; c++) {
			start_rots[c] = trail[c].rotation;

			if (c == 0)
			{
				dist_from_last[c] = 0.0f;

				start_dir[c] = (trail[c].position - trail[c+1].position).normalized;
			}
			else
			{
				Vector3 t_to_p = trail[c - 1].position - trail[c].position;

				if (c == trail.Length - 1)
				{
					start_dir[c] = t_to_p.normalized;
				}
				else
				{
					Vector3 n_to_t = trail[c].position - trail[c+1].position;

					start_dir[c] = ((t_to_p + n_to_t) / 2).normalized;
				}

				dist_from_last[c] = (trail[c].position - trail[c - 1].position).magnitude;
			}

			origins[c] = trail[c].position;
		}

		if (butt != null)
		{
			//back_handle = trail[trail.Length - 1];
			butt.enabled = false;

			Physics.IgnoreCollision(this.GetComponent<CharacterController>(), butt, true);
		}

		if (Cam != null)
		{
			Cam.player_control = this.gameObject;
			Cam.CameraDistance = CameraDistance;
			Cam.CameraSpeed = CameraSpeed;
		}

		//RAGDOLL TEST
		//RagdollEnabled(false);
		//SetupRagdoll();
    }

    // Update is called once per frame
    void Update()
    {
		float stamina_start = stamina;

		switch (i_playerID)
		{
			case 1:
				if (can_climb && Input.GetButton("Climb"))
				{
					state = FerretState.Climbing;
				}
				else if (state == FerretState.Climbing)
				{
					state = FerretState.Idle;
				}
				break;
			case 2:
				if (can_climb && Input.GetButton("Climb2"))
				{
					state = FerretState.Climbing;
				}
				else if (state == FerretState.Climbing)
				{
					state = FerretState.Idle;
				}
				break;
		}
		float sprint = 0;
		switch (i_playerID)
		{
			case 1:
				sprint = Input.GetAxis("Sprint");
				break;
			case 2:
				sprint = Input.GetAxis("Sprint2");
				break;
		}

		for (int c = 0; c < origins.Length; c++)
		{
			origins[c] = trail[c].position;
		}

		//sprint = Input.GetAxis("Sprint");

		bool hit_obj = false;
		bool moved = false;
		bool butt_moved = false;

		float dt = Time.deltaTime;

		//Controls
		float joystick_x = 0;
		float joystick_y = 0;
		switch (i_playerID)
        {
			case 1:
				joystick_x = Input.GetAxis("Horizontal");
				joystick_y = Input.GetAxis("Vertical");
				break;
			case 2:
				joystick_x = Input.GetAxis("Horizontal2");
				joystick_y = Input.GetAxis("Vertical2");
				break;
		}
		

		float reverse = Input.GetAxis("Reverse");

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
				movement += Vector3.up * CURR_PLAYER_SPEED * dt;
				break;

			case FerretState.Slipping:
				dir = Quaternion.Euler(0.0f, player_orientation, 0.0f) * Vector3.forward;
				ApplyGravity(ref movement, dt);
				movement += (dir * CURR_PLAYER_SPEED) * dt;
				break;

			case FerretState.Headbutt:
				dir = Quaternion.Euler(0.0f, player_orientation, 0.0f) * Vector3.forward;
				ApplyGravity(ref movement, dt);
				movement += (dir * PLAYER_SPEED * SprintModifier) * dt;
				break;
			case FerretState.Ragdoll:
				ApplyRagdoll();
				break;
			default:

				switch (i_playerID)
				{
					case 1:
						if (on_ground && Input.GetButton("Jump") && stamina > 0.0f && stamina > JumpCost)
						{
							Jump(PLAYER_JUMP);

							stamina -= JumpCost;
						}
						break;
					case 2:
						if (on_ground && Input.GetButton("Jump2") && stamina > 0.0f && stamina > JumpCost)
						{
							Jump(PLAYER_JUMP);

							stamina -= JumpCost;
						}
						break;
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

					if (reverse < 1.0f)
					{
						reversing = false;
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

						if (sprint > 0.0f && stamina > 0.0f && stamina > SprintCost && SprintCooldown <= 0.0f)
						{
							Headbutt();
						}

						movement += input_motion;

						if (handle != null)
						{
							handle.rotation = transform.rotation;
						}

						if (butt != null && back_handle != null)
						{
							butt.enabled = false;
							butt.transform.position = back_handle.position;
							//butt.enabled = true;
						}
					}
					else
					{
						reversing = true;
						Vector3 destination_point = handle.position + (Quaternion.Euler(0, theta, 0) * Vector3.forward * (max_distance * trail.Length-1)) + new Vector3(0.0f, butt.transform.position.y - handle.position.y, 0.0f);

						if(Reversing_Marker != null)
							Reversing_Marker.position = destination_point;

						butt.GetComponent<CharacterController>().enabled = false;
						butt.transform.position = back_handle.position;
						butt.GetComponent<CharacterController>().enabled = true;

						Vector3 butt_dir = (destination_point - butt.transform.position).normalized;
						float butt_dest_dist = (destination_point - butt.transform.position).magnitude;

						if (butt_dest_dist < PLAYER_SPEED * dt)
						{
							float diff = (PLAYER_SPEED * dt) - butt_dest_dist;

							butt.Move(butt_dir * butt_dest_dist);
							cc.Move((destination_point - handle.position).normalized * diff);
						}
						else
						{
							butt.Move(butt_dir * PLAYER_SPEED * dt);
						}

						back_handle.position = butt.transform.position;
						//butt.enabled = false;

						origins[origins.Length - 1] = back_handle.position;
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

		//if(!hit_obj && (moved || butt_moved))
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
				if (stamina >= MaxStamina)
                {
					stamina = MaxStamina;
					stamtimer -= Time.deltaTime;
				}
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
		if (stamina != MaxStamina)
        {
			stamtimer = stambartime;

		}

		if (staminabar != null)
		{
			staminabar.fillAmount = stamina / 100f;

			int temp = (int)stamina;

			if (StaminaDisplay != null)
				StaminaDisplay.text = temp.ToString();

			if (stamtimer <= 0)
			{
				//staminabar.gameObject
				fullstaminabar.SetActive(false);
			}
			else
			{
				fullstaminabar.SetActive(true);
			}
		}
		//int temp = (int)stamina;
		//StaminaDisplay.text = temp.ToString();


		//Cooldowns
		if(SprintCooldown > 0.0f)
        {
			SprintCooldown -= dt;
        }

		if(SprintCooldown < 0.0f)
        {
			SprintCooldown = 0.0f;
        }


		//Headbutt Duration
		if(SprintDuration > 0.0f)
        {
			SprintDuration -= dt;
        }

		if(SprintDuration <= 0.0f && state == FerretState.Headbutt)
        {
			SprintDuration = 0.0f;
			state = FerretState.Idle;
        }
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
		butt.enabled = false;
		butt.transform.position = back_handle.position;
		butt.enabled = true;

		back_up_vel += Physics.gravity * dt;

		CollisionFlags cf = butt.Move(back_up_vel * dt);

		if (cf.HasFlag(CollisionFlags.Below))
			back_up_vel = Vector3.zero;

		back_handle.transform.position = butt.transform.position;
	}

	private void AdjustTail()
	{
		Vector3[] new_pos = new Vector3[trail.Length];
		new_pos[0] = trail[0].position;

		// first pass, mainly determines the position of the skeleton.
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

		Vector3[] back_pass_pos = new Vector3[trail.Length]; // to store the back pass calculated positions.
		Vector3[] average_pos = new Vector3[trail.Length];

		// Test Intermediate pass
		if (reversing)
		{

			for (int c = trail.Length - 1; c >= 0; c--)
			{
				if (c == trail.Length - 1)
					back_pass_pos[c] = butt.transform.position;
				else
				{
					Vector3 dir = back_pass_pos[c + 1] - origins[c];

					back_pass_pos[c] = origins[c] + dir.normalized * (dir.magnitude - max_distance);
				}
			}

			for (int c = 1; c < trail.Length; c++)
			{
				average_pos[c] = (new_pos[c] + back_pass_pos[c]) / 2;
			}
		}
		else
		{
			average_pos = new_pos;
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

				t.position = average_pos[c];
			}
			else if (prev != null)
			{
				dir = (prev.position - t.position).normalized;

				local_up = (t.up + prev.up) / 2;

				t.position = average_pos[c];
			}
			else if (next != null)
			{
				local_up = (t.up + next.up) / 2;

				dir = (t.position - next.position).normalized;
			}

			/****************************
			 * Handle the main body's rotation.
			 * *************************/

			//float x_change = Vector2.Angle(new Vector2(dir.y, dir.z), new Vector2(start_dir[c].y, start_dir[c].z));
			//float y_change = Vector2.Angle(new Vector2(dir.x, dir.z), new Vector2(start_dir[c].x, start_dir[c].z));

			//t.rotation = start_rots[c];

			//Quaternion q = Quaternion.identity;
			//if (Vector3.Dot(start_dir[c], dir) < 1.0f && Vector3.Dot(start_dir[c], dir) > -1.0f)
			//{
			//	Vector3 cross = Vector3.Cross(start_dir[c], dir);

			//	q.x = cross.x;
			//	q.y = cross.y;
			//	q.z = cross.z;

			//	q.w = Mathf.Sqrt(start_dir[c].magnitude * start_dir[c].magnitude) * (dir.magnitude * dir.magnitude) + Vector3.Dot(start_dir[c], dir);
			//}

			//t.rotation = q * start_rots[c];
			//t.rotation = Quaternion.LookRotation(dir, head.up);
			t.rotation = Quaternion.LookRotation(dir, Vector3.up);

				//t.localRotation = Quaternion.Euler(t.localRotation.eulerAngles.x, 0.0f, t.localRotation.eulerAngles.z);

				//if (t.localRotation.eulerAngles.y > 5.0f || t.localRotation.eulerAngles.y < -5.0f)
				//{
				//	Debug.Log("HOI!!!!!!!");
				//}


			// Determine the Head's rotation.
			if (c == 0 && head != null)
			{
				if (reversing)
				{
					Vector3 inverse_dir3 = dir;
					Vector3 orientation_dir3 = Quaternion.Euler(0.0f, player_orientation, 0.0f) * Vector3.forward;

					Vector2 new_orientation = new Vector2(inverse_dir3.x, inverse_dir3.z);
					Vector2 cur_orientation = new Vector2(orientation_dir3.x, orientation_dir3.z);

					float diff = Vector2.SignedAngle(cur_orientation, new_orientation);

					player_orientation -= diff;
				}

				float look_t = Mathf.Lerp(0.0f, -LookTheta, climb_timer / LookTime);

				if (can_climb)
					head.rotation = Quaternion.Euler(look_t, player_orientation, 0.0f) * head_rot;
				else
					head.rotation = Quaternion.Euler(t.rotation.eulerAngles.x, player_orientation, t.rotation.eulerAngles.z) * head_rot;
			}


			// Apply The Starting Rotations of the ferret in order to maintain the desired shape.
			t.rotation *= start_rots[c];
		}
		
	}

	public void Headbutt()
    {
		if (state != FerretState.Headbutt)
		{
			state = FerretState.Headbutt;
			SprintCooldown = 3.0f; //How long before you can Headbutt again
			SprintDuration = 0.5f; //How long the headbutt lasts
			stamina -= SprintCost;
		}
		
		
    }

	public void DropItem()
	{
		if(grabber != null)
			grabber.Drop();
	}


	/************************************
	 *		Ragdoll Functions			*
	 ************************************/

	// Ragdoll Enabled
	/*
	 *	- enables/disables Ragdoll
	 */
	public void RagdollEnabled(bool b)
	{
		Ragdoll.gameObject.SetActive(b);
	}

	// Ragdoll Setup
	/*
	 * - Sets the State to Ragdoll
	 * - Initializes the Ragdoll Joint positions to match the current skeleton position.
	 */
	public void SetupRagdoll()
	{
		state = FerretState.Ragdoll;

		RagdollMatch(SkeletonRoot, Ragdoll, true);
	}

	//	Apply Ragdoll
	/*
	 * - Matches the current skeleton to the ragdoll's transforms.
	 */
	private void ApplyRagdoll()
	{
		RagdollMatch(SkeletonRoot, Ragdoll);
	}

	// Ragdoll Match
	/*
	 * Inputs:
	 *	Transform skeleton	- Transform of the skeleton's bone
	 *	Transform ragdoll	- Transform of the Ragdoll's associated bone
	 *	bool rag_to_skel	- Boolean will determine which bone is being modified, true = the ragdoll is modified to match the skeleton; false = the skeleton is modified to match the ragdoll.
	 *	
	 * - Recursive, returns if skeleton or ragdoll has no children.
	 * - changes the transform of either skeleton or ragdoll to match the other based on rag_to_bone.
	 * - Calls RagdollMatch for each child of skeleton if ragdoll has an associated child.
	 */
	private void RagdollMatch(Transform skeleton, Transform ragdoll, bool rag_to_skel = false)
	{
		Transform associated = ragdoll.Find(skeleton.gameObject.name);

		if (associated == null)
			return;

		if (rag_to_skel)
		{
			associated.position = skeleton.position;
			associated.rotation = skeleton.rotation;
		}
		else
		{
			skeleton.position = associated.position;
			skeleton.rotation = associated.rotation;
		}

		for (int c = 0; c < skeleton.childCount; c++)
		{
			RagdollMatch(skeleton.GetChild(c), ragdoll, rag_to_skel);
		}
	}

	public float GetPlayerOrientation()
	{
		return player_orientation;
	}
}
