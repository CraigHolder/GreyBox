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

	public float orientation = 0.0f;
	Vector3[] origin_pos;

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
}
