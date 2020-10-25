using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Checkpoint : MonoBehaviour
{
    Vector3 vec3_checkpoint;
    public int i_checkpointID;
    public PlayerMovement s_playerscript;
    public PluginController s_plugin;
    bool b_activated = false;
    public Text t_timer;
    public bool b_finish = false;

    void Start()
    {
        vec3_checkpoint = transform.position;
    }
    void OnTriggerEnter(Collider collision)
    {
        if(b_activated == false)
        {
            s_playerscript.vec3_checkpoint = vec3_checkpoint;
            float currentTime = Time.time;
            float checkpointTime = currentTime - s_plugin.lastTime;
            s_plugin.lastTime = currentTime;

            s_plugin.SaveTime(checkpointTime);
            t_timer.text = s_plugin.LoadTime(i_checkpointID).ToString();
            b_activated = true;
            if (b_finish == true)
            {
                SceneManager.LoadScene("EndScene");
            }

        }
    }
}
