using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class climbwallBehavior : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
	if (other.gameObject.tag == "Player")
		{
			player_controller_behavior pc = other.gameObject.GetComponent<player_controller_behavior>();
			if (pc != null)
				pc.PlayerCanClimb(true);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			player_controller_behavior pc = other.gameObject.GetComponent<player_controller_behavior>();
			if (pc != null)
				pc.PlayerCanClimb(false);
		}
	}
}
