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

	[Header("Accessories")]
	public GameObject player;
	public GameObject[] bodyItems;
	private int bCurrentItem;
	public GameObject[] hatItems;
	private int hCurrentItem;
	public GameObject[] maskItems;
	private int mCurrentItem;

	public GameObject orb;

	private int i_BlueColour;
	private int i_RedColour;
	private int i_Skin;

	bool b_Accessoriezed = false;

	//TEMP VARIABLES:
	public int i_PlayerTeam = 0;

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

	public void SetCosmetics(int b, int h, int m, int BC, int RC, int S, int T)
    {
		bCurrentItem = b;
		hCurrentItem = h;
		mCurrentItem = m;

		i_BlueColour = BC;
		i_RedColour = RC;
		i_Skin = S;

		i_PlayerTeam = T;
		if (!b_Accessoriezed)
		{
			//Loops through and turns on all of the chosen accessories.
			for (int i = 0; i < 3; i++)
			{
				switch (i)
				{
					case 0:
						Activate(0, bCurrentItem, bodyItems);
						break;
					case 1:
						Activate(1, hCurrentItem, hatItems);
						break;
					case 2:
						Activate(2, mCurrentItem, maskItems);
						break;
				}
			}
			b_Accessoriezed = true;
		}
	}

	public void ApplyColour()
	{
		//First gives the player their fur colour.
		switch (i_Skin)
		{
			case 0:
				player.GetComponent<SkinnedMeshRenderer>().material = Resources.Load<Material>("Materials/Ferret/FerretBaseColour");
				break;
			case 1:
				player.GetComponent<SkinnedMeshRenderer>().material = Resources.Load<Material>("Materials/Ferret/FerretAlbino");
				break;
			case 2:
				player.GetComponent<SkinnedMeshRenderer>().material = Resources.Load<Material>("Materials/Ferret/FerretDarkBrown");
				break;
		}

		string s_TeamColour = "";

		//Learns the players team and uses that to determine which colour to set the temp variable to.
		if (i_PlayerTeam == 0)
		{
			switch (i_RedColour)
			{
				case 0:
					s_TeamColour = "Red";
					break;
				case 1:
					s_TeamColour = "Orange";
					break;
				case 2:
					s_TeamColour = "Yellow";
					break;
			}
		}
		else
		{
			switch (i_BlueColour)
			{
				case 0:
					s_TeamColour = "Blue";
					break;
				case 1:
					s_TeamColour = "Green";
					break;
				case 2:
					s_TeamColour = "Purple";
					break;
			}
		}

		/*****************************/
		/*       VERY IMPORTANT      */
		/*****************************/

		//applies all cosmetics with their colour value mats to the player.
		// //bodyItems;
		bodyItems[0].GetComponent<SkinnedMeshRenderer>().material = Resources.Load<Material>("Materials/Cape/Cape" + s_TeamColour);
		bodyItems[1].GetComponent<SkinnedMeshRenderer>().material = Resources.Load<Material>("Materials/Dragon_Wings/Dragon_Wings" + s_TeamColour);
		// //maskItems;
		maskItems[0].GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/Mask/Mask" + s_TeamColour);
		maskItems[1].GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/Goggles/Goggles" + s_TeamColour);
		// //hatItems;
		hatItems[0].GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/Tophat/Tophat" + s_TeamColour);
		hatItems[1].GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/ArcherHat/ArcherHat" + s_TeamColour);


		orb.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/TeamColors/" + s_TeamColour);
		//TopHat.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/Tophat" + s_TeamColour);
		//DragonWings.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/Dragon_Wings" + s_TeamColour);
		//Cape.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/Cape" + s_TeamColour);
		//Mask.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/Mask" + s_TeamColour);
		//ArcherHat.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/ArcherHat" + s_TeamColour);
	}

	public void Activate(int part, int currentItem, GameObject[] items)
	{
		ApplyColour();
		//After gaining the colours, activates correct accessories and disables all others.
		for (int c = 0; c < items.Length; c++)
		{
			if (c == currentItem)
			{
				//If accessories are unactive when this is used, doesnt work for some reason??
				items[c].SetActive(true);
				//items[c].transform.rotation = player.transform.rotation;
			}
			else
			{
				items[c].SetActive(false);
			}
		}
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
