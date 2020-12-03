using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeTrial : MonoBehaviour
{
    public float f_timeremaining;
    player_controller_behavior s_player;
    public Text t_scoretext;
    public Text t_timetext;
    public Button t_mainmenubutton;
    Command c_command;
    public Nest s_nest;

    // Start is called before the first frame update
    void Start()
    {
        s_player = GameObject.FindGameObjectWithTag("Player").GetComponent<player_controller_behavior>();
        t_scoretext.gameObject.SetActive(false);
        t_mainmenubutton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(f_timeremaining <= 0f)
        {
            t_timetext.text = "Time: 0";
            t_scoretext.gameObject.SetActive(true);
            t_mainmenubutton.gameObject.SetActive(true);

            t_scoretext.text = "Final Score: " + s_nest.i_teamscore.ToString();
            s_player.PLAYER_SPEED = 0;
            s_player.PLAYER_JUMP = 0;

            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            f_timeremaining -= Time.deltaTime;
            t_timetext.text = "Time: " + f_timeremaining.ToString();
        }
    }

    public void GotoMainMenu()
    {
        c_command = new GotoMainMenuCommand();
        c_command.Execute(c_command, s_player.gameObject);
    }
}
