using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    public GameObject obj_player;
    CharacterController c_controller;
    PlayerMovement s_playerscript;

    void Start()
    {
        c_controller = obj_player.GetComponent<CharacterController>();
        s_playerscript = obj_player.GetComponent<PlayerMovement>();
    }

    void OnTriggerEnter(Collider collision)
    {
        c_controller.enabled = false;
        obj_player.transform.position = s_playerscript.vec3_checkpoint;
        c_controller.enabled = true;
    }
    
}
