using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    Command c_command;
    GameObject obj_placeholder;

    public List<Text> L_AchievmentText = new List<Text>();
    //public TextAsset T_savefile;

    void Start()
    {
        if (L_AchievmentText.Count >= 1)
        {
            AchievementTextControl();
        }
        Cursor.lockState = CursorLockMode.Confined;
    }

    void AchievementTextControl()
    {
        L_AchievmentText[0].color =
                new Color(0.5f + (0.5f * PlayerPrefs.GetInt(Observer.EventType.Tutorial.ToString(), 0)),
                0.5f + (0.5f * PlayerPrefs.GetInt(Observer.EventType.Tutorial.ToString(), 0)),
                0.5f + (0.5f * PlayerPrefs.GetInt(Observer.EventType.Tutorial.ToString(), 0)),
                1);
        L_AchievmentText[1].color =
            new Color(0.5f + (0.5f * PlayerPrefs.GetInt(Observer.EventType.PickupObject.ToString(), 0)),
            0.5f + (0.5f * PlayerPrefs.GetInt(Observer.EventType.PickupObject.ToString(), 0)),
            0.5f + (0.5f * PlayerPrefs.GetInt(Observer.EventType.PickupObject.ToString(), 0)),
            1);
        L_AchievmentText[2].color =
            new Color(0.5f + (0.5f * PlayerPrefs.GetInt(Observer.EventType.Bounce.ToString(), 0)),
            0.5f + (0.5f * PlayerPrefs.GetInt(Observer.EventType.Bounce.ToString(), 0)),
            0.5f + (0.5f * PlayerPrefs.GetInt(Observer.EventType.Bounce.ToString(), 0)),
            1);
        L_AchievmentText[3].color =
            new Color(0.5f + (0.5f * PlayerPrefs.GetInt(Observer.EventType.Push.ToString(), 0)),
            0.5f + (0.5f * PlayerPrefs.GetInt(Observer.EventType.Push.ToString(), 0)),
            0.5f + (0.5f * PlayerPrefs.GetInt(Observer.EventType.Push.ToString(), 0)),
            1);
        L_AchievmentText[4].color =
            new Color(0.5f + (0.5f * PlayerPrefs.GetInt(Observer.EventType.Remote.ToString(), 0)),
            0.5f + (0.5f * PlayerPrefs.GetInt(Observer.EventType.Remote.ToString(), 0)),
            0.5f + (0.5f * PlayerPrefs.GetInt(Observer.EventType.Remote.ToString(), 0)),
            1);
        L_AchievmentText[5].color =
            new Color(0.5f + (0.5f * PlayerPrefs.GetInt(Observer.EventType.Return.ToString(), 0)),
            0.5f + (0.5f * PlayerPrefs.GetInt(Observer.EventType.Return.ToString(), 0)),
            0.5f + (0.5f * PlayerPrefs.GetInt(Observer.EventType.Return.ToString(), 0)),
            1);
        L_AchievmentText[6].color =
            new Color(0.5f + (0.5f * PlayerPrefs.GetInt(Observer.EventType.Steal.ToString(), 0)),
            0.5f + (0.5f * PlayerPrefs.GetInt(Observer.EventType.Steal.ToString(), 0)),
            0.5f + (0.5f * PlayerPrefs.GetInt(Observer.EventType.Steal.ToString(), 0)),
            1);
    }


    public void GotoTestScene()
    {
        c_command = new GotoTestSceneCommand();
        c_command.Execute(c_command, obj_placeholder);
    }

    public void GotoTutorial()
    {
        c_command = new GotoTutorialCommand();
        c_command.Execute(c_command, obj_placeholder);
    }

    public void GotoAchievements()
    {
        c_command = new GotoAchievementCommand();
        c_command.Execute(c_command, obj_placeholder);
    }

    public void GotoMainMenu()
    {
        c_command = new GotoMainMenuCommand();
        c_command.Execute(c_command, obj_placeholder);
    }

    public void QuitProgram()
    {
        c_command = new QuitCommand();
        c_command.Execute(c_command, obj_placeholder);
    }

    public void ResetAchieve()
    {
        c_command = new ResetAchievementsCommand();
        c_command.Execute(c_command, obj_placeholder);
        AchievementTextControl();
    }
}
