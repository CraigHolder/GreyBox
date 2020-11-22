using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class climbwallBehavior : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
	if (other.gameObject.tag == "Player")
		{
			other.gameObject.GetComponent<player_controller_behavior>().PlayerCanClimb(true);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			other.gameObject.GetComponent<player_controller_behavior>().PlayerCanClimb(false);
		}
	}
}
