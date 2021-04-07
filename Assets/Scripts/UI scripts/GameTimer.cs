using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public const float f_MAXTIME = 60.0f;
    public float f_timeremaining;
    public const float f_MAXSECONDS = 60.0f;
    public float f_curSeconds;
    player_controller_behavior s_player;
    public Text t_Winner;
    public Text t_scoretext;
    public RawImage hat;
    public Text t_timetext;
    public Button t_mainmenubutton;
    Command c_command;
    public Nest s_nestR;
    public Nest s_nestB;



    // Start is called before the first frame update
    void Start()
    {
        f_timeremaining = f_MAXTIME;
        f_curSeconds = 60.0f;
        s_player = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<player_controller_behavior>();
        t_scoretext.gameObject.SetActive(false);
        t_Winner.gameObject.SetActive(false);
        t_mainmenubutton.gameObject.SetActive(false);
        hat.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKey(KeyCode.Tab))
        //{
        //    c_command = new GotoMainMenuCommand();
        //    c_command.Execute(c_command, this.gameObject);
        //}

        if (f_timeremaining <= 0f)
        {
            t_timetext.text = "Time: 0:00";
            t_scoretext.gameObject.SetActive(true);
            t_Winner.gameObject.SetActive(true);
            t_mainmenubutton.gameObject.SetActive(true);
            hat.gameObject.SetActive(true);

            if (s_nestB.i_teamscore > s_nestR.i_teamscore)
            {
                t_Winner.text = "Blue Team Wins!";
                t_scoretext.text = "Final Score: " + s_nestB.i_teamscore.ToString();
            }
            else if (s_nestB.i_teamscore < s_nestR.i_teamscore)
            {
                t_Winner.text = "Red Team Wins!";
                t_scoretext.text = "Final Score: " + s_nestR.i_teamscore.ToString();
            }
            else
            {
                t_scoretext.text = "It's a Tie!";
            }

            s_player.PLAYER_SPEED = 0;
            s_player.PLAYER_JUMP = 0;

            Cursor.lockState = CursorLockMode.Confined;
            GotoLobby();
        }
        else
        {
            if (f_curSeconds <= 0)
                f_curSeconds = f_MAXSECONDS;

            f_timeremaining -= Time.deltaTime;
            f_curSeconds -= Time.deltaTime;

            int tempM = (int)(f_timeremaining / 60);
            int tempS = (int)f_curSeconds;
            if (tempS >= 10)
                t_timetext.text = "Time: " + tempM.ToString() + ":" + tempS.ToString();
            else
                t_timetext.text = "Time: " + tempM.ToString() + ":0" + tempS.ToString();
        }
    }

    public void GotoLobby()
    {
        c_command = new GotoLobbySceneCommand();
        c_command.Execute(c_command, s_player.gameObject);
    }
}
