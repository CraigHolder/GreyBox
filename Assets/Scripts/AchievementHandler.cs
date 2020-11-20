using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementHandler : MonoBehaviour
{
    public TextAsset T_savefile;

    void Start()
    {
        if (T_savefile == null)
        {
            T_savefile = new TextAsset();
        }
    }

    public void RecordAchievement(Observer.EventType e_event)
    {
        Debug.Log(T_savefile.text);
        

    }
}
