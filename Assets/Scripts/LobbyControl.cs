using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyControl : MonoBehaviour
{
    Command c_command;
    GameObject obj_placeholder;

    public InputField ipfield;

    //public List<Text> L_AchievmentText = new List<Text>();
    //public TextAsset T_savefile;

    void Start()
    {
        //if (L_AchievmentText.Count >= 1)
        //{
        //    AchievementTextControl();
        //}
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        //PlayerPrefs.SetInt("Mask", mCurrentItem);
        PlayerPrefs.SetString("IPConnect", ipfield.text);
    }


    public void GotoTestScene()
    {
        c_command = new GotoTestSceneCommand();
        c_command.Execute(c_command, obj_placeholder);
    }

    public void GotoLobbyScene()
    {
        c_command = new GotoLobbySceneCommand();
        c_command.Execute(c_command, obj_placeholder);
    }

    public void GotoCharacterScene()
    {
        c_command = new GotoCharacterSceneCommand();
        c_command.Execute(c_command, obj_placeholder);
    }

    public void GotoServerScene()
    {
        c_command = new GotoServerSceneCommand();
        c_command.Execute(c_command, obj_placeholder);
    }
    public void GotoClientScene()
    {
        c_command = new GotoClientSceneCommand();
        c_command.Execute(c_command, obj_placeholder);
    }

    public void GotoTutorial()
    {
        c_command = new GotoTutorialCommand();
        c_command.Execute(c_command, obj_placeholder);
    }

    public void GotoCredits()
    {
        c_command = new GotoCreditsCommand();
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

    public void GotoObjectEditor()
    {
        c_command = new GotoObjectEditorCommand();
        c_command.Execute(c_command, obj_placeholder);
    }

    public void QuitProgram()
    {
        c_command = new QuitCommand();
        c_command.Execute(c_command, obj_placeholder);
    }

}
