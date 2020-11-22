using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


abstract public class Command
{
    virtual public void Execute(Command command, GameObject obj_selected) { }
    public GameObject obj_Controlled = new GameObject();

    public virtual void Undo() { }
    public virtual void Redo() { }

    public static List<Command> L_previouscommands = new List<Command>();
    public static int i_Commandpos = 0;

    public static bool b_undotracker = false;
}

public class BounceObjCommand : Command
{
    override public void Execute(Command command, GameObject obj_selected)
    {
        if (b_undotracker == true)
        {
            if ((L_previouscommands.Count + i_Commandpos) < L_previouscommands.Count)
            {
                L_previouscommands.Clear();
                i_Commandpos = 0;
                b_undotracker = false;
            }
        }

        obj_Controlled = obj_selected;
        BounceObj();
        L_previouscommands.Add(command);
    }

    override public void Redo()
    {
        BounceObj();
    }

    override public void Undo()
    {
        //obj_Controlled.transform.Rotate(-5, 0, 0);
    }

    void BounceObj()
    {
        obj_Controlled.GetComponent<Rigidbody>().AddForce(new Vector3(0,100,0));
    }
}

public class GotoMainMenuCommand : Command
{
    override public void Execute(Command command, GameObject obj_selected)
    {
        if (b_undotracker == true)
        {
            if ((L_previouscommands.Count + i_Commandpos) < L_previouscommands.Count)
            {
                L_previouscommands.Clear();
                i_Commandpos = 0;
                b_undotracker = false;
            }
        }

        obj_Controlled = obj_selected;
        ToMainMenu();
        L_previouscommands.Add(command);
    }

    override public void Redo()
    {
        ToMainMenu();
    }

    override public void Undo()
    {
        //obj_Controlled.transform.Rotate(-5, 0, 0);
    }

    void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

public class QuitCommand : Command
{
    override public void Execute(Command command, GameObject obj_selected)
    {
        if (b_undotracker == true)
        {
            if ((L_previouscommands.Count + i_Commandpos) < L_previouscommands.Count)
            {
                L_previouscommands.Clear();
                i_Commandpos = 0;
                b_undotracker = false;
            }
        }

        obj_Controlled = obj_selected;
        QuitProgram();
        L_previouscommands.Add(command);
    }

    override public void Redo()
    {
        QuitProgram();
    }

    override public void Undo()
    {
        //obj_Controlled.transform.Rotate(-5, 0, 0);
    }

    void QuitProgram()
    {
        Application.Quit();
    }
}
public class GotoTestSceneCommand : Command
{
    override public void Execute(Command command, GameObject obj_selected)
    {
        if (b_undotracker == true)
        {
            if ((L_previouscommands.Count + i_Commandpos) < L_previouscommands.Count)
            {
                L_previouscommands.Clear();
                i_Commandpos = 0;
                b_undotracker = false;
            }
        }

        obj_Controlled = obj_selected;
        ToTestScene();
        L_previouscommands.Add(command);
    }

    override public void Redo()
    {
        ToTestScene();
    }

    override public void Undo()
    {
        //obj_Controlled.transform.Rotate(-5, 0, 0);
    }

    void ToTestScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}

public class GotoTutorialCommand : Command
{
    override public void Execute(Command command, GameObject obj_selected)
    {
        if (b_undotracker == true)
        {
            if ((L_previouscommands.Count + i_Commandpos) < L_previouscommands.Count)
            {
                L_previouscommands.Clear();
                i_Commandpos = 0;
                b_undotracker = false;
            }
        }

        obj_Controlled = obj_selected;
        ToTutorialScene();
        L_previouscommands.Add(command);
    }

    override public void Redo()
    {
        ToTutorialScene();
    }

    override public void Undo()
    {
        //obj_Controlled.transform.Rotate(-5, 0, 0);
    }

    void ToTutorialScene()
    {
        SceneManager.LoadScene("CraigTutorialScene");
    }
}

public class GotoAchievementCommand : Command
{
    override public void Execute(Command command, GameObject obj_selected)
    {
        if (b_undotracker == true)
        {
            if ((L_previouscommands.Count + i_Commandpos) < L_previouscommands.Count)
            {
                L_previouscommands.Clear();
                i_Commandpos = 0;
                b_undotracker = false;
            }
        }

        obj_Controlled = obj_selected;
        ToAchievement();
        L_previouscommands.Add(command);
    }

    override public void Redo()
    {
        ToAchievement();
    }

    override public void Undo()
    {
        //obj_Controlled.transform.Rotate(-5, 0, 0);
    }

    void ToAchievement()
    {
        SceneManager.LoadScene("AchievementScene");
    }
}

public class ResetAchievementsCommand : Command
{
    override public void Execute(Command command, GameObject obj_selected)
    {
        if (b_undotracker == true)
        {
            if ((L_previouscommands.Count + i_Commandpos) < L_previouscommands.Count)
            {
                L_previouscommands.Clear();
                i_Commandpos = 0;
                b_undotracker = false;
            }
        }

        obj_Controlled = obj_selected;
        ResetAchievements();
        L_previouscommands.Add(command);
    }

    override public void Redo()
    {
        ResetAchievements();
    }

    override public void Undo()
    {
        //obj_Controlled.transform.Rotate(-5, 0, 0);
    }

    void ResetAchievements()
    {
        PlayerPrefs.DeleteKey(Observer.EventType.Tutorial.ToString());
        PlayerPrefs.DeleteKey(Observer.EventType.PickupObject.ToString());
        PlayerPrefs.DeleteKey(Observer.EventType.Bounce.ToString());
        PlayerPrefs.DeleteKey(Observer.EventType.Push.ToString());
        PlayerPrefs.DeleteKey(Observer.EventType.Remote.ToString());
        PlayerPrefs.DeleteKey(Observer.EventType.Return.ToString());
        PlayerPrefs.DeleteKey(Observer.EventType.Steal.ToString());
    }
}

public class UndoCommand : Command
{
    override public void Execute(Command command, GameObject obj_selected)
    {
        //checks to make sure that there are commands in the list and that command pos is not an invalid number
        if(L_previouscommands.Count >= 1 && (L_previouscommands.Count - 1) + i_Commandpos >= 0)
        {
            //gets the command to undo
            Command c_last = L_previouscommands[(L_previouscommands.Count - 1) + i_Commandpos];
            c_last.Undo();
            //moves the position in the list
            i_Commandpos -= 1;
            //triggers the tracker
            b_undotracker = true;

            //L_previouscommands.RemoveAt(L_previouscommands.Count - 1);

        }
    }

    override public void Redo()
    {
        
    }

    override public void Undo()
    {
        
    }

}

public class RedoCommand : Command
{
    override public void Execute(Command command, GameObject obj_selected)
    {
        if (L_previouscommands.Count >= 1 && ((L_previouscommands.Count - 1) + i_Commandpos) < (L_previouscommands.Count - 1))
        {
            i_Commandpos += 1;
            Command c_last = L_previouscommands[(L_previouscommands.Count - 1) + i_Commandpos];
            
            c_last.Redo();
            
            
            //L_previouscommands.RemoveAt(L_previouscommands.Count - 1);

        }
    }

    override public void Redo()
    {

    }

    override public void Undo()
    {

    }

}
