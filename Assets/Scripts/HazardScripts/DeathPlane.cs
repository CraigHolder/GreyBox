using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    public GameObject obj_player;
    CharacterController c_controller;
    PlayerMovement s_playerscript;
    Command c_command;
    public bool b_active = false;

    public bool b_returnmenu = false;

    void Start()
    {
        obj_player = GameObject.FindGameObjectWithTag("Player");
        c_controller = obj_player.GetComponent<CharacterController>();
        s_playerscript = obj_player.GetComponent<PlayerMovement>();

    }

    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            b_active = true;
            if (b_returnmenu == true)
            {
                c_command = new GotoMainMenuCommand();
                c_command.Execute(c_command, obj_player);
            }
            else
            {
                c_controller.enabled = false;
                obj_player.transform.position = s_playerscript.vec3_checkpoint;
                c_controller.enabled = true;
            }
        }
        
    }
    
}
